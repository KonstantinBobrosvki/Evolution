using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modal;

namespace Controller
{

    [Serializable]
    public partial class MapController:IEnumerable
    {
        #region Properties
        /// <summary>
        /// Width of Map
        /// </summary>
        public int Width { get => Map.GetLength(0); }

        /// <summary>
        /// Height of Map
        /// </summary>
        public int Height { get => Map.GetLength(1); }

        /// <summary>
        /// Seed of Map
        /// </summary>
        public int Seed { get; }

        /// <summary>
        /// Count of empety cells
        /// </summary>
        public int EmpetyCells { get; private set; }

        public int Area { get => Width*Height-2*Height-2*Width+4; }

        #endregion
        private readonly WorldObject[,] Map;


        #region Constructors
        /// <summary>
        /// Constructor of world map
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="seed">Seed for randomazing </param>
        public MapController(int width, int height,int seed)
        {
            if (width < 3 || height < 3)
                throw new ArgumentOutOfRangeException();

           
            this.Seed = seed;
            

            
            Map = new WorldObject[width, height];


            EmpetyCells = Width * Height ;




            Random rnd = new Random(Seed);
            GenerateBorder();
            GenerateFood(rnd.Next(0, EmpetyCells / 10));
            GeneratePoison(rnd.Next(0, EmpetyCells / 20));
            GenerateWalls(rnd.Next(0,EmpetyCells/20));
            
           

          
        }

        public MapController(int width,int height,int seed,int FoodCount,int PoisonCount,int WallCount)
        {
            if (width < 3 || height < 3)
                throw new ArgumentOutOfRangeException();

           
            this.Seed =seed;
           


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
        public MapController(WorldObject[,]map,int seed)
        {
            if (map is null)
                throw new ArgumentNullException();

            if (map.GetLength(0) < 3 || map.GetLength(1) < 3)
                throw new ArgumentOutOfRangeException();

            Map=(WorldObject[,])map.Clone();
            Seed =seed;

           
        }
        #endregion

        public MapController Clone()
        {
            var result = new MapController(Map, Seed);
            result.FoodOnMap = FoodOnMap;
            result.EmpetyCells = EmpetyCells;
            result.PoisonOnMap = PoisonOnMap;
            return result ;
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
                if (this[x, y] is Wall && !(value is Wall))
                    throw new Exception("U cant change wall cell");

                //Если делаем пустой
                if(value is null)
                {
                    if(!(this[x,y] is null))
                    {
                        if (this[x, y] is Food)
                            FoodOnMap--;
                        else if (this[x,y] is Poison)
                            PoisonOnMap--;

                        if(!(this[x,y] is null))
                        EmpetyCells++;

                    }
                }
        
                    
                else if(value is Food)
                {
                        if (this[x, y] is null)
                            EmpetyCells--;
                        else if (this[x, y] is Poison)
                            PoisonOnMap--;

                        if(!(this[x,y] is Food))
                        FoodOnMap++;
                }

                 else  if (value is Poison)
                 {
                        if (Map[x, y] is null)
                            EmpetyCells--;
                        else if (this[x, y] is Food)
                            FoodOnMap--;

                        if (!(this[x, y] is Poison))
                            PoisonOnMap++;
                 }

                 else if(value is CreatureBody)
                 {
                    if (Map[x, y] is null)
                        EmpetyCells--;

                    else if (this[x, y] is Food)
                        FoodOnMap--;

                    else if (this[x, y] is Poison)
                        PoisonOnMap--;
                 }

                

                if (!(value is null))
                {
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
                return null;

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

        public IEnumerator GetEnumerator()
        {
           return Map.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (obj is MapController controller)
            {
                if(Width!=controller.Width||Height!=controller.Height)
                {
                    return false;
                }

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var item1 = this[x, y];
                        var item2 = controller[x, y];

                        if (item1 == null)
                        {
                            if (item2 != null)
                                return false;
                            else
                                continue;
                        }
                        if (!item1.Equals(item2))
                            return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Width*Height;
        }
    }
}
