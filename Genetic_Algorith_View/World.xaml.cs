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
using Controller;

namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для World.xaml
    /// </summary>
    public partial class World : Window
    {
        MapController map;
        public World()
        {
            InitializeComponent();
            WorldController.Start();
            map = WorldController.Map;
           
    
         
            Drawer();
         
        }
        void Drawer()
        {
            Field.Children.Clear();
            Field.Rows=map.Width ;
            Field.Columns =map.Height;
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var item = map[x, y];

                    if (item is Modal.Wall)
                    {
                        Field.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(128, 128, 128)), Width = Height });

                    }
                    else if (item is Modal.Food)
                    {
                        Field.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)), Width = Height });
                    }
                    else if (item is null)
                    {
                        Field.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)), Width = Height });
                    }
                    else if(item is Modal.Poison)
                    {
                        Field.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0)), Width = Height });

                    }
                    else if(item is Modal.CreatureBody c)
                    {
                        var g = new Grid() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 255)) };
                        g.Children.Add(new TextBlock() { Text = c.Health.ToString(),Background= new SolidColorBrush(Color.FromRgb(0, 0, 255)) });
                        Field.Children.Add(g);
                    }
                }
            }
                
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorldController.NextTurn();
            Drawer();
        }
    }
}
