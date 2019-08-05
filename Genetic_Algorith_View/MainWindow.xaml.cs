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
            if(Windows.World.AlreadyOpened)
            {
                MessageBox.Show("You already opened world screen.");
                App.WorldScreen.Show();
                return;
            }
            WorldOptions worldOptions = new WorldOptions();
            worldOptions.ShowDialog();
            
          
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (Windows.World.AlreadyOpened)
            {
                MessageBox.Show("You already opened world screen.");
                App.WorldScreen.Show();
                return;
            }
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = App.PathToFolder;
            
            System.Windows.Forms.DialogResult dialogresult = dialog.ShowDialog();

            string path = dialog.SelectedPath;
          
            try
            {
                if (dialogresult == System.Windows.Forms.DialogResult.OK)
                {


                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    if (!Directory.Exists(App.PathToFolder + @"\Saves"))
                    {
                        Directory.CreateDirectory(App.PathToFolder + @"\Saves");
                    }
                   

                    using (FileStream stream = new FileStream(path + @"\WorldController.dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                       App.WorldController=(WorldController) binaryFormatter.Deserialize(stream);

                        //It works
                        App.WorldController.CurrentMap = App.WorldController.CurrentMap;
                    }


                    App.WorldScreen = new Windows.World();

                    App.WorldScreen.Show();



                    this.Hide();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File "+ex.FileName +" not found.Maybe you deleted it");
                return;
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                MessageBox.Show("Some files were changed.You musn't do it.");
                return;

            }
            //catch(ArgumentNullException ex)
            //{
            //    throw ex;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("This is strange error");
            //    MessageBox.Show(ex.Message);
            //    MessageBox.Show(ex.StackTrace);
            //    MessageBox.Show(ex.GetType().ToString());
            //    return;

            //}

           
        }
    }
}
