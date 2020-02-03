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
      


        App()
        {
            InitializeComponent();
        }
        
        [STAThread]
        static void Main(string[] args=null)
        {

          
          App app = new App();
     
           var StartScreen = new MainWindow();
            app.Run(StartScreen);
        }
    }
}
