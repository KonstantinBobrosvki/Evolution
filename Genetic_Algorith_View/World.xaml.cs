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
        System.Windows.Threading.DispatcherTimer timer;
        public World()
        {
            InitializeComponent();
            WorldController.Start();

            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(0,0,0,2);

           

            FullDrawer();
         
        }
        void FullDrawer()
        {
            Field.Children.Clear();
          
            Field.Rows=WorldController.Map.Width ;
            Field.Columns =WorldController.Map.Height;
            
                for (int x = 0; x < WorldController.Map.Width; x++)
                {
                  for (int y = 0; y < WorldController.Map.Height; y++)
                  {
                    var item = WorldController.Map[x, y];
                    int index = y * WorldController.Map.Width + x;


                    var foradd = new WorldBlock(x, y);
                    Field.Children.Add(foradd);
                    
                    
                  
                  }
                }
          


        }
        /// <summary>
        /// Optimized version of full drawer
        /// </summary>
        void ParticialDrawer()
        {

          
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorldController.NextTurn();

            foreach (var creature in WorldController.Population)
            {
                foreach (var incteracted in creature.LastInteractedCells)
                {
                    int index = incteracted.Item2 * WorldController.Map.Width + incteracted.Item1;
                    ((WorldBlock)Field.Children[index]).Reset();
                }
            }
        }
      



 

    private void timer_Tick(object sender, EventArgs e)
    {

            ParticialDrawer();
         
    }

        private void StartStop(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();
            else
                timer.Start();
        }
    }
}
