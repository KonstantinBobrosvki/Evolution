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
    ///
    [Serializable]
    public partial class World : Window
    {


        #region
        List<CreatureController> Creatures { get => App.WorldController.Creatures; }

        public static MapController Map { get => App.WorldController.CurrentMap; }

        


        DispatcherTimer Timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };

        public DateTime StartTime = DateTime.Now;

        //It can be only one window
        public static bool AlreadyOpened { get; private set; } = false;

        
          #endregion

        /// <summary>
        /// Standart constructor gettin map from app property and creating creatures
        /// </summary>
        public World()
        {
            if (AlreadyOpened)
            {
                throw new Exception("You can open only one window");
            }

            this.Closed += ClosedEvent;

            AlreadyOpened = true;

            App.StartScreen.Hide();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            InitializeComponent();

            //Close all app when closes this
            this.Closed += (sender, e) => App.StartScreen?.Close();

            Timer.Tick += Timer_Tick;

            
           



            StartDraw();

           

            Best8.Columns = 8;

            for (int i = 0; i <8; i++)
            {

                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 3, 3, 0.5), ToolTip = "Health of creature in moment of creating childs", FontSize = 20 });

            }
            for (int i = 0; i < 8; i++)
            {
                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 0.5, 3, 3), ToolTip = "Generations without evolution of creature", FontSize = 20 });

            }

            
            LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;

            PosionOnMapLabel.Content = Map.PoisonOnMap;

            FoodOnMapLabel.Content = Map.FoodOnMap;


            this.KeyDown+= (o, e) => { if (e.Key == System.Windows.Input.Key.Escape) Exit(); };


            App.WorldController.RestartEvent += RestartInfoUpdate;

            if(App.WorldController.LastRestart!=null)
            RestartInfoUpdate(null, App.WorldController.LastRestart);

            
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
                    rect.Tag =GetOneRankIndex(x,y);
                    rect.MouseDown += (s, a) => {
                        var one = (int)rect.Tag;
                        var width = Map.Width;
                        var y1 = one / width;
                        var x1 = one - y1 * width;

                        if (Map[x1, y1] is null)
                            MessageBox.Show("Free");
                        else if (Map[x1, y1] is CreatureBody)
                        {
                            MessageBox.Show(Map[x1, y1].GetType().ToString().Split('.')[1] + " " + ((CreatureBody)Map[x1, y1]).Health);

                        }
                        else
                            MessageBox.Show(Map[x1, y1].GetType().ToString().Split('.')[1]);

                    };


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

        public static int GetOneRankIndex(int x, int y)
        {
            return CreatureController.Map.Width * y + x;
        }

        public void ReDraw(int x, int y)
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

        private void WorldInfoUpdate()
        {
            CurrentLive.Content = "Current turns: "+ App.WorldController.CurrentTurns;
            MaxLive.Content = "Max turns: "+ App.WorldController.MaxTurns;
            LiveCreaturesCountLabel.Content ="Live creatures: "+ App.WorldController.Creatures.Count;
            Genretaions.Content = "Generations Count: " + App.WorldController.GenerationsCount;
            AvarangeLiveLabel.Content = "Avarange turns: "+ App.WorldController.AvarangeTurns;
            FoodOnMapLabel.Content = Map.FoodOnMap;
            PosionOnMapLabel.Content = Map.PoisonOnMap;
        }

        #endregion

        #region Events
       
        private void Timer_Tick(object sender, EventArgs e)
        {


            App.WorldController.WorldLive(ReDraw);
            
            ElapsedTimeLabel.Content = "Elapsed real time: " + (DateTime.Now - StartTime).ToString();
            WorldInfoUpdate();

        }

        private void NextMoveButton_Click(object sender, RoutedEventArgs e)
        {

            App.WorldController.WorldLive(ReDraw);
            WorldInfoUpdate();
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
                Timer.Stop();
                

                if (!Directory.Exists(App.PathToFolder + @"\Saves"))
                {
                    Directory.CreateDirectory(App.PathToFolder + @"\Saves");
                }
                
                string  path = App.PathToFolder + @"\Saves\" + DateTime.Now.ToString().Replace(':','-');
            Directory.CreateDirectory(path);
            var temp = App.WorldController;


            WorldController.Save(path, App.WorldController);

                




             
                MessageBox.Show("The save is in folder ''"+path+"'' .You can rename it if you want ");


              

           
        }

        private void Exit()
        {
            Timer.Stop();
           var answer= MessageBox.Show("Are you sure?", "Exit Window", MessageBoxButton.YesNo);
            if(answer==MessageBoxResult.Yes)
            {
                var answer2 =  MessageBox.Show("Do you want to save?", "Save Window", MessageBoxButton.YesNo);

                if (answer2 == MessageBoxResult.Yes)
                    SaveButton_Click(null, null);

                AlreadyOpened = false;
                Closed -= ClosedEvent;
                  this.Close();
            }
            else if(answer==MessageBoxResult.No)
            {
                MessageBox.Show("Good choice");
            }
        }

        private void ClosedEvent(object sender,EventArgs e)
        {
            Exit();
        }

        private void RestartInfoUpdate(object sender,Controller.NewGenerationEventArgs args)
        {
            for (int i = 0; i < 8; i++)
            {
               ((Label) Best8.Children[i]).Content = args.Parents[i].Health;
               ((Label)Best8.Children[i+8]).Content = args.Parents[i].GenerationsWithoutEvolution;
            }
        }
        #endregion


    }





}
