using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Controller
{
    [Serializable]
    public  partial class WorldController
    {
       
        public delegate void Drawer(int x, int y);

        public event Action<object, NewGenerationEventArgs> RestartEvent;

        public NewGenerationEventArgs LastRestart { get; private set; }

        /// <summary>
        ///  One turn existing
        /// </summary>
        /// <param name="drawer">Delegate for drawing.Set null=no drawing</param>
        public void WorldLive(Drawer drawer)
        {
            CurrentTurns++;
            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];

                var Interacted = item.Think();

                if (item.Health == 0)
                {
                    CreatureController.Map[item.X, item.Y] = null;


                    Creatures.RemoveAt(i);
                    i--;



                    if (Creatures.Count == 8)
                    {
                        Restart();
                        if (drawer != null)
                        {
                            for (int x = 1; x < CurrentMap.Width - 1; x++)
                            {
                                for (int y = 1; y < CurrentMap.Height - 1; y++)
                                {
                                    drawer(x, y);
                                }
                            }
                        }
                        return;
                    }

                }

                Interacted.AddRange(CheckMinimum());

                if (drawer != null)
                {
                    foreach (var (X, Y) in Interacted)
                    {
                        drawer(X, Y);
                    }
                }


            }
            

        }


        public WorldController(MapController map,List<CreatureController> creatures)
        {
            if (map == null)
                throw new ArgumentNullException();

            if (map.EmpetyCells < 80)
                throw new ArgumentException("Too small map");

            this.StartMap = map.Clone();
            CurrentMap = StartMap.Clone();

            MinFood = CurrentMap.EmpetyCells / 30;
            MinPoison = CurrentMap.EmpetyCells / 60;

            Creatures = creatures ?? throw new ArgumentNullException();
        }
        public WorldController(MapController map)
        {
            if (map == null)
                throw new ArgumentNullException();

            if (map.EmpetyCells < 80)
                throw new ArgumentException("Too small map");

            this.StartMap = map.Clone();
            CurrentMap = StartMap.Clone();
            Creatures = new List<CreatureController>(64);

            for (int i = 0; i < 64; i++)
            {
                var temp = map.FreePosition();
                Creatures.Add(new CreatureController(temp.Item1, temp.Item2));
            }

            MinFood = CurrentMap.EmpetyCells / 30;
            MinPoison = CurrentMap.EmpetyCells / 60;
        }

        private WorldController(MapController startmap,int notuse)
        {
            StartMap = startmap;
        }

      
        /// <summary>
        /// Saves data to DIRECTORY
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="worldController">Worldcontroller</param>
        public static void Save(string path,WorldController worldController)
        {
            
            //For info data
            using (StreamWriter writer = new StreamWriter(path+"\\worldcontroller.evol"))
            {
                writer.WriteLine(worldController.MaxTurns);
                writer.WriteLine(worldController.CurrentTurns);
                writer.WriteLine(worldController.GenerationsCount);
                writer.WriteLine(worldController.MinFood);
                writer.WriteLine(worldController.MinPoison);
                writer.WriteLine(worldController.AllTurns);


            }

            //For startmap
            MapController.Save(path + "\\startmap.evol", worldController.StartMap);

            //For currentmap
            MapController.Save(path + "\\currentmap.evol", worldController.CurrentMap);

            //for Creatures
            for (int i = 0; i < worldController.Creatures.Count; i++)
            {
                var item = worldController.Creatures[i];
                CreatureController.Save(path + "\\creature" + i + ".evol",item);
            }

          


        }
        /// <summary>
        /// Load from file
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <returns>result</returns>
       public static WorldController Load(string path)
       {
            //For startmap
            var result = new WorldController(MapController.Load(path + "\\startmap.evol"),0);

            //For info data
            using (var reader = new StreamReader(path + "\\worldcontroller.evol"))
            {
             result.maxturns=int.Parse(reader.ReadLine());
            result.currentTurns= int.Parse( reader.ReadLine());
           result.generationsCount=  int.Parse( reader.ReadLine());
           result.minfood=  int.Parse( reader.ReadLine());
           result.minpoison=  int.Parse( reader.ReadLine());
           result.allturns=  int.Parse( reader.ReadLine());


            }

            
        

            //For currentmap
            result.currentmap= MapController.Load(path + "\\currentmap.evol");
            CreatureController.Map = result.CurrentMap;

            //for Creatures

            var creatures_paths = Directory.GetFiles(path, "*creature*").Where((str) => str.Contains("creature")).ToList();
           
            result.Creatures = new List<CreatureController>(64);
            for (int i = 0; i < creatures_paths.Count; i++)
            {

               
                var temp = CreatureController.Load(path + "\\creature" + i + ".evol");
                result.Creatures.Add(temp);
            }

        


            return result;
       }

    }
}
