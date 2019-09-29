using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.IO;
using Controller;
using System.Collections.Generic;
using System.Windows.Media;

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
            if (App.MainScreen != null)
                throw new Exception("WTF");
            App.MainScreen = this;         
        }

        private void NewWorld()
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

        private void LoadWorld()
        {
            if (Windows.World.AlreadyOpened)
            {
                MessageBox.Show("You already opened world screen.");
                App.WorldScreen.Show();
                return;
            }
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = App.PathToFolder;
            
            System.Windows.Forms.DialogResult dialogresult = dialog.ShowDialog();

            
          
            try
            {
                if (dialogresult == System.Windows.Forms.DialogResult.OK)
                {


                   
                        App.WorldController = WorldController.Load(dialog.SelectedPath);

                        //It works
                        App.WorldController.CurrentMap = App.WorldController.CurrentMap;
                   
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

        private void EnviromentButton_Click(object sender, RoutedEventArgs e)
        {
           var choise= MessageBox.Show("Do you want to create new world?", "Eviroment", MessageBoxButton.YesNo);

            if (choise == MessageBoxResult.Yes)
                NewWorld();  
            else
                LoadWorld();


        }

        private void NewAIButton_Click(object sender, RoutedEventArgs e)
        {
            var tempo = new Windows.ChooseAIType();
            this.ShowInTaskbar = false;
            tempo.ShowDialog();
            this.Hide();
        }
    }
}
