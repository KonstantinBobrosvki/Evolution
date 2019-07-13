using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Modal;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Console;
using Genetic_Algorith_View;
using System.Windows;
using System.Runtime.InteropServices;

namespace TimeWarp
{
    class Program
    {
        public static MapController Map
        {
            get
            {
                return CreatureController.Map;
            }
        }

        static long MaxTurns;

        static long CurrentTurns;

        static int MinFood;

        static int MinPoison;

        static long AllTurns;

        static  int epochs;

        static List<CreatureController> Creatures=new List<CreatureController>(64);

        static DateTime StartTime = DateTime.Now;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        [STAThread]
        static void Main(string[] args)
        {
            WriteLine("Hello this is a hepler app to Evolution.");
            WriteLine("Mission of this application is speed up of original program");
            ForegroundColor = ConsoleColor.Red;
            WriteLine();
            WriteLine("ALERT! ");
            WriteLine("THIS APP DO NOT INPUT CHECKS.");
            WriteLine();
            ForegroundColor = ConsoleColor.White;

            WriteLine("Please enter Width");
            int width =int.Parse(ReadLine());
            WriteLine("Enter Height");
            int height = int.Parse(ReadLine());
            WriteLine("Enter Seed");
            int seed = int.Parse(ReadLine());

            Genetic_Algorith_View.App.Map = new MapController(width, height, seed);

            CreatureController.Map = Genetic_Algorith_View.App.Map.Clone();

            MinFood = (Map.Area - 64) / 10;
            MinPoison = (Map.Area - 64) / 20;

            if (MinFood == 0)
                MinFood = 1;

            if (MinPoison == 0)
                MinPoison = 1;


            for (int i = 0; i < 64; i++)
            {
                var temp = Map.FreePosition();

                Creatures.Add(new CreatureController(temp.Item1, temp.Item2));
               
            }

            WriteLine("Enter epoch for living");
            epochs =int.Parse(ReadLine());


            for (int i = 0; i < epochs; i++)
            {
                WorldLive();
            }

            WriteLine("Ready");
            WriteLine("Do you want open graphic verion?");
            WriteLine("y/n");

            var result = ReadKey().Key;
            if (result == ConsoleKey.Y)
            {
                App.MainScreen = new MainWindow();
                new System.Threading.Thread(Save).Start();
                Open();
            }
            else
            {
               Save();
            }
          

        }

        static void CheckMinimum()
        {

            if (Map.FoodOnMap < MinFood)
            {

              Map.GenerateFood((MinFood - Map.FoodOnMap));
               

            }
            if (Map.PoisonOnMap < MinPoison)
            {
               Map.GeneratePoison((MinPoison - Map.PoisonOnMap));
                

            }

        }

        static void WorldLive()
        {
            CurrentTurns++;


            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];

               item.Think();
               
                if (item.Health == 0)
                {
                    CreatureController.Map[item.X, item.Y] = null;

                  
                    Creatures.RemoveAt(i);
                    i--;
                   


                    if (Creatures.Count == 8)
                    {
                        Restart();
                        return;
                    }

                }

            }
            CheckMinimum();

        }

        static void Restart()
        {
            AllTurns += CurrentTurns;
            if (MaxTurns < CurrentTurns)
            {
                MaxTurns = CurrentTurns;

            }

           
            CurrentTurns = 0;
           
            var newpopulation = new List<CreatureController>(64);
            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];
                newpopulation.AddRange(item.GetChildrens(8, 2));
               

            }



            CreatureController.Map = Genetic_Algorith_View.App.Map.Clone();



            foreach (var item in newpopulation)
            {
                var place = CreatureController.Map.FreePosition();
                
                var body = new CreatureBody();
                CreatureController.Map[place.Item1, place.Item2] = body;
                item.Body = body;
            }
            for (int i = newpopulation.Count; i < 64; i++)
            {

                for (int x = 1; x < Map.Width - 1; x++)
                {
                    for (int y = 1; y < Map.Height - 1; y++)
                    {
                        var cell = Map[x, y];
                        if (cell is Wall || cell is CreatureBody)
                        {

                        }
                        else
                        {
                            Map[x, y] = new CreatureBody();

                            newpopulation[i++].Body = (CreatureBody)Map[x, y];
                        }
                    }
                }
            }
            Creatures = newpopulation;



            


            CheckMinimum();
        }

        static void Save()
        {

            
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            if (!Directory.Exists(App.PathToFolder + @"\Saves"))
            {
                Directory.CreateDirectory(App.PathToFolder + @"\Saves");
            }
            var temp = DateTime.Now.ToString();
            string path = App.PathToFolder + @"\Saves\" + temp.Replace(':', '-');

            Directory.CreateDirectory(path);




            using (FileStream stream = new FileStream(path + @"\Creatures.dat", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
            {
                binaryFormatter.Serialize(stream, Creatures);
            }

            using (FileStream stream = new FileStream(path + @"\CurrentMap.dat", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
            {
                binaryFormatter.Serialize(stream, Map);
            }
            using (FileStream stream = new FileStream(path + @"\StartMap.dat", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
            {
                binaryFormatter.Serialize(stream, App.Map);
            }


            using (StreamWriter writer = new StreamWriter(new FileStream(path + @"\WorldDatas.txt", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite)))
            {
                writer.WriteLine(MaxTurns);
                writer.WriteLine(CurrentTurns);
                writer.WriteLine(epochs);
                writer.WriteLine(AllTurns);
                writer.WriteLine((DateTime.Now - StartTime));
                writer.WriteLine(MinFood);
                writer.WriteLine(MinPoison);

            }

            WriteLine();
           WriteLine(" The save is in folder ''" + path + "'' .You can rename it if you want ");
           
           




        }

        static void Open()
        {
          

            App.WorldScreen = new Genetic_Algorith_View.Windows.World(Creatures,Map);

         
                App.WorldScreen.MaxTurns = MaxTurns;
                App.WorldScreen.CurrentTurns = CurrentTurns;
                App.WorldScreen.AllTurns = AllTurns;
                App.WorldScreen.GenerationsCount = epochs;
                App.WorldScreen.StartTime = DateTime.Now;
                App.WorldScreen.MinFood =MinFood;
                App.WorldScreen.MinPoison = MinPoison;



            var handle = GetConsoleWindow();

            ShowWindow(handle, 0);

            Application app = new Application();
           app.Run(App.WorldScreen);

            app.Exit+=(s,e)=>Application.Current.Shutdown();
        }

    }
}
