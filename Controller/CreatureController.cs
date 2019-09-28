using System;
using System.Collections.Generic;
using Modal;
using System.IO;
using System.Text;

namespace Controller
{
    [Serializable]
    public  class CreatureController
    {
        #region Some AI staff
        /// <summary>
        /// Main Brain
        /// </summary>
        private int[] LogicBlocks = new int[64];
        /// <summary>
        /// For easier work
        /// </summary>
        private int Current { get => NotUseCurrent; set { if (value < 0) throw new ArgumentOutOfRangeException(); if (value >= 64) value -= 64; NotUseCurrent = value; } }
        /// <summary>
        /// Do not use it anywhere
        /// </summary>
        private int NotUseCurrent = 0;
        #endregion

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

        public bool MustEvolve { get; set; } = true;

        #endregion


        /// <summary>
        /// Creates Random controller with new body
        /// </summary>
        /// <param name="x">X position for new body</param>
        /// <param name="y">Y position for new body</param>
        public CreatureController(int x,int y):this()
        {
            Map[x, y] = new CreatureBody(x,y);
            Body =(CreatureBody) Map[x, y];
            
        }


        /// <summary>
        /// Creates Random controller
        /// </summary>
        public CreatureController()
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
            var result = new List<(int, int)>(2);
            result.Add((X, Y));
            for (int i = 0; i < 10; i++)
            {
                var actioncode = LogicBlocks[Current];

                if (actioncode < 0)
                {
                    throw new Exception("WTF");
                }
                //For rotating
                else if (actioncode < 8)
                {
                    Rotate(actioncode);
                    Current++;
                }
                //For ONLY seeing
                else if (actioncode < 16)
                {
                    var type = See(actioncode - 8);
                    if (type is null)
                        Current++;
                    else if (type is CreatureBody)
                        Current += 2;
                    else if (type is Food)
                        Current += 3;
                    else if (type is Poison)
                        Current += 4;
                    else if (type is Wall)
                        Current += 5;
                    else
                        throw new Exception("WTF");


                }
                //For Move
                else if (actioncode < 24)
                {
                    var type = See(actioncode - 16);
                    if (type is null)
                        Current++;
                    else if (type is CreatureBody)
                        Current += 2;
                    else if (type is Food)
                        Current += 3;
                    else if (type is Poison)
                        Current += 4;
                    else if (type is Wall)
                        Current += 5;

                    var where = IndexOfCell(actioncode - 16);
                    Move(where.Item1, where.Item2);
                    result.Add((where.Item1, where.Item2));
                    break;
                }
                //For Catch
                else if (actioncode < 32)
                {
                    var type = See(actioncode - 24);
                    if (type is null)
                        Current++;
                    else if (type is CreatureBody)
                        Current += 2;
                    else if (type is Food)
                        Current += 3;
                    else if (type is Poison)
                        Current += 4;
                    else if (type is Wall)
                        Current += 5;

                    var where = IndexOfCell(actioncode - 24);
                    Catch(where.Item1, where.Item2);
                    result.Add((where.Item1, where.Item2));
                    break;
                }
                //Only adding to current
                //If u want new functions write them here
                else if (actioncode < 64)
                {
                    Current += actioncode;

                }
                //Exception
                else
                    throw new Exception("In ThinkPrivate bigger than 64");
            }
            Body.Health--;
            return result;
        }

        /// <summary>
        /// Some random change in AI
        /// </summary>
        public virtual void Evolve()
        {
            if (!MustEvolve)
                return;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            GenerationsWithoutEvolution = 0;
            LogicBlocks[rnd.Next(0, 64)] = rnd.Next(0, 64);
        }

        /// <summary>
        /// Returns copies
        /// </summary>
        /// <param name="count">Copies count</param>
        /// <param name="mutatescount">Changed copies</param>
        /// <returns>Copies</returns>
        public virtual List<CreatureController>GetChildrens(int count,int mutatescount)
        {
            if (count < 0 || count < mutatescount)
                throw new ArgumentOutOfRangeException();
            var result = new List<CreatureController>(count);
            for (int i = 0; i < count; i++)
            {
                CreatureController c = new CreatureController();
                c.LogicBlocks = (int[])LogicBlocks.Clone();
                c.GenerationsWithoutEvolution = this.GenerationsWithoutEvolution + 1;
                if (mutatescount > 0)
                {
                    c.Evolve();
                    mutatescount--;
                }
                c.Current = 0;

                result.Add(c);
            }
            return result;
        }

        #endregion

        #region Methods for interacting with world
    

        /// <summary>
        /// Move on other cell
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        protected void Move(int x,int y)
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
        protected void Catch(int x,int y)
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

        protected void Rotate(int times)
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
        protected Tuple<int,int> IndexOfCell(int rotate)
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

        protected WorldObject See(int rotate)
        {
            var temp = IndexOfCell(rotate);
            return Map[temp.Item1, temp.Item2];
        }

        #endregion

        public virtual void Save(string path )
        {
            CreatureController controller = this;

            using (StreamWriter writer = new StreamWriter(path))
            {
                //First save X Y
                writer.WriteLine(controller.X + "+" + controller.Y);

                //Second is current logic block index
                writer.WriteLine(controller.Current);

                //Generations without evolution
                writer.WriteLine(controller.GenerationsWithoutEvolution);

                //Must that creature evolve
                writer.WriteLine(MustEvolve);

                //Brain
                StringBuilder builder = new StringBuilder();

                foreach (var item in controller.LogicBlocks)
                {
                    builder.Append(item + ";");
                }
                writer.WriteLine(builder.ToString());


            }
        }

        public virtual void Load(string path)
        {
            if (Map == null)
                throw new Exception("First load map");
            using (StreamReader reader = new StreamReader(path))
            {
                var xy = reader.ReadLine().Split('+') ;
                var currentindex = int.Parse(reader.ReadLine());
                var generations = int.Parse(reader.ReadLine());
                var evolve = bool.Parse(reader.ReadLine());
                var brain = new int[64];
                var temp = reader.ReadLine().Split(';');
                
                for (int i = 0; i < 64; i++)
                {
                   
                        brain[i] = int.Parse(temp[i].Replace(" ",""));
                    
                }
               
              
                this.body = Map[int.Parse(xy[0]), int.Parse(xy[1])] as CreatureBody;
                this.LogicBlocks = brain;
                this.NotUseCurrent = currentindex;
                this.GenerationsWithoutEvolution = generations;
                
            }
        }

        public override bool Equals(object obj)
        {
            if(obj is CreatureController creature)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (creature.LogicBlocks[i] != LogicBlocks[i])
                       return false;
                      
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
