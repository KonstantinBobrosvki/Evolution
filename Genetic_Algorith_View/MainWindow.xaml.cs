using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.IO;
using Controller;
using System.Collections.Generic;
namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            App.MainScreen = this;
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            WorldOptions worldOptions = new WorldOptions();
            worldOptions.Show();
            
          
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult dialogresult = dialog.ShowDialog();

            string path = dialog.SelectedPath;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            try
            {
                if (dialogresult == System.Windows.Forms.DialogResult.OK)
                {
                    //For APP
                    using (StreamReader reader = new StreamReader(new FileStream(path + @"\App.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        App.Height = int.Parse(reader.ReadLine());
                        App.Width = int.Parse(reader.ReadLine());
                        App.MinFood = int.Parse(reader.ReadLine());
                        App.MinPoison = int.Parse(reader.ReadLine());
                        App.ChangeMap = bool.Parse(reader.ReadLine());
                        App.CreaturesCount = int.Parse(reader.ReadLine());
                        App.MinimumForNewGeneration = int.Parse(reader.ReadLine());

                    }

                    //For cratures
                    List<CreatureController> creatures = new List<CreatureController>(App.CreaturesCount);
                    using (FileStream stream = new FileStream(path + @"\Creatures.dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        creatures = (List<CreatureController>)binaryFormatter.Deserialize(stream);
                    }


                    //For map
                    using (FileStream stream = new FileStream(path + @"\Map.dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        CreatureController.Map = (MapController)binaryFormatter.Deserialize(stream);
                    }

                    App.Map = CreatureController.Map;

                    App.WorldScreen = new Windows.World(creatures, App.Map);

                    using (StreamReader reader = new StreamReader(new FileStream(path + @"\WorldDatas.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        App.WorldScreen.MaxTurns = int.Parse(reader.ReadLine());
                        App.WorldScreen.CurrentTurns = int.Parse(reader.ReadLine());
                        App.WorldScreen.GenerationsCount = int.Parse(reader.ReadLine());
                        App.WorldScreen.AllTurns = int.Parse(reader.ReadLine());
                        App.WorldScreen.Elapsed= TimeSpan.Parse(reader.ReadLine());

                        

                    }

                   

                    App.WorldScreen.Show();



                    this.Hide();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File "+ex.FileName +" not found.Maybe you deleted it");
                throw;
            }
            

        }
    }
}
