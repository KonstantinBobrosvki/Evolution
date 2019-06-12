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
        private readonly Random Seed;

        /// <summary>
        /// Count of empety cells
        /// </summary>
        public int EmpetyCells { get; private set; }
        
        private readonly WorldObject[,] Map;


        #region Constructors
        /// <summary>
        /// Constructor of world map
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="seed">Seed for randomazing </param>
        public MapController(int width, int height,int? seed)
        {
            if (width < 3 || height < 3)
                throw new ArgumentOutOfRangeException();

            if(seed==null)
            {
                Random random = new Random();
                this.Seed =new Random();
            }
            else
            {
                this.Seed = new Random((int)seed);
            }

            
            Map = new WorldObject[width, height];


            EmpetyCells = Width * Height ;


           
           

            GenerateBorder();
            GenerateFood(Seed.Next(0, EmpetyCells / 10));
            GeneratePoison(Seed.Next(0, EmpetyCells / 20));
            GenerateWalls(Seed.Next(0,EmpetyCells/20));
            
           

          
        }

        public MapController(int width,int height,int? seed,int FoodCount,int PoisonCount,int WallCount)
        {
            if (width < 3 || height < 3)
                throw new ArgumentOutOfRangeException();

            if (seed == null)
            {
                this.Seed = new Random();
            }
            else
            {
                this.Seed =new Random( (int)seed);
            }


            Map = new WorldObject[width, height];


            EmpetyCells = Width * Height;

            

            GenerateBorder();
            GenerateWalls(WallCount);
            GenerateFood(FoodCount);
            GeneratePoison(PoisonCount);

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map">Map</param>
        /// <param name="rnd">Random for randomazing</param>
        public MapController(WorldObject[,]map,Random rnd)
        {
            if (map.GetLength(0) < 3 || map.GetLength(1) < 3)
                throw new ArgumentOutOfRangeException();

            if (map is null)
                throw new ArgumentNullException();  
            
            Map=(WorldObject[,])map.Clone();
            Seed =rnd;
        }
        #endregion

        public MapController Clone()
        {
            return new MapController(Map, Seed); ;
        }

        public WorldObject this[int x, int y]
        {

            get
            {
                if (x < 0 || y < 0)
                    throw new IndexOutOfRangeException();
                if(x>=Width||y>=Height)
                    throw new IndexOutOfRangeException();

                return Map[x, y];
            }

            set
            {
                //Если до етого пустая
                if(value is null)
                {
                    if(!(this[x,y] is null))
                    EmpetyCells++;
                }
                else
                {
                    if (Map[x, y] is null)
                    {
                        EmpetyCells--;
                    }
                    //если просто меняем содержимое
                    else
                    {
                        if (Map[x, y] is Food)
                        {
                            if(value.GetType()!=typeof(Food))
                            FoodOnMap--;
                        }
                        else if (Map[x, y] is Poison)
                        {
                            if (value.GetType() != typeof(Poison))
                                PoisonOnMap--;
                        }
                    }
                    value.X = x;
                    value.Y = y;

                }
                Map[x, y] = value;
            }

        }

        /// <summary>
        /// Get random empety place
        /// </summary>
        /// <returns>Position of empety cell</returns>
        public Tuple<int,int> FreePosition()
        {
            if (EmpetyCells == 0)
                throw new Exception();

            Random rnd = new Random();
            while(true)
            {
                var x = rnd.Next(0, Width);
                var y = rnd.Next(0, Height);
                var element = this[x, y];
                if (element is null)
                    return new Tuple<int, int>(x, y);
            }
        }

    }
}
