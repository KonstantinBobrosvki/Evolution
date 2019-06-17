using System;
using System.Collections.Generic;
using Modal;

namespace Controller
{
    public partial class CreatureController
    {
        #region properties
        public static MapController Map { get; set; }

        public CreatureBody Body {
            get => body;
            set {
               
                body = value;
            }
        }
        private CreatureBody body;

        public int X { get => Body.X; }
        public int Y { get => Body.Y; }
        public int Health { get => Body.Health; }
        public CreatureBody.SeeDirection Sight { get => Body.Sight; }

        public int GenerationsWithoutEvolution { get; private set; }

        #endregion
        public CreatureController(int x,int y):this()
        {
            Map[x, y] = new CreatureBody(x,y);
            Body =(CreatureBody) Map[x, y];

            //For optimization
            if (this.GetType().ToString() != typeof(CreatureController).ToString())
                LogicBlocks = null;
        }

        private CreatureController()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < LogicBlocks.Length; i++)
            {
                LogicBlocks[i] = random.Next(0, 64);
            }
        }

        #region Methods for evolving

        /// <summary>
        /// Using AI
        /// </summary>
        /// <returns>Interacted cells</returns>
        public virtual List<(int X,int Y)> Think()
        {
            return ThinkPrivate();
        }

        /// <summary>
        /// Some random change in AI
        /// </summary>
        public virtual void Evolve()
        {
            EvolvePrivate();
        }

        /// <summary>
        /// Returns copies
        /// </summary>
        /// <param name="count">Copies count</param>
        /// <param name="mutatescount">Changed copies</param>
        /// <returns>Copies</returns>
        public virtual List<CreatureController>GetChildrens(int count,int mutatescount)
        {
            return GetChilds(count, mutatescount);
        }

        #endregion

        #region Methods for interacting with world
    

        /// <summary>
        /// Move on other cell
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void Move(int x,int y)
        {

           
            if (Map[x,y]==null)
            {
                Map[Body.X, Body.Y] = null;
                Map[x, y] = Body;
                return;
            }


            Body.Health += Map[x, y].HealthAfterInteract;
            if (Map[x, y].CanMoveTrought)
            {
                
                Map[Body.X, Body.Y] = null;
                Map[x, y] = Body;


            }


        }

        /// <summary>
        /// Get something on another cell without moving
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void Catch(int x,int y)
        {
           

            if (Map[x,y] is Poison)
            {
                Map[x, y] = new Food(x, y);
                return;
            }
            if(Map[x,y] is Food)
            {
                Body.Health += Map[x, y].HealthAfterInteract;
                Map[x, y] = null;
            }

           
        }

        public void Rotate(int times)
        {
            if (times > 7 || times < 0)
                throw new ArgumentOutOfRangeException();

            int temp =(int) Body.Sight + times;
            while (temp  >= 8)
                temp -= 8;
            Body.Sight = (CreatureBody.SeeDirection)temp;
        }

        /// <summary>
        /// Method for index of cell by degres for rotate by body sight
        /// </summary>
        /// <param name="rotate">How many times is rotating</param>
        /// <returns>X,Y</returns>
        public Tuple<int,int> IndexOfCell(int rotate)
        {
            if (rotate < 0|| rotate > 7)
                throw new ArgumentOutOfRangeException();

            int temp =(int)Body.Sight + rotate;
            while (temp >= 8)
                temp -= 8;
            switch (temp)
            {
                case 0:
                    return new Tuple<int, int>(X, Y - 1);
                case 1:
                    return new Tuple<int, int>(X+1, Y - 1);
                case 2:
                    return new Tuple<int, int>(X+1, Y);
                case 3:
                    return new Tuple<int, int>(X+1, Y + 1);
                case 4:
                    return new Tuple<int, int>(X, Y + 1);
                case 5:
                    return new Tuple<int, int>(X-1, Y + 1);
                case 6:
                    return new Tuple<int, int>(X-1, Y );
                case 7:
                    return new Tuple<int, int>(X-1, Y - 1);
                default:
                    throw new Exception("Unhandled error");
            }

            
        }

        public WorldObject See(int rotate)
        {
            var temp = IndexOfCell(rotate);
            return Map[temp.Item1, temp.Item2];
        }

        #endregion
    }
}
