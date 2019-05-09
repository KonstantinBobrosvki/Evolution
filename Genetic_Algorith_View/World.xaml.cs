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
        public World()
        {
            InitializeComponent();
            CreatureController.GenerateMap(20,20);
            Drawer();
        }
        void Drawer()
        {
            Field.Rows = CreatureController.WorldMap.GetLength(0);
            Field.Columns = CreatureController.WorldMap.GetLength(1);
            for (int x = 0; x < CreatureController.WorldMap.GetLength(0); x++)
            {
                for (int y = 0; y < CreatureController.WorldMap.GetLength(1); y++)
                {
                    var item = CreatureController.WorldMap[x, y];

                    if (item is Modal.Wall)
                    {
                        Field.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(128, 128, 128)),Width=Height });
                       
                    }
                    else
                    {
                        Field.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)),Width=Height});

                    }
                }
            }
                
            
        }
    }
}
