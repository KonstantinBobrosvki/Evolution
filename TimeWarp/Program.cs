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


            var WorldController =new WorldController( new MapController(width, height, seed));

            WriteLine("Enter epoch for living");
            int epochs =int.Parse(ReadLine());


            for (int i = 0; i < epochs; i++)
            {
                WorldController.WorldLive(null);
            }

            WriteLine("Ready");
            WriteLine("Do you want open graphic verion?");
            WriteLine("y/n");

            var result = ReadKey().Key;
            if (result == ConsoleKey.Y)
            {
                App.MainScreen = new MainWindow();
                 Save(WorldController);
                Open(WorldController);
            }
            else
            {
               Save(WorldController);
            }
          

        }

     

        static void Save(WorldController worldController)
        {

            
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            if (!Directory.Exists(App.PathToFolder + @"\Saves"))
            {
                Directory.CreateDirectory(App.PathToFolder + @"\Saves");
            }
            var temp = DateTime.Now.ToString();
            string path = App.PathToFolder + @"\Saves\" + temp.Replace(':', '-');

            using (FileStream stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
            {
                binaryFormatter.Serialize(stream,worldController );
            }

            WriteLine();
           WriteLine(" The save is in folder ''" + path + "'' .You can rename it if you want ");
           
           




        }

        static void Open(WorldController worldController)
        {
            App.WorldController = worldController;

            App.WorldScreen = new Genetic_Algorith_View.Windows.World();

            var handle = GetConsoleWindow();

            ShowWindow(handle, 0);

            Application app = new Application();
           app.Run(App.WorldScreen);

            app.Exit+=(s,e)=>Application.Current.Shutdown();
        }

    }
}
