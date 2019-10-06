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
    /// Логика взаимодействия для ChooseLogicBlock.xaml
    /// </summary>
    public partial class ChooseLogicBlock : Window
    {
        public event Action<object, int> ChoosedItemCode;
        public ChooseLogicBlock()
        {
            InitializeComponent();

            var StandartImages = AllGrid;

            var iteration = 0;
            //Loop for adding images in standart AI

            for (int i = 0; i < 8; i++)
            {
                var temp = new Uri("pack://application:,,,/Resources/StandartAI/Catch" + i + ".png");

                var image = new Image() { Source = new BitmapImage(temp) };
                StandartImages.Children.Insert(iteration++, image);
            }
            for (int i = 0; i < 8; i++)
            {
                var temp = new Uri("pack://application:,,,/Resources/StandartAI/Move" + i + ".png");

                var image = new Image() { Source = new BitmapImage(temp) };
                StandartImages.Children.Insert(iteration++, image);
            }
            for (int i = 0; i < 8; i++)
            {
                var temp = new Uri("pack://application:,,,/Resources/StandartAI/See" + i + ".png");

                var image = new Image() { Source = new BitmapImage(temp) };
                StandartImages.Children.Insert(iteration++, image);
            }
            for (int i = 1; i < 8; i++)
            {
                var temp = new Uri("pack://application:,,,/Resources/StandartAI/Rotate" + i + ".png");

                var image = new Image() { Source = new BitmapImage(temp) };
                StandartImages.Children.Insert(iteration++, image);
            }
            for (;iteration<64;iteration ++)
            {
                Label l = new Label();
                l.Content = iteration;

                StandartImages.Children.Insert(iteration, l);

            }




            StandartImages.Rows = 8;
            StandartImages.Columns = 8;
        }
    }
}
