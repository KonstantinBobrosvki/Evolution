using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using Controller;
using Modal;
using System.Windows.Media;

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для World.xaml
    /// </summary>
    public partial class World : Window
    {
        public readonly MapController Map;
        public World()
        {
            
            InitializeComponent();

            //Close all app when closes this
            this.Closed += (sender, e) => App.MainScreen?.Close();

            Map = App.Map;

            StartDraw();

        }
        private void StartDraw()
        {
            MapField.Columns = Map.Width;
            MapField.Rows = Map.Height;
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    WorldObject element=null;
                    try
                    {
                       element = Map[x, y];
                    }
                    catch (Exception)
                    {

                        MessageBox.Show(x + " " + y);
                    }
                    Grid rect = new Grid();
                   

                    if(element is Wall)
                    {
                        rect.Background =new SolidColorBrush( Color.FromRgb(128, 128, 128));
                    }
                    else if(element is CreatureBody)
                    {
                        rect.Background = new SolidColorBrush(Color.FromRgb(0, 0,255));
                        rect.Children.Add(new Label() { Content = ((CreatureBody)element).Health });
                    }
                    else if(element is Food)
                    {
                        rect.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else if (element is Poison)
                    {
                        rect.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    else if(element is null)
                    {
                        rect.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        //This was all types I dnk what to do
                        throw new Exception();
                    }

                    MapField.Children.Add(rect);
                }
            }
        }

        private int GetOneRankIndex(int x,int y)
        {
            return MapField.Rows * y + x;
        }
    }
}
