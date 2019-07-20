using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Controller;
namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Properties

        public static WorldController WorldController;
   

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
     
            MainScreen = new MainWindow();
            app.Run(MainScreen);
        }
    }
}
