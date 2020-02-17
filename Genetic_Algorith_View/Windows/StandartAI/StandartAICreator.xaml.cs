using Controller;
using Modal;
using MyUIElements;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для StandartAICreator.xaml
    /// </summary>
    public partial class StandartAICreator : Window
    {
        //For real time testing
        System.Timers.Timer timer;
        //In ms
        const double SpeedOfTesting = 250;

        private int[] BrainArray = new int[64];
        SpecialWorldController Enviroment;
        const int WidthMap = 30, HeightMap = 30;

        private readonly SynchronizationContext synchronizationContext;

        public StandartAICreator()
        {
            InitializeComponent();

            this.KeyDown += (o, e) => { if (e.Key == System.Windows.Input.Key.Escape) Exit(); };

            for (int i = 0; i < 64; i++)
            {
                var image = new Grid();            
                LogicBlocksGrid.Children.Add(image);
                image.MouseDown += ImageClick;
                image.MouseEnter += (t,e) => { ShowNextBlocks((int)image.Tag); };
                image.MouseLeave += (t, e) =>
                {
                    var remaing = 5;
                    for (int ii = AllGrid.Children.Count - 1; ii >= 0; ii--)
                    {
                        var item = AllGrid.Children[ii];
                        if (item is ArrowLine)
                        {
                            AllGrid.Children.RemoveAt(ii);
                            remaing--;
                            if (remaing == 0)
                                break;
                        }
                    } };
                image.Tag = i;
            }
            RandomizeBlocks();
            Enviroment = new SpecialWorldController(new CreatureController(BrainArray), new MapController(WidthMap, HeightMap, Guid.NewGuid().GetHashCode()));
            
            Enviroment.GetType().GetProperty("MinFood").SetValue(Enviroment, 20, null);
            Enviroment.GetType().GetProperty("MinPoison").SetValue(Enviroment, 20, null);

            
            Enviroment.RestartEvent += (o, e) => FullReDraw();
           
            LogicBlocksGrid.Columns = 8;
            LogicBlocksGrid.Rows = 9;
            

            


            //For real time testing
            timer = new System.Timers.Timer(SpeedOfTesting);
            timer.Elapsed += RealTimeTesting;

            Enviroment.SpecialInfoChanged += (o, a) => new Thread(() => ChangeInfos()).Start(); 

            for (int i = 0; i < WidthMap*HeightMap; i++)
            {
                var rect = new Rectangle();              
                VisualRealTimeTestingGrid.Children.Add(rect);

            }
            VisualRealTimeTestingGrid.Columns = WidthMap;
            VisualRealTimeTestingGrid.Rows = HeightMap;

            FullReDraw();

            

            synchronizationContext = SynchronizationContext.Current;

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            
          
        }

        //public StandartAICreator(CreatureController c) : this()
        //{
        //    var temp = c.GetBrain();
        //    WorldController.ChangeSubject(temp);
        //    for (int i = 0; i < temp.Length; i++)
        //    {
        //        SetCode(i, temp[i]);
        //    }
        //}


        private void RealTimeTesting(object o, object f)
        {

           synchronizationContext.Post(new SendOrPostCallback(o1 =>
            {
                Enviroment.WorldLive(ReDrawMap);
                CurrentLiveTimeLabel.Content = Enviroment.CurrentTurns;
                MaxLiveTimeLabel.Content = Enviroment.MaxTurns;
           }), null);


        } 

        private void ChangeInfos()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate ()
                        {
                           EatsAllFoodLabel.Content= Enviroment.EatsAllAvinableFood;
                            FindInfinityLoopsLabel.Content = Enviroment.HaveLoops;                          
                        });
        }

        private void ReDrawMap(int x,int y)
        {
          
                            var rect = VisualRealTimeTestingGrid.Children[x + y * WidthMap] as Rectangle;

                            WorldObject element = Enviroment.CurrentMap[x, y];



                            if (element is Wall)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            }
                            else if (element is CreatureBody)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                            }
                            else if (element is Food)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            }
                            else if (element is Poison)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            }
                            else if (element is null)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                            }
                            else
                            {
                                //This was all types I dnk what to do
                                throw new Exception();
                            }
                       
        }

        private void ImageClick(object sender,MouseEventArgs e)
        {
            var it = sender as Grid;
           
            var choosingwindow = new ChooseLogicBlock();
            
            choosingwindow.ChoosedItemCode += (s, i) => { SetCode((int)it.Tag, i); };
            choosingwindow.ShowDialog();


            choosingwindow.Close();
        }

        private void RandomizeBlocks()
        {

           

            Random rnd = new Random();
            
            for (int i = 0; i < 64; i++)
            {
                
              
                var code = rnd.Next(0, 64);
                var element = LogicBlocksGrid.Children[i] as Grid;

                //Do not use SetCOde for optimization
                #region SetCodeCopy
                Brush brush = new ImageBrush();


              
                element.Children.Clear();
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
                    element.Children.Add(new Label()
                    {
                        Content = code,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FontSize = FontSize * 1.5
                    });
                }



                BrainArray[i] = code;
                element.Background = brush;
                #endregion

            }
        }

        private void SetCode(int index,int code)
        {
            /*
         * 0-7 Rotate
         * 8-15 See
         * 16-23 Move
         * 23-31 Catch
         */

            Brush brush = new ImageBrush();

           
            var element = LogicBlocksGrid.Children[index] as Grid;
            element.Children.Clear();
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
                element.Children.Add(new Label()
                {
                    Content = code,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontSize = FontSize * 1.5
                });
            }



            BrainArray[index] = code;
            element.Background = brush;
            Enviroment.ChangeSubject(BrainArray);
          

        
           
            
        }

        private void FullReDraw()
        {
            for (int x = 0; x < WidthMap; x++)
            {
                for (int y = 0; y < HeightMap; y++)
                {
                    ReDrawMap(x, y);
                }
            }

        }

        private void CheckBoxRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            if (timer.Enabled)
            {
                rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                CheckBoxStateLabel.Content = "OFF";

            }
            else
            {
                rect.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                CheckBoxStateLabel.Content = "ON";
            }
            timer.Enabled = !timer.Enabled;
        }
  
        private void NameInputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string value = NameInputTextBox.Text;
            if (String.IsNullOrWhiteSpace(value))
                value = "Name is";
            if (value.StartsWith("Name is"))
                value = "";
            NameInputTextBox.Text = value;
            Enviroment.Subject.Name = value;

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Saves"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Saves");
            }

            string path = Directory.GetCurrentDirectory() + @"\Saves\" + DateTime.Now.ToString().Replace(':', '-');
            Directory.CreateDirectory(path);
            


            WorldController.Save(path, Enviroment.ConvertTo());







            MessageBox.Show("The save is in folder ''" + path + "'' .You can rename it if you want ");
        }



        /// <summary>
        /// For drawing of arrows for next operation in logic blocks (on poison,food and etc)
        /// </summary>
        /// <param name="index">start index</param>
        private void ShowNextBlocks(int index)
        {

            index++;
            if (index >= 64)
                index -= 64;
            var el= LogicBlocksGrid.Children[index++] as Grid;       
            {
                //For food
                var ar = new ArrowLine();
                ar.Stroke = Brushes.Green;
                ar.StrokeThickness = 20;
              
                ar.X1 = el.PointToScreen(new Point(0, 0)).X + el.ActualWidth / 2-20;
                ar.Y1 = el.PointToScreen(new Point(0, 0)).Y + 70 + el.ActualHeight;
               
                ar.Y2 = ar.Y1 -40;
                

                ar.X2 = ar.X1;
               


                AllGrid.Children.Add(ar);
                ar.BringIntoView();
                
            }
            if (index >= 64)
                index -= 64;
            el = LogicBlocksGrid.Children[index++] as Grid;
            {
                //For poison

                var ar = new ArrowLine();
                ar.Stroke = Brushes.Red;
                ar.StrokeThickness = 20;

                ar.X1 = el.PointToScreen(new Point(0, 0)).X + el.ActualWidth / 2 - 20;
                ar.Y1 = el.PointToScreen(new Point(0, 0)).Y + 70 + el.ActualHeight;

                ar.Y2 = ar.Y1 - 40;


                ar.X2 = ar.X1;



                AllGrid.Children.Add(ar);
                ar.BringIntoView();
            }
            if (index >= 64)
                index -= 64;
            el = LogicBlocksGrid.Children[index++] as Grid;
            {
                //For creature
                var ar = new ArrowLine();
                ar.Stroke = Brushes.Blue;
                ar.StrokeThickness = 20;

                ar.X1 = el.PointToScreen(new Point(0, 0)).X + el.ActualWidth / 2 - 20;
                ar.Y1 = el.PointToScreen(new Point(0, 0)).Y + 70 + el.ActualHeight;

                ar.Y2 = ar.Y1 - 40;


                ar.X2 = ar.X1;



                AllGrid.Children.Add(ar);
                ar.BringIntoView();
            }
            if (index >= 64)
                index -= 64;
            el = LogicBlocksGrid.Children[index++] as Grid;
            {
                //For empty

                var ar = new ArrowLine();
                ar.Stroke = Brushes.AntiqueWhite;
                ar.StrokeThickness = 20;

                ar.X1 = el.PointToScreen(new Point(0, 0)).X + el.ActualWidth / 2 - 20;
                ar.Y1 = el.PointToScreen(new Point(0, 0)).Y + 70 + el.ActualHeight;

                ar.Y2 = ar.Y1 - 40;


                ar.X2 = ar.X1;



                AllGrid.Children.Add(ar);
                ar.BringIntoView();
            }
            if (index >= 64)
                index -= 64;
            el = LogicBlocksGrid.Children[index++] as Grid;
            {

                //For wall

                var ar = new ArrowLine();
                ar.Stroke = Brushes.DarkGray;
                ar.StrokeThickness = 20;

                ar.X1 = el.PointToScreen(new Point(0, 0)).X + el.ActualWidth / 2 - 20;
                ar.Y1 = el.PointToScreen(new Point(0, 0)).Y + 70 + el.ActualHeight;

                ar.Y2 = ar.Y1 - 40;


                ar.X2 = ar.X1;


                
                AllGrid.Children.Add(ar);
                ar.BringIntoView();
            }
            
        }

        private void Exit()
        {
            
            var answer = MessageBox.Show("Are you sure?", "Exit Window", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                var answer2 = MessageBox.Show("Do you want to save?", "Save Window", MessageBoxButton.YesNo);

                if (answer2 == MessageBoxResult.Yes)
                    SaveButton_Click(null, null);
                
                this.Close();
                App.Current.Shutdown();
            }
            else if (answer == MessageBoxResult.No)
            {
                MessageBox.Show("Good choice");
            }
        }
    }
}
