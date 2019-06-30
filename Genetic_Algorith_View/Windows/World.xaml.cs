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
        List<CreatureController> Creatures = new List<CreatureController>(64);

        public MapController Map { get => CreatureController.Map; }

        

        public long MaxTurns {
            get => maxturns;
            set {
                maxturns = value;
                MaxLive.Content = "Max time of life: " + MaxTurns;
            }
        }
        private long maxturns;

        public long CurrentTurns { 
            get => currentTurns;
            set {
                currentTurns = value;
                CurrentLive.Content = "Current life: " + CurrentTurns;
            }
        }
        private long currentTurns;

        public long GenerationsCount {
            get => generationsCount;
            set
            {
                generationsCount = value;
                Genretaions.Content = "Generations count: " + GenerationsCount;
                AvarangeLiveLabel.Content = "Avarange turns: " + AllTurns / GenerationsCount;
               
            }
        }
        private long generationsCount;

        public long AllTurns = 0;

        DispatcherTimer Timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };

        public DateTime StartTime = DateTime.Now;

        

          #endregion

       

        public World():this(App.Map ?? new MapController(App.Width, App.Height, DateTime.Now.Millisecond))
        {

            if (Map.EmpetyCells < 64)
            {
                App.MainScreen.Show();

                throw new ArgumentException("The world is too small");
            }
            for (int i = 0; i < 64; i++)
            {
                var temp = Map.FreePosition();
               
                Creatures.Add(new CreatureController(temp.Item1, temp.Item2));
                ReDraw(temp.Item1, temp.Item2);
            }

            LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;

            return;
       

        }

        public World(List<CreatureController> creatures,MapController map):this(map)
        {
            if (creatures is null)
                throw new ArgumentNullException();

            Creatures = creatures;

            LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;

        }

        private World(MapController map)
        {

            App.MainScreen.Hide();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            InitializeComponent();

            //Close all app when closes this
            this.Closed += (sender, e) => App.MainScreen?.Close();

            Timer.Tick += Timer_Tick;

            CreatureController.Map = map;

            StartDraw();

            LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;

            PosionOnMapLabel.Content = Map.PoisonOnMap;

            FoodOnMapLabel.Content = Map.FoodOnMap;

            Best8.Columns = 8;

            for (int i = 0; i <8; i++)
            {

                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 3, 3, 0.5), ToolTip = "Health of creature in moment of creating childs", FontSize = 20 });

            }
            for (int i = 0; i < 8; i++)
            {
                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 0.5, 3, 3), ToolTip = "Generations without evolution of creature", FontSize = 20 });

            }

            //If Min is non defined
            if (App.MinFood == -1)
                App.MinFood = Map.Square / 10;
            if (App.MinPoison == -1)
                App.MinPoison = Map.Square / 30;


            CheckMinimum();

         

            this.KeyDown+= (o, e) => { if (e.Key == System.Windows.Input.Key.Escape) Exit(); };
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
            try
            {
                if (Map.FoodOnMap < App.MinFood * 2 / 4)
                {

                    var changed = Map.GenerateFood((App.MinFood - Map.FoodOnMap) * 2);
                    foreach (var item in changed)
                    {
                        ReDraw(item.Item1, item.Item2);
                    }

                }
                if (Map.PoisonOnMap < App.MinPoison * 2 / 4)
                {
                    var changed = Map.GeneratePoison((App.MinPoison - Map.PoisonOnMap) * 2);
                    foreach (var item in changed)
                    {
                        ReDraw(item.Item1, item.Item2);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void Restart()
        {
            if (MaxTurns < CurrentTurns)
            {
                MaxTurns = CurrentTurns;

            }

            AllTurns += CurrentTurns;

            CurrentTurns = 0;
            GenerationsCount++;


            var newpopulation = new List<CreatureController>(64);
            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];
                newpopulation.AddRange(item.GetChildrens(8, 2));
                ((Label)Best8.Children[i]).Content = item.Health;
                ((Label)Best8.Children[i+Best8.Columns]).Content = item.GenerationsWithoutEvolution;

            }


            if (!App.ChangeMap)
            CreatureController.Map = App.Map.Clone();
            else
            CreatureController.Map = new MapController(App.Width, App.Height, DateTime.Now.Millisecond);


            foreach (var item in newpopulation)
            {
                var place = CreatureController.Map.FreePosition();
                if (place is null)
                    break;
                var body = new CreatureBody();
                CreatureController.Map[place.Item1, place.Item2] = body;
                item.Body = body;
            }
            Creatures = newpopulation;

           

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
           

            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];

                var Interacted = item.Think();
                foreach (var (X, Y) in Interacted)
                {
                    ReDraw(X, Y);
                }
                if (item.Health == 0)
                {
                    CreatureController.Map[item.X, item.Y] = null;
                    ReDraw(item.X, item.Y);
                    Creatures.RemoveAt(i);
                    i--;
                    LiveCreaturesCountLabel.Content = "Live creatures count: " + Creatures.Count;
                  

                    if (Creatures.Count ==8)
                    {
                        Restart();
                        return;
                    }

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
           
            ElapsedTimeLabel.Content = "Elapsed real time: " + (DateTime.Now - StartTime).ToString();
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
                Timer.Stop();
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                if (!Directory.Exists(App.PathToFolder + @"\Saves"))
                {
                    Directory.CreateDirectory(App.PathToFolder + @"\Saves");
                }
                var temp = DateTime.Now.ToString();
                string  path = App.PathToFolder + @"\Saves\" + temp.Replace(':','-');
               
                Directory.CreateDirectory(path);

               

               
                using (FileStream stream = new FileStream(path+@"\Creatures.dat", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                {
                    binaryFormatter.Serialize(stream, Creatures);
                }

                using (FileStream stream = new FileStream(path + @"\Map.dat", FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                {
                    binaryFormatter.Serialize(stream, Map);
                }

                using (StreamWriter writer = new StreamWriter(new FileStream(path + @"\App.txt", FileMode.CreateNew,FileAccess.Write, FileShare.ReadWrite)))
                {
                    writer.WriteLine(App.Height);
                    writer.WriteLine(App.Width);
                    writer.WriteLine(App.MinFood);
                    writer.WriteLine(App.MinPoison);
                    writer.WriteLine(App.ChangeMap);
                  
                    writer.Flush();
                }

                using (StreamWriter writer = new StreamWriter(new FileStream(path + @"\WorldDatas.txt", FileMode.CreateNew,FileAccess.Write, FileShare.ReadWrite)))
                {
                    writer.WriteLine(MaxTurns);
                    writer.WriteLine(CurrentTurns);
                    writer.WriteLine(GenerationsCount);
                    writer.WriteLine(AllTurns);
                    writer.WriteLine((DateTime.Now - StartTime));
                   
                    writer.Flush();
                }
                

                MessageBox.Show("The save is in folder ''"+temp+"'' .You can rename it if you want ");


              

           
        }

        private void Exit()
        {
           var answer= MessageBox.Show("Are you sure?", "Exit Window", MessageBoxButton.YesNo);
            if(answer==MessageBoxResult.Yes)
            {
                var answer2 =  MessageBox.Show("Do you want to save?", "Save Window", MessageBoxButton.YesNo);

                if (answer2 == MessageBoxResult.Yes)
                    SaveButton_Click(null, null);
               
                  this.Close();
            }
            else if(answer==MessageBoxResult.No)
            {
                MessageBox.Show("Good choice");
            }
        }
        #endregion

     
    }





}
