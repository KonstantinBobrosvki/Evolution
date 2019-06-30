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
        #region Properties

        public static Controller.MapController Map;

        public static int Height;

        public static int Width;

        public static int MinFood=-1;

        public static int MinPoison=-1;
        //The map will be same after restart or not
        public static bool ChangeMap = false;

       
        public static MainWindow MainScreen;

        public static Windows.World WorldScreen;

        public static readonly string PathToFolder = AppDomain.CurrentDomain.BaseDirectory;

        #endregion


        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main(string[] args=null)
        {

          
            App app = new App();
            MainWindow window = new MainWindow();
               
            app.Run(window);
        }
    }
}
