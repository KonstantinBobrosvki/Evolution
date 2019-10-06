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
    /// Логика взаимодействия для StandartAICreator.xaml
    /// </summary>
    public partial class StandartAICreator : Window
    {
        public StandartAICreator()
        {
            InitializeComponent();

            for (int i = 0; i < 64; i++)
            {
                Image image = new Image();
                LogicBlocksGrid.Children.Add(image);
                image.MouseDown += ImageClick;
                image.Tag = i;
            }



            LogicBlocksGrid.Columns = 8;
            LogicBlocksGrid.Rows = 8;
        }

        private void ImageClick(object sender,MouseEventArgs e)
        {

        }
    }
}
