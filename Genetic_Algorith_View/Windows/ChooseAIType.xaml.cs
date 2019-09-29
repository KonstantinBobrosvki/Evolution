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
            StandartImages.Columns = 4;
            StandartImages.Rows = 8;


            var iteration = 0;
            //Loop for adding images in standart AI
            for (int i = 1; i < 8; i++)
            {
                var temp = new Uri("pack://application:,,,/Resources/StandartAI/Rotate"+i+".png");
                
                var image = new Image() { Source = new BitmapImage(temp) };
                StandartImages.Children.Insert(iteration++,image);
            }
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




            StandartImages.Columns = 4;
            StandartImages.Rows = 8;
        }


    }
}
