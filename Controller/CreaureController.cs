using System;
using System.Collections.Generic;
using Modal;
using BrainsTypes;

namespace Controller
{
    public  class CreatureController
    {
        /// <summary>
        /// Body of current creature
        /// </summary>
        public CreatureBody Body { get; private set; }

        /// <summary>
        /// Logic centre 
        /// </summary>
        public CreatureBrain Brain { get; private set; }

        /// <summary>
        /// The map for all living creatures
        /// </summary>
        public static MapController WorldMap { get; set; }

        private WorldObject[] Near {

            get
            {
                var d = new List<WorldObject>(9);
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i==0&&j==0)
                        {
                            continue;
                        }
                        d.Add(WorldMap[Body.X + i, Body.Y + j]);
                    }
                }

                return d.ToArray();

            }
        }

        #region Constructors
        /// <summary>
        /// Standart constructor
        /// </summary>
        public CreatureController()
        {
            Body = new CreatureBody() { Health = 20, Sight = CreatureBody.SeeDirection.Top };
            Brain = new StandartBrain();

            Body.DieEvent += Dead;
           
        }

        public CreatureController(CreatureBody body, CreatureBrain brain)
        {
            if (body is null || brain is null)
                throw new ArgumentNullException();

            Body = body;
            Body.DieEvent += Dead;
            Brain = brain;
        }

        public CreatureController(CreatureBody body)
        {
            Body = body;
            Brain = new StandartBrain() ;
            Body.DieEvent += Dead;
        }
        #endregion
        public CreatureController Clone()
        {
            var clone = new CreatureController();
            clone.Body.Health = this.Body.Health;
            clone.Body.Sight =this.Body.Sight;

            clone.Brain = this.Brain.Clone();


            return clone;
        }

        #region Methods for interacting with world
        /// <summary>
        /// Do what it should do acording to brain
        /// </summary>
        public void Live()
        {
            

          var act=  Brain.Think(Near,Body.Sight);
            
            switch (act.Act)
            {
                case Modal.Action.ActionType.Catch:
                    Catch(act.Rotate);
                    break;
                case Modal.Action.ActionType.Move:
                    Move(act.Rotate);
                    break;
                default:

                    break;
                    
            }
            Body.Health--;
        }


        private void Move(Modal.Action.RotateTimes rotate)
        {
            var temp = (int)rotate + (int)Body.Sight;
            while (temp >= 8)
                temp -= 8;

            var cell = Near[temp];
            if(cell==null)
            {
                WorldMap[Body.X, Body.Y] = null;

                var tempo = GetPosition(temp);
                Body.X = tempo.Item1;
                Body.Y = tempo.Item2;

                WorldMap[Body.X,Body.Y] = this.Body;
                return;
            }
            if (Near[temp].CanMoveTrought)
            {
                WorldMap[Body.X, Body.Y] = null;


                Body.X = cell.X;
                Body.Y = cell.Y;

                WorldMap[cell.X, cell.Y] = this.Body;
            }
            Body.Health += cell.HealthAfterInteract;

        }

        private void Catch(Modal.Action.RotateTimes rotate)
        {
            var temp = (int)rotate + (int)Body.Sight;
            while (temp >= 8)
                temp -= 8;

            var cell = Near[temp];

            if(cell is Poison)
            {
                WorldMap[cell.X, cell.Y] = new Food(cell.X, cell.Y);
            }
            else if(cell!=null)
            {
                Body.Health += cell.HealthAfterInteract;
            }
        }

        private Tuple<int ,int> GetPosition(int t)
        {
            if (t < 0 || t > 7)
                throw new ArgumentOutOfRangeException();
            switch (t)
            {
                case 0:
                    return new Tuple<int, int>(Body.X - 1, Body.Y - 1);
                case 1:
                    return new Tuple<int, int>(Body.X - 1, Body.Y );

                case 2:
                    return new Tuple<int, int>(Body.X - 1, Body.Y + 1);

                case 3:
                    return new Tuple<int, int>(Body.X , Body.Y - 1);

                case 4:
                    return new Tuple<int, int>(Body.X , Body.Y + 1);

                case 5:
                    return new Tuple<int, int>(Body.X + 1, Body.Y - 1);

                case 6:
                    return new Tuple<int, int>(Body.X + 1, Body.Y );

                case 7:
                    return new Tuple<int, int>(Body.X + 1, Body.Y + 1);

                default:
                    throw new ArgumentOutOfRangeException();

            }

        }

        
        #endregion
        
        private void Dead(object sender,EventArgs e)
        {
            WorldMap[Body.X, Body.Y] = null;
            Body = null;
            Brain = null;
           
        }
    }
}
