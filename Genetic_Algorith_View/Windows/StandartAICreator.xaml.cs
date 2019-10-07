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
                var image = new Grid();
                
             
                LogicBlocksGrid.Children.Add(image);
                image.MouseDown += ImageClick;
                image.Tag = i;
            }
            RandomizeBlocks();


            LogicBlocksGrid.Columns = 8;
            LogicBlocksGrid.Rows = 8;
        }

        private void ImageClick(object sender,MouseEventArgs e)
        {

        }

        private void RandomizeBlocks()
        {

            /*
        * 0-7 Rotate
        * 8-15 See
        * 16-23 Move
        * 23-31 Catch
        */

            Random rnd = new Random();
            
            for (int i = 0; i < 64; i++)
            {
                Brush brush = new ImageBrush();
              
                var code = rnd.Next(0, 64);
                var element = LogicBlocksGrid.Children[i] as Grid;
                element.Children.Clear();
                if (code<8)
                {              
                  brush=new ImageBrush( new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Rotate" + code + ".png")));      
                }
                else if(code<16)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/See" + (code-8) + ".png")));

                }
                else if (code < 24)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Move" + (code - 16) + ".png")));

                }
                else if (code < 32)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Catch" + (code - 24) + ".png")));
                }
                else
                {
                    brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    element.Children.Add(new Label() {
                        Content = code,
                        HorizontalContentAlignment =HorizontalAlignment.Center,
                        VerticalContentAlignment =VerticalAlignment.Center,
                        FontSize=FontSize*1.5
                    });
                }




                element.Background = brush;
            }
        }
    }
}
