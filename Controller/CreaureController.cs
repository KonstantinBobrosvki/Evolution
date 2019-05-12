using System;
using System.Collections.Generic;
using Modal;
using BrainsTypes;

namespace Controller
{
    public class CreatureController
    {
        /// <summary>
        /// Body of current creature
        /// </summary>
        public CreatureBody Body { get; private set; }

        /// <summary>
        /// Logic centre 
        /// </summary>
        public CreatureLogic Brain { get; private set; }

        /// <summary>
        /// The map for all living creatures
        /// </summary>
        public static MapController WorldMap { get; set; }

        #region Constructors
        /// <summary>
        /// Standart constructor
        /// </summary>
        public CreatureController()
        {
            Body = new CreatureBody() { Health = 20, SeeDirection = 1 };
           

            Brain= NewBrain();
        }

        public CreatureController(CreatureBody body,CreatureLogic brain)
        {
            if (body is null || brain is null)
                throw new ArgumentNullException();

            Body = body;
            Brain = brain;
        }

        public CreatureController(CreatureBody body)
        {
            Body = body;
            Brain=  NewBrain();

        }
        #endregion
        public CreatureController Clone()
        {
            var clone = new CreatureController();
            clone.Body.Health = this.Body.Health;
            clone.Body.SeeDirection =this.Body.SeeDirection;

            clone.Brain = this.Brain.Clone();


            return clone;
        }

        public void Do()
        {
            var act = Brain.Think();

            switch (act.actions)
            {
                case Doing.Actions.Catch:

                    break;
            }

        }

        private static CreatureLogic NewBrain()
        {
            var Brain = new StandartBrain();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Brain.Logic.Length; i++)
            {
                Brain.Logic.SetValue(rnd.Next(0, 64), i);
            }
            return Brain;
           
        }
    }
}
