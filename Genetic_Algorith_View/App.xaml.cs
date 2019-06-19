using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Controller.MapController Map;

        public static int Height;

        public static int Width;

        public static int MinFood=-1;

        public static int MinPoison=-1;
        //The map will be same after restart or not
        public static bool ChangeMap = false;

        public static int CreaturesCount = 64;

        //How many creatures will create childs
        public static int MinimumForNewGeneration = 8;

        public static MainWindow MainScreen;

        public static Windows.World WorldScreen;

        public static readonly string PathToFolder = AppDomain.CurrentDomain.BaseDirectory;
    }
}
