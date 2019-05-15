
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;


namespace BrainsTypes
{
    public class StandartBrain : CreatureBrain
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
                    throw new System.ArgumentOutOfRangeException();
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
                    throw new System.ArgumentNullException();

                if (value.Length != 64)
                    throw new System.ArgumentException();

                logic = value;
            }

        }
        private int[] logic;


        public StandartBrain()
        {
            CurrentIndex = 0;
            Logic = RandomBrain();
        }



        private static int[] RandomBrain()
        {
            var temp = new int[64];
            System.Random random = new System.Random();
            for (int i = 0; i < 64; i++)
            {
                temp[i] = random.Next(0, 63);
            }
            return temp;
        }

        /// <summary>
        /// Не должно быть больше десяти раз вызываться думанье подряд
        /// </summary>
        private int trys { get; set; } = 0;

        /// <summary>
        /// Check next action
        /// </summary>
        /// <param name="near">Near objects</param>
        /// <returns>What is next action</returns>
        public override Action Think(WorldObject[] near,CreatureBody.SeeDirection direction)
        {
            

            var cur = Logic[CurrentIndex];

            if(trys==10)
            {
                trys = 0;
                return new Action(Action.ActionType.Nothing, Action.RotateTimes.Zero);
            }
            trys++;
            
           
            if (cur >= 0 && cur < 8)
            {
                CurrentIndex += AddToIndex(WhatInCell(near,direction,(Action.RotateTimes)cur));
                trys = 0;
                return new Action(Action.ActionType.Move, (Action.RotateTimes)cur);
            }
            else if (cur >= 8 && cur < 16)
            {
                CurrentIndex += AddToIndex(WhatInCell(near,direction,(Action.RotateTimes)(cur-8)));
                trys = 0;

                return new Action(Action.ActionType.Catch, (Action.RotateTimes)(cur - 8));
            }
            else if (cur >= 16 && cur < 24)
            {
                CurrentIndex += AddToIndex(WhatInCell(near,direction,(Action.RotateTimes)(cur-16)));
              

                return Think(near,direction);
            }
            else if (cur >= 24 && cur < 32)
            {
                CurrentIndex += 1;
               

                return Think(near,direction);
            }
            else
            {
                CurrentIndex += cur;
                Think(near, direction);
            }
            return new Action();
        }

        /// <summary>
        /// This method controls input from the world
        /// </summary>
        /// <param name="worldObject">object for checking</param>
        /// <returns>What we must add to index</returns>
        private static int AddToIndex(WorldObject worldObject)
        {
            if (worldObject is Poison)
                return 1;
            else if (worldObject is Wall)
                return 2;
            else if (worldObject is CreatureBody)
                return 3;
            else if (worldObject is Food)
                return 4;
            else if (worldObject is null)
                return 5;
            else
                throw new System.Exception();
        }

        public override CreatureBrain Clone()
        {
            var z = new StandartBrain();
            z.CurrentIndex = 0;
            z.Logic = new int[64];
            for (int i = 0; i < logic.Length; i++)
            {
                z.Logic[i] = Logic[i];
            }
            return z;
        }
    }
}
