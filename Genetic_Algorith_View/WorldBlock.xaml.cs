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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для WorldBlock.xaml
    /// </summary>
    public partial class WorldBlock : UserControl
    {
        public static DependencyProperty WorldObjectProperty;

        private int X;
        private int Y;

        public Modal.WorldObject ShowedObject
        {
            get
            {
                return (Modal.WorldObject)GetValue(WorldObjectProperty);
            }

            private set
            {
                SetValue(WorldObjectProperty, value);
            }
        }

        static WorldBlock()
        {
            WorldObjectProperty = DependencyProperty.Register("WorldObject", typeof(Modal.WorldObject), typeof(WorldBlock)
                , new FrameworkPropertyMetadata(new PropertyChangedCallback(OnWorldObjectGnaged)));
        }
        public WorldBlock(int x,int y)
        {
            X = x;
            Y = y;

            InitializeComponent();
            Reset();
        }
        static void OnWorldObjectGnaged(DependencyObject sender,DependencyPropertyChangedEventArgs e)
        {
            var item = (Modal.WorldObject)e.NewValue;
          var current=  (WorldBlock)sender;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.Stretch = Stretch.Fill;
            if (item is Modal.Wall)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri("Images/wall.jpg", UriKind.Relative));

                current.Block.Fill = imageBrush;

            }
            else if (item is Modal.Food)
            {
                
                imageBrush.ImageSource = new BitmapImage(new Uri("Images/Food.jpg", UriKind.Relative));
                current.Block.Fill = imageBrush;
            }
            else if (item is null)
            {
              
                current.Block.Fill = new SolidColorBrush(Color.FromRgb(255,255,255));
            }
            else if (item is Modal.Poison)
            {

                imageBrush.ImageSource = new BitmapImage(new Uri("Images/Poison.jpg", UriKind.Relative));

                current.Block.Fill = imageBrush;

            }
            else if (item is Modal.CreatureBody c)
            {

                current.Block.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            }
        }
        public void Reset()
        {
           ShowedObject= Controller.WorldController.Map[X, Y];
        }

    }
}
