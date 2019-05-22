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

        public int EmpetyCells { get; private set; }

        public List<CreatureController> Population { get; private set; }
        
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

            Population = new List<CreatureController>(65);
            Map = new WorldObject[width, height];


            EmpetyCells = Width * Height;

            var rnd = new Random(Seed);

            GenerateBorder();
            GenerateWalls(rnd.Next(0,EmpetyCells/20));
            GenerateFood(rnd.Next(0,EmpetyCells/10));
            GeneratePoison(rnd.Next(0,EmpetyCells/20));
            GenerateCreatures(64);
        }

        public void Reset(BrainsTypes.CreatureBrain[] brains)
        {
            if (brains == null || brains.Length != 64)
                throw new ArgumentException();

            var rnd = new Random(Seed);

            EmpetyCells = Height * Width;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Map[x, y] = null;
                }
            }

            GenerateBorder();
            GenerateWalls(rnd.Next(0, EmpetyCells / 20));
            GenerateFood(rnd.Next(0, EmpetyCells / 10));
            GeneratePoison(rnd.Next(0, EmpetyCells / 20));
            GenerateCreatures(64,brains);
        }

        public WorldObject this[int x, int y]
        {

            get
            {
                if (x < 0 || y < 0)
                    throw new IndexOutOfRangeException();

             
                return Map[x, y];
            }

            internal set
            {
                
                Map[x, y] = value;
            }
        }

        

    }
}
