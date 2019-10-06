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

        public static MainWindow StartScreen;

        public static List<Window> UsingWindows = new List<Window>();

        /// <summary>
        /// Respendes most important screen for moment
        /// </summary>
        public static Window CurrentMain
        {
            get => currentmain;

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if(currentmain!=null)
                {
                    currentmain.Closed -= Close;
                    currentmain.Closing -= Close;

                }
                value.Closed += Close;

                currentmain = value;
                currentmain.Show();

            }

        }

        private static Window currentmain;

        /// <summary>
        /// Close all program windows
        /// </summary>
        public static void Close(object sender,EventArgs e)
        {
            foreach (var item in UsingWindows)
            {
                try
                {
                    item.Close();

                }
                catch (Exception)
                {

                    
                }
            }
        }
        


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
     
            StartScreen = new MainWindow();
            app.Run(StartScreen);
        }
    }
}
