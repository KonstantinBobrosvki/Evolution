using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using Controller;
using Modal;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для World.xaml
    /// </summary>
    ///
    public partial class World : Window
    {


        #region
        List<CreatureController> Creatures { get => WorldController.Creatures; }

        public  MapController Map { get => WorldController.CurrentMap; }

        private readonly WorldController WorldController;

        DispatcherTimer Timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };

        public DateTime StartTime = DateTime.Now;

          #endregion

        /// <summary>
        /// Standart constructor gettin map from app property and creating creatures
        /// </summary>
        public World(WorldController worldController)
        {

            WorldController = worldController;
            this.Closed += ClosedEvent;
            this.Closing += (e, a) =>
            {
                try
                {
                    App.Current.Shutdown();
                }
                catch (Exception)
                {

                    
                }
            };
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            InitializeComponent();

            Timer.Tick += Timer_Tick;

            StartDraw();

            Best8.Columns = 8;

            for (int i = 0; i <8; i++)
            {
                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 3, 3, 0.5), ToolTip = "Количество здраве в момента на създаване на потомство", FontSize = 20 });
            }
            for (int i = 0; i < 8; i++)
            {
                Best8.Children.Add(new Label() { BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), BorderThickness = new Thickness(3, 0.5, 3, 3), ToolTip = "Поколения без еволюция", FontSize = 20 });
            }

            
            LiveCreaturesCountLabel.Content = "Количесвто живи същества: " + Creatures.Count;

            PosionOnMapLabel.Content = Map.PoisonOnMap;

            FoodOnMapLabel.Content = Map.FoodOnMap;


            this.KeyDown+= (o, e) => { if (e.Key == System.Windows.Input.Key.Escape) Exit(); };


            WorldController.RestartEvent += RestartInfoUpdate;

            if(WorldController.LastRestart!=null)
            RestartInfoUpdate(null, WorldController.LastRestart);

            
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
                            MessageBox.Show("Празно");
                        else if (Map[x1, y1] is CreatureBody)
                        {

                            Timer.Stop();
                            var window = new Window();
                            System.Windows.Controls.Primitives.UniformGrid grid = new System.Windows.Controls.Primitives.UniformGrid();
                            grid.Rows = 9;
                            grid.Columns = 8;

                            Brush brush = new ImageBrush();
                            var controller = Creatures.Where((c) => c.Body.Equals(Map[x1, y1])).First();
                            var BrainArray = controller.GetBrain();


                            for (int i = 0; i < 64; i++)
                            {
                                var code = BrainArray[i];
                                grid.Children.Add(new Grid());
                                var uiel = grid.Children[i] as Grid;
                                if (code < 8)
                                {
                                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Rotate" + code + ".png")));
                                }
                                else if (code < 16)
                                {
                                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/See" + (code - 8) + ".png")));

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
                                    uiel.Children.Add(new Label()
                                    {
                                        Content = code,
                                        HorizontalContentAlignment = HorizontalAlignment.Center,
                                        VerticalContentAlignment = VerticalAlignment.Center,
                                        FontSize = FontSize * 1.5
                                    });
                                }

                                uiel.Background = brush;

                            }
                            grid.Children.Add(new Label());

                            grid.Children.Add(new Label());
                            grid.Children.Add(new Label() {
                                Content = "Здраве: " + controller.Health,
                                FontSize = FontSize * 1.5,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center
                            });

                            grid.Children.Add(new Label() {

                                Content = "Generations \n without \n evolution: \n " + controller.GenerationsWithoutEvolution,
                                //  FontSize = FontSize * 1.5,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center,

                            });
                            grid.Children.Add(new Label() {
                                Content = "Name: " + controller.Name,
                                FontSize = FontSize * 1.5,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center
                            });
                            grid.Children.Add(new Label());
                            grid.Children.Add(new Label());

                            window.Content = grid;
                            window.ShowDialog();





                        }
                        else if (Map[x1, y1] is Food)
                            MessageBox.Show("Храна");
                        else if (Map[x1, y1] is Poison)
                            MessageBox.Show("Отрова");
                        else if (Map[x1, y1] is Wall)
                            MessageBox.Show("Стена");
                        else if (Map[x1, y1] is CreatureBody)
                            MessageBox.Show("Друго животно");


                        //MessageBox.Show(Map[x1, y1].GetType().ToString().Split('.')[1]);

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

        private static int GetOneRankIndex(int x, int y)
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
            CurrentLive.Content = "Текущо оцеляно време : " + WorldController.CurrentTurns;
            MaxLive.Content = "Максимално количесвто оцеляно време : " + WorldController.MaxTurns;
            LiveCreaturesCountLabel.Content = "Количесвто живи същества: " + WorldController.Creatures.Count;
            Genretaions.Content = "Количество поколения " + WorldController.GenerationsCount;
            AvarangeLiveLabel.Content = "Средно време: " + WorldController.AvarangeTurns;
            FoodOnMapLabel.Content = Map.FoodOnMap;
            PosionOnMapLabel.Content = Map.PoisonOnMap;
        }

        #endregion

        #region Events
       
        private void Timer_Tick(object sender, EventArgs e)
        {


            WorldController.WorldLive(ReDraw);
            
            ElapsedTimeLabel.Content = "Изминало реално време: " + (DateTime.Now - StartTime).ToString();
            WorldInfoUpdate();

        }

        private void NextMoveButton_Click(object sender, RoutedEventArgs e)
        {

            WorldController.WorldLive(ReDraw);
            WorldInfoUpdate();
        }


        private void TimerButton_Click(object sender, RoutedEventArgs e)
        {

            Timer.IsEnabled = !Timer.IsEnabled;
            if (Timer.IsEnabled)
            {
                TimerButton.Content = "Стоп";
              
            }
            else
            {
                TimerButton.Content = "Започни";

            }

        }

        private void TimerInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(TimerInput.Text, out int c))
            {
                MessageBox.Show("Тука се въвеждат само числа");
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

                MessageBox.Show("Няма място");
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

                MessageBox.Show("Няма място");
            }
           PosionOnMapLabel.Content= Map.PoisonOnMap;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
                Timer.Stop();
                

                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Saves"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Saves");
                }
                
                string  path = Directory.GetCurrentDirectory() + @"\Saves\" + DateTime.Now.ToString().Replace(':','-');
            Directory.CreateDirectory(path);
            var temp = WorldController;


            WorldController.Save(path, WorldController);

                




             
                MessageBox.Show("Съхранението е в ''"+path+"'' .Може да го преименуват ,ако искате. ");


              

           
        }

        private void Exit()
        {
            Timer.Stop();
           var answer= MessageBox.Show("Сигурни ли сте?", "Изход", MessageBoxButton.YesNo);
            if(answer==MessageBoxResult.Yes)
            {
                var answer2 =  MessageBox.Show("Искате ли да запазите?", "Запазване", MessageBoxButton.YesNo);

                if (answer2 == MessageBoxResult.Yes)
                    SaveButton_Click(null, null);

              
                Closed -= ClosedEvent;
                  this.Close();
                App.Current.Shutdown();
            }
            else if(answer==MessageBoxResult.No)
            {
                MessageBox.Show("Добър избор :) ");
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
