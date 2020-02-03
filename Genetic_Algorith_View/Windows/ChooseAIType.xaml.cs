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

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChooseAIType.xaml
    /// </summary>
    public partial class ChooseAIType : Window
    {
        public ChooseAIType()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            
            StandartImages.Rows = 4;
            StandartImages.Columns = 8;


          
            StandartImages.Rows = 4;
            StandartImages.Columns = 8;
        }

        private void Standart_Choose_Click(object sender, RoutedEventArgs e)
        {
            var Creator = new StandartAICreator();
            Creator.Show();
            Creator.Closed += (o,a) => this.Close();
            this.Hide();

        }
    }
}
