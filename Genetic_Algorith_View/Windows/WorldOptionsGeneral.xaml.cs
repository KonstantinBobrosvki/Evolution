using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Genetic_Algorith_View.Windows;
namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для WorldOptions.xaml
    /// </summary>
    public partial class WorldOptions : Window
    {
        

        public WorldOptions()
        {
            InitializeComponent();
        }

        private void NewWorld_Click(object sender, RoutedEventArgs e)
        {
           
            var z = new World() ;
            z.Show();
        }
    }
}
