using System;
using System.Collections.Generic;
using Modal;

namespace Controller
{
    public class CreatureController
    {
        /// <summary>
        /// Body of current creature
        /// </summary>
        public Creature Body { get; private set; }

        /// <summary>
        /// Logic centre 
        /// </summary>
        public CreatureLogic Brain { get; private set; }

        /// <summary>
        /// The map for all living creatures
        /// </summary>
        public static WorldObject[,] WorldMap { get; set; }

        public static void GenerateMap(int width,int height)
        {
            WorldMap = new WorldObject[width, height];
        
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 ||x == width-1 )
                        WorldMap[x, y] =  new Wall();
                    if( y == 0 || y == height - 1)
                        WorldMap[x, y] = new Wall();

                }
            }
        }
        /// <summary>
        /// Standart constructor
        /// </summary>
        public CreatureController()
        {
            Body = new Creature() { Health = 20, SeeDirection = 1 };
            Brain = new CreatureLogic();


            #region Setting random brain
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Brain.Logic.Length; i++)
            {
                Brain.Logic.SetValue(rnd.Next(0, 64), i);
            }
            #endregion
        }

        /// <summary>
        /// Causing action 
        /// </summary>
        public void Think()
        {

        }

        public CreatureController Clone()
        {
            var clone = new CreatureController();
            clone.Body.Health = this.Body.Health;
            clone.Body.SeeDirection =this.Body.SeeDirection;

            for (int i = 0; i < this.Brain.Logic.Length; i++)
            {
                clone.Brain.Logic.SetValue(this.Brain.Logic[i], i);
            }
            clone.Brain.Current = this.Brain.Current;


            return clone;
        }
    }
}
