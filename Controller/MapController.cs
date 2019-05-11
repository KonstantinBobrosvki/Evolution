using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;

namespace Controller
{
    public partial class MapController
    {
        /// <summary>
        /// Weidth of Map
        /// </summary>
        public int Width { get => Map.GetLength(0); }

        /// <summary>
        /// Height of Map
        /// </summary>
        public int Height { get => Map.GetLength(1); }

        /// <summary>
        /// Seed of Map
        /// </summary>
        public int Seed { get => seed; }
        private readonly int seed;
        
        private readonly WorldObject[,] Map;

        /// <summary>
        /// Constructor of world map
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="seed">Seed for randomazing </param>
        public MapController(int width, int height,int? seed=null)
        {
            if(seed==null)
            {
                Random random = new Random();
                this.seed = random.Next();
            }
            else
            {
                this.seed = (int)seed;
            }
            Map = new WorldObject[width, height];

            GenerateBorder();
            GenerateWalls(5);
            GenerateFood(30);
        }

     

        public WorldObject this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0)
                    throw new IndexOutOfRangeException();
              
                return Map[x, y];
            }
            private  set
            {
                if (value == null)
                    throw new ArgumentNullException();
                Map[x, y] = value;
            }
        }

        

    }
}
