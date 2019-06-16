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
        List<CreatureController> Creatures;

        public  MapController Map { get => CreatureController.Map; }

        private  MapController StartMap;
        public World()
        {
            

            InitializeComponent();

            

            //Close all app when closes this
            this.Closed += (sender, e) => App.MainScreen?.Close();

           
            CreatureController.Map = App.Map;

            StartMap = Map.Clone();

            Creatures = new List<CreatureController>(64);

            for (int i = 0; i < Creatures.Capacity; i++)
            {
                var position = Map.FreePosition();
                var c = new CreatureController(position.Item1, position.Item2);
                
                Creatures.Add(c);
            }

           
            StartDraw();

        }
        private void StartDraw()
        {
            MapField.Columns = Map.Width;
            MapField.Rows = Map.Height;
            MapField.Children.Clear();
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    WorldObject element= Map[x, y];
                    
                    Grid rect = new Grid();
                   

                    if(element is Wall)
                    {
                        rect.Background =new SolidColorBrush( Color.FromRgb(128, 128, 128));
                    }
                    else if(element is CreatureBody)
                    {
                       
                            rect.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
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
            return CreatureController.Map.Width * y + x;
        }

        private void NextMoveButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];

                var Interacted = item.Think();
                foreach (var position in Interacted)
                {
                    ReDraw(position.X, position.Y);
                }
                if (item.Health == 0)
                {
                    CreatureController.Map[item.X, item.Y] = null;
                    ReDraw(item.X, item.Y);
                    item.Body = null;
                    Creatures.RemoveAt(i);
                    i--;
                }
                if(Creatures.Count==8)
                {
                    Restart();
                    break;
                }
            }
            
        }

        private void Restart()
        {
            var newpopulation = new List<CreatureController>(64);
            foreach (var item in Creatures)
            {
               newpopulation.AddRange( item.GetChildrens(8, 2));
            }
            CreatureController.Map = StartMap.Clone();

            foreach (var item in newpopulation)
            {
              var place=  CreatureController.Map.FreePosition();
                var body= new CreatureBody();
                CreatureController.Map[place.Item1, place.Item2] = body;
                item.Body = body;
            }
            Creatures = newpopulation;
            StartDraw();
        }

        private void ReDraw(int x,int y)
        {
            WorldObject element = Map[x, y];

            Grid rect =(Grid) MapField.Children[GetOneRankIndex(x,y)];
            rect.Children.Clear();

            if (element is Wall)
            {
                rect.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
            else if (element is CreatureBody)
            {
              
                    rect.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    rect.Children.Add(new Label() { Content = ((CreatureBody)element).Health });
                
            }
            else if (element is Food)
            {
                rect.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else if (element is Poison)
            {
                rect.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (element is null)
            {
                rect.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                //This was all types I dnk what to do
                throw new Exception();
            }
        }
    }
}
