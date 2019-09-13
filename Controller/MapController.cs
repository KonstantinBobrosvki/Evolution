using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

            foreach (var item in Map)
            {
                if (item is Food)
                    FoodOnMap++;
                else if (item is Poison)
                    PoisonOnMap++;
                else if (item is null)
                    EmpetyCells++;
            }


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

        public static void Save(string path,MapController controller)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                //First we save size
                writer.WriteLine(controller.Width+"+"+controller.Height);
                //Second is seed
                writer.WriteLine(controller.Seed);

                //Then we save all map with their int analogs
                //0-empety 1-Creature Body 2-Food 3-Poison 4-Wall
                //Every info finishes with ;

                //Builder is for optimaizing and easy
                StringBuilder builder = new StringBuilder();

                for (int y = 0; y < controller.Height; y++)
                {
                    for (int x = 0; x < controller.Width; x++)
                    {
                        var item = controller[x, y];
                        if (item is null)
                        {
                            builder.Append(0 + "+"+x+"+"+y+";");
                        }
                        else if (item is CreatureBody body)
                        {
                            builder.Append(1 + "+" + body.Health + "+" + (int)body.Sight + "+" + x + "+" + y + ";");
                        }
                        else if (item is Food)
                        {
                            builder.Append(2+ "+" + x + "+" + y +";");
                        }
                        else if (item is Poison)
                        {
                            builder.Append(3 + "+" + x + "+" + y+ ";");
                        }
                        else if (item is Wall)
                        {
                            builder.Append(4 + "+" + x + "+" + y + ";");
                        }

                    }
                }     
                writer.WriteLine(builder.ToString());
                writer.Flush();
            }
        }
        public static MapController Load(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                var sizes = reader.ReadLine();
                var width = int.Parse(sizes.Split('+')[0]);
                var height = int.Parse(sizes.Split('+')[1]);
                var seed = int.Parse(reader.ReadLine());
                var map_string = reader.ReadLine().Split(';');
                var map = new WorldObject[width, height];

                foreach (var item in map_string)
                {
                    if (item == "")
                        break;
                    var type_int = item[0];
                    var parametrs = item.Split('+');

                    if (type_int == '1')
                    {
                       
                     map[int.Parse(parametrs[3]),int.Parse(parametrs[4])] = new CreatureBody(int.Parse(parametrs[3]), int.Parse(parametrs[4]))
                            {
                        Health = int.Parse(parametrs[1]),
                                Sight = (CreatureBody.SeeDirection)int.Parse(parametrs[2])
                    };
                        continue;
                    }

                    var x = int.Parse(parametrs[1]);
                    var y = int.Parse(parametrs[2]);

                    if (type_int == '0')
                    {
                        map[x, y] = null;
                    }
                    else if (type_int == '2')
                    {
                        map[x, y] = new Food(x, y);
                    }
                    else if (type_int == '3')
                    {
                        map[x, y] = new Poison(x, y);
                    }
                    else if (type_int == '4')
                    {
                        map[x, y] = new Wall(x, y);
                    }
                    else
                        throw new System.IO.FileLoadException();
                }

               
                return new MapController(map, seed);
               
            }
        }
    }
}
