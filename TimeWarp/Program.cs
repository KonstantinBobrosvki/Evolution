using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Modal;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Console;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace TimeWarp
{
    public class Program
    {
       
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static Worksheet workSheet;

        
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


            var worldController =new WorldController(new MapController(width, height, seed));

            WriteLine("Enter epoch for living");
            long epochs =long.Parse(ReadLine());



            string pathforexcel = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            // Создаём экземпляр рабочий книги Excel
             Microsoft.Office.Interop.Excel.Workbook workBook;
            // Создаём экземпляр листа Excel
           

            workBook = excelApp.Workbooks.Add();
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            worldController.RestartEvent += ExcelFunc;
            long i = 0;
            worldController.RestartEvent += (t,t1) => i++;
            new System.Threading.Thread(() =>
            {
                for (; i <= epochs;)
                {
                    worldController.WorldLive(null);
                    Console.CursorLeft = 0;
                    Console.Write(i);




                }
            }).Start();
            Console.WriteLine("Enter some-text for stop");

            Console.ReadKey();
           
            WriteLine();
            WriteLine("Ready");

            string path = AppDomain.CurrentDomain.BaseDirectory + @"Saves\\" + DateTime.Now.ToString().Replace(':', '-');

            WriteLine();
            Directory.CreateDirectory(path);
            WorldController.Save(path, worldController);
            
            WriteLine(" The save is in folder ''" + path + "'' .You can rename it if you want ");

          
                workSheet.Cells[1, 1] = "Survived time:";
                workSheet.Cells[1, 3] = "Max survived time " + worldController.MaxTurns;
                workSheet.Cells[1, 5] = "Avarange survived time " + worldController.AvarangeTurns;
                workSheet.Columns.AutoFit();
                excelApp.Visible = true;
                excelApp.UserControl = true;
                Console.ReadKey();
            
          
        }

        static void ExcelFunc(object sender,NewGenerationEventArgs e)
        {
            var worldcontroller = (WorldController)sender;

            if(workSheet!= null)
            workSheet.Cells[worldcontroller.GenerationsCount+1, 1] = e.CurrentLiveTime;
            
        }

       

    }
}
