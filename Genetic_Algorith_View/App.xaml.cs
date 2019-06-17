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
       public static MainWindow MainScreen;
       public static Windows.World WorldScreen;
    }
}
