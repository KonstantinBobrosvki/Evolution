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
            if (App.StartScreen != null)
                throw new Exception("WTF");
            App.StartScreen = this;
            App.UsingWindows.Add(this);
            App.CurrentMain = this;
        }

        private void NewWorld()
        {
            WorldOptions worldOptions = new WorldOptions();
            App.UsingWindows.Add(worldOptions);

            worldOptions.ShowDialog();
            
          
        }

        private void LoadWorld()
        {
            
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = App.PathToFolder;
            
            System.Windows.Forms.DialogResult dialogresult = dialog.ShowDialog();

            
          
            try
            {
                if (dialogresult == System.Windows.Forms.DialogResult.OK)
                {


                   
                        App.WorldController = WorldController.Load(dialog.SelectedPath);

                        //It works .Some bugs from past
                        App.WorldController.CurrentMap = App.WorldController.CurrentMap;
                   
                    App.WorldScreen = new Windows.World();

                    App.UsingWindows.Add(App.WorldScreen);
                    App.CurrentMain = App.WorldScreen;

                   



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
            catch (Exception ex)
            {
                MessageBox.Show("This is strange error");
                MessageBox.Show(ex.Message);
             
                return;

            }


        }

        private void EnviromentButton_Click(object sender, RoutedEventArgs e)
        {
           var choise= MessageBox.Show("Do you want to create new world? \n (Yes for new/No for load)", "Eviroment", MessageBoxButton.YesNo);

            if (choise == MessageBoxResult.Yes)
                NewWorld();  
            else
                LoadWorld();


        }

        private void NewAIButton_Click(object sender, RoutedEventArgs e)
        {
            var tempo = new Windows.ChooseAIType();
            App.UsingWindows.Add(tempo);
            App.CurrentMain = tempo;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key==System.Windows.Input.Key.Escape)
            {
                var result = MessageBox.Show("Are you sure?", "Exit", MessageBoxButton.YesNo);
                if(result==MessageBoxResult.Yes)
                {
                    App.Close(null,null);
                }
                else
                {

                }
            }
        }
    }
}
