using System;
using System.Collections.Generic;
using Modal;

namespace Controller
{
    public partial class CreatureController
    {
        public static MapController Map { get; set; }

        public CreatureBody Body { get; set; }

        public int X { get => Body.X; }
        public int Y { get => Body.Y; }

        public CreatureController(int x,int y)
        {
            Map[x, y] = new CreatureBody(x,y);
            Body =(CreatureBody) Map[x, y];
           
        }

        /// <summary>
        /// Method for interacting with world
        /// </summary>
        /// <returns>Interacted cells</returns>
        public virtual List<(int X,int Y)> Think()
        {
            
            return ThinkPrivate();
        }

        /// <summary>
        /// Move on other cell
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void Move(int x,int y)
        {
            Body.Health--;

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

        public void Catch(int x,int y)
        {
            Body.Health--;
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
            if (times > 8 || times < 1)
                throw new ArgumentOutOfRangeException();

            int temp =(int) Body.Sight;
            while (temp + times >= 8)
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
            if (rotate < 1 || rotate > 8)
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
    }
}
