using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using Controller;
using Modal;
using System.Windows.Media;
using System.Windows.Threading;
using System.Linq;

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для World.xaml
    /// </summary>
    public partial class World : Window
    {
       
        List<CreatureController> Creatures;

        public MapController Map { get => CreatureController.Map; }

        private MapController StartMap;

        private long MaxTurns = 0;

        private long CurrentTurns = 0;

        private long GenerationsCount = 0;

        private long AllTurns = 0;

        DispatcherTimer Timer = new DispatcherTimer();

        DateTime start = DateTime.Now;

       

        public World()
        {


            InitializeComponent();

            
          


            //Close all app when closes this
            this.Closed += (sender, e) => App.MainScreen?.Close();

            Timer.Tick += Timer_Tick;

            if(App.Map==null)
            {
                CreatureController.Map = new MapController(App.Width, App.Height, null);
            }
            else
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

        #region Methods

        private void StartDraw()
        {
            MapField.Columns = Map.Width;
            MapField.Rows = Map.Height;
            MapField.Children.Clear();
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    WorldObject element = Map[x, y];

                    Grid rect = new Grid();


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

                    MapField.Children.Add(rect);
                }
            }
        }

        private int GetOneRankIndex(int x, int y)
        {
            return CreatureController.Map.Width * y + x;
        }


        private void Restart()
        {

            AllTurns += CurrentTurns;

            CurrentTurns = 0;
            GenerationsCount++;


            var newpopulation = new List<CreatureController>(64);
            foreach (var item in Creatures)
            {
                newpopulation.AddRange(item.GetChildrens(8, 2));
            }

            if(App.Map!=null)
            CreatureController.Map = StartMap.Clone();
            else
            CreatureController.Map = new MapController(App.Width, App.Height, null);


            foreach (var item in newpopulation)
            {
                var place = CreatureController.Map.FreePosition();
                var body = new CreatureBody();
                CreatureController.Map[place.Item1, place.Item2] = body;
                item.Body = body;
            }
            Creatures = newpopulation;

            Genretaions.Content = "Generations count: " + GenerationsCount;
            AvarangeLiveLabel.Content = "Avarange turns: " + AllTurns / GenerationsCount;

            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    ReDraw(x, y);
                }
            }
                   
        }

        private void ReDraw(int x, int y)
        {
            WorldObject element = Map[x, y];

            Grid rect = (Grid)MapField.Children[GetOneRankIndex(x, y)];
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

       

        private void WorldLive()
        {
            CurrentTurns++;
            CurrentLive.Content = "Current life: " + CurrentTurns;
            if(MaxTurns<CurrentTurns)
            {
                MaxTurns = CurrentTurns;
                MaxLive.Content = "Max time of life: " + MaxTurns;

            }

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
                    LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;
                }
                if (Creatures.Count == 8)
                {
                    Restart();
                    break;
                }
            }
        }

        private void Best8Output()
        {
            List<CreatureController> creatureControllers = new List<CreatureController>(10) { Creatures[0] };
            foreach (var item in Creatures)
            {
                foreach (var item1 in creatureControllers)
                {
                    if(item.Health>item1.Health)
                    {
                        creatureControllers.Add(item);
                        if(creatureControllers.Count==9)
                        creatureControllers.Remove(item1);
                        break;
                    }
                }
            }
           

           
            for (int i = 0; i < 8; i++)
            {
                var item = creatureControllers[i];
                Best8.Children[i]=new Label() { Content = item.Health };
                Best8.Children[i+8] = new Label() { Content = item.GenerationsWithoutEvolution };

            }

        }
        #endregion

        #region Events
       
        private void Timer_Tick(object sender, EventArgs e)
        {
            WorldLive();
            ElapsedTimeLabel.Content = "Elapsed real time: " + (DateTime.Now - start).ToString();
        }

        private void NextMoveButton_Click(object sender, RoutedEventArgs e)
        {
            WorldLive();

        }


        private void TimerButton_Click(object sender, RoutedEventArgs e)
        {

            Timer.IsEnabled = !Timer.IsEnabled;
            if (Timer.IsEnabled)
            {
                TimerButton.Content = "Stop";
              
            }
            else
            {
                TimerButton.Content = "Start";

            }

        }

        private void TimerInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(TimerInput.Text, out int c))
            {
                MessageBox.Show("This field contains only numbers");
                if(TimerInput.Text.Length>1)
                TimerInput.Text.Remove(TimerInput.Text.Length - 1);
                return;
            }
            Timer.Interval = new TimeSpan(0, 0, 0, 0, c);
        }

        #endregion
    }
}
