using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;


namespace BrainsTypes
{
    public class StandartBrain : CreatureLogic
    {

        private int current;

        /// <summary>
        /// Current command index
        /// </summary>
        public int CurrentIndex
        {
            get => current;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                current = value;

                //Max value for current is 63(Logic.Leght is 64)
                while (CurrentIndex >= 64)
                    CurrentIndex -= 64;
            }

        }

        /// <summary>
        /// Brain of creature
        /// </summary>
        public int[] Logic
        {
            get => logic;
            set
            {

                if (value == null)
                    throw new ArgumentNullException();

                if (value.Length != 64)
                    throw new ArgumentException();

                logic = value;
            }

        }
        private int[] logic;


        public StandartBrain()
        {
            CurrentIndex = 0;
            Logic = new int[64];
        }

        /// <summary>
        /// Не должно быть больше десяти раз вызываться думанье подряд
        /// </summary>
        private int trys { get; set; } = 0;

        /// <summary>
        /// Check next action
        /// </summary>
        /// <returns>What is next action</returns>
        public override Doing Think()
        {
            if (!RecivedResult)
                throw new Exception("You cant do it while u have not give the result from last try");

            var cur = Logic[CurrentIndex];

            if(trys==10)
            {
                Ready = true;
                RecivedResult = true;
                trys = 0;
                return new Doing(Doing.Actions.Nothing, Doing.Rotate.Zero);
            }
            trys++;

           
            if (cur >= 0 && cur < 8)
            {
                RecivedResult = false;
                Ready = true;
                return new Doing(Doing.Actions.Go, (Doing.Rotate)cur);
            }
            else if (cur >= 8 && cur < 16)
            {
                RecivedResult = false;
                Ready = true;
                return new Doing(Doing.Actions.Catch, (Doing.Rotate)(cur - 8));
            }
            else if (cur >= 16 && cur < 24)
            {
                RecivedResult = false;
                return new Doing(Doing.Actions.See, (Doing.Rotate)(cur - 16));
            }
            else if (cur >= 24 && cur < 32)
            {
                CurrentIndex += 1;
               
                return new Doing(Doing.Actions.Rotate, (Doing.Rotate)(cur - 24));
            }
            else
            {
                CurrentIndex += cur;
                return new Doing(Doing.Actions.Nothing, Doing.Rotate.Zero);
            }
           
        }

        public override void ReciveResult(WorldObject typeobject)
        {
            RecivedResult = true;
            
            if (typeobject is Poison)
                CurrentIndex++;
            else if (typeobject is Wall)
                CurrentIndex += 2;
            else if (typeobject is CreatureBody)
                CurrentIndex += 3;
            else if (typeobject is Food)
                CurrentIndex += 4;
            else if (typeobject is null)
                CurrentIndex += 5;
            else
            {
                throw new Exception("WTF what type is it");
            }

        }

        public override CreatureLogic Clone()
        {
            var z = new StandartBrain();
            z.CurrentIndex = this.CurrentIndex;
            z.Logic = new int[64];
            for (int i = 0; i < logic.Length; i++)
            {
                z.Logic[i] = logic[i];
            }
            return z;
        }
    }
}
