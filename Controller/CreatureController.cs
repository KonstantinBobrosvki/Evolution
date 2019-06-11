using System;
using System.Collections.Generic;
using Modal;

namespace Controller
{
    public partial class CreatureController
    {
        public static MapController Map { get; set; }

        public CreatureBody Body { get; set; }
        
        public CreatureController(int x,int y):this()
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
            List<(int, int)> InteractedCells = new List<(int, int)>(2) { (Body.X, Body.Y) };

            InteractedCells.Add(PrivateThink());

            return InteractedCells;
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
    }
}
