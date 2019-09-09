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
using Microsoft.Office.Interop.Excel;

namespace TimeWarp
{
    class Program
    {
       
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static Worksheet workSheet;

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


            var WorldController =new WorldController(new MapController(width, height, seed));

            WriteLine("Enter epoch for living");
            long epochs =long.Parse(ReadLine());



            string pathforexcel = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

           Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            // Создаём экземпляр рабочий книги Excel
            Microsoft.Office.Interop.Excel.Workbook workBook;
            // Создаём экземпляр листа Excel
           

            workBook = excelApp.Workbooks.Add();
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            WorldController.RestartEvent += ExcelFunc;
           
            for (long i = 0; i <= epochs; i++)
            {
                WorldController.WorldLive(null);
                Console.CursorLeft = 0;
                Console.Write(i);

               


            }
            WriteLine();
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
                workSheet.Cells[1, 1] = "Survived time:";
                workSheet.Cells[1, 3] = "Max survived time " + WorldController.MaxTurns;
                workSheet.Cells[1, 5] = "Avarange survived time " + WorldController.AvarangeTurns;
              

             
             
                
                excelApp.Visible = true;
                excelApp.UserControl = true;
                Console.ReadKey();
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

            Directory.CreateDirectory(path);




            using (FileStream stream = new FileStream(path + @"\WorldController.dat", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
            {
                binaryFormatter.Serialize(stream, worldController);
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

            System.Windows.Application app = new System.Windows.Application();
            app.Run(App.WorldScreen);

            app.Exit+=(s,e) => System.Windows.Application.Current.Shutdown();
        }

        static void ExcelFunc(object sender,NewGenerationEventArgs e)
        {
            var worldcontroller = (WorldController)sender;

            workSheet.Cells[worldcontroller.GenerationsCount+1, 1] = e.CurrentLiveTime;
            
        }

    }
}
