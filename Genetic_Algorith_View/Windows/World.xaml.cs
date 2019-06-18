using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using Controller;
using Modal;
using System.Windows.Media;
using System.Windows.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для World.xaml
    /// </summary>
    public partial class World : Window
    {
        #region
        List<CreatureController> Creatures;

        public MapController Map { get => CreatureController.Map; }

        

        private long MaxTurns = 0;

        private long CurrentTurns = 0;

        private long GenerationsCount = 0;

        private long AllTurns = 0;

        DispatcherTimer Timer = new DispatcherTimer();

        DateTime start = DateTime.Now;

          #endregion

        public World()
        {


            InitializeComponent();

            //Close all app when closes this
            this.Closed += (sender, e) => App.MainScreen?.Close();

            Timer.Tick += Timer_Tick;

            if (App.Map == null)
            {
                App.Map = new MapController(App.Width, App.Height, null);
                CreatureController.Map = App.Map.Clone();

            }
            else
            {
                CreatureController.Map = App.Map.Clone();
            }
            //If Min is non defined
                if (App.MinFood == -1)
                    App.MinFood = Map.Square / 10;
                if (App.MinPoison==-1)
                    App.MinPoison = Map.Square / 30;

            

           

            Creatures = new List<CreatureController>(64);

            for (int i = 0; i < App.CreaturesCount; i++)
            {
                var position = Map.FreePosition();
                var c = new CreatureController(position.Item1, position.Item2);

                Creatures.Add(c);
            }


            StartDraw();

            LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;

            PosionOnMapLabel.Content = Map.PoisonOnMap;

            FoodOnMapLabel.Content= Map.FoodOnMap;

            for (int i = 0; i < 8; i++)
            {
                
                    Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 3, 3, 0.5), ToolTip = "Health of creature in moment of creating childs", FontSize = 20 });


            }
            for (int i = 0; i < 8; i++)
            {
                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 0.5, 3, 3), ToolTip = "Generations without evolution of creature", FontSize = 20 });

            }
            CheckMinimum();
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

        private static int GetOneRankIndex(int x, int y)
        {
            return CreatureController.Map.Width * y + x;
        }

        private void CheckMinimum()
        {
            if(Map.FoodOnMap<App.MinFood*2/4)
            {
                while( App.MinFood != Map.FoodOnMap)
                {
                    var temp = Map.FreePosition();
                    if (temp is null)
                        break;
                    else
                    {
                        Map[temp.Item1, temp.Item2] = new Food();
                        ReDraw(temp.Item1, temp.Item2);
                    }
                }
                
            }
        }

        private void Restart()
        {

            AllTurns += CurrentTurns;

            CurrentTurns = 0;
            GenerationsCount++;


            var newpopulation = new List<CreatureController>(App.CreaturesCount);
            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];
                newpopulation.AddRange(item.GetChildrens(8, 2));
                ((Label)Best8.Children[i]).Content = item.Health;
                ((Label)Best8.Children[i+8]).Content = item.GenerationsWithoutEvolution;

            }


            if (!App.ChangeMap)
            CreatureController.Map = App.Map.Clone();
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

            for (int y = 1; y < Map.Height-1; y++)
            {
                for (int x = 1; x < Map.Width-1; x++)
                {
                    ReDraw(x, y);
                }
            }

            LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;

            FoodOnMapLabel.Content = "Food on map: " + Map.FoodOnMap;
            PosionOnMapLabel.Content = "Poison on map: " + Map.PoisonOnMap;

            CheckMinimum();
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
                if (Creatures.Count == App.MinimumForNewGeneration)
                {
                    Restart();
                    break;
                }
            }
            CheckMinimum();


            FoodOnMapLabel.Content =  Map.FoodOnMap;
            PosionOnMapLabel.Content = Map.PoisonOnMap;

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


        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var t = Map.FreePosition();
                Map[t.Item1, t.Item2] = new Food();
                ReDraw(t.Item1, t.Item2);
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Not enough space");
            }
            FoodOnMapLabel.Content = Map.FoodOnMap;
        }

        private void PosionFoodButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var t = Map.FreePosition();
                Map[t.Item1, t.Item2] = new Poison();
                ReDraw(t.Item1, t.Item2);
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Not enough space");
            }
           PosionOnMapLabel.Content= Map.PoisonOnMap;
        }

        private void SaveAndExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you want to save world","",MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                if (!Directory.Exists(App.PathToFolder + @"\Saves"))
                {
                    Directory.CreateDirectory(App.PathToFolder + @"\Saves");
                }
                var temp = DateTime.Now.ToString();
                string path = App.PathToFolder + @"\Saves\"+temp.Replace(':','-');
               
                Directory.CreateDirectory(path);

               

               
                using (FileStream stream = new FileStream(path+@"/Creatures", FileMode.CreateNew))
                {
                    binaryFormatter.Serialize(stream, Creatures);
                }
                using (FileStream stream = new FileStream(path + @"/Map", FileMode.CreateNew))
                {
                    binaryFormatter.Serialize(stream, Map);
                }

                MessageBox.Show("The save is in folder ''"+temp+"'' ");
                Close();
            }
        }
    }

        #endregion

   
}
