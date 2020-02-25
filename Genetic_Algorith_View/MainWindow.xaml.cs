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
         
        }

        private void NewWorld()
        {


            WorldOptions worldOptions = new WorldOptions();
            
            worldOptions.ShowDialog();
            var worldcontroller = worldOptions.Result;
            if (worldcontroller is null)
                return;
            var worldWindow = new Windows.World(worldcontroller);
            
            worldWindow.Show();
            this.Hide();
           

        }

        private void LoadWorld()
        {
            
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = Directory.GetCurrentDirectory();
            
            System.Windows.Forms.DialogResult dialogresult = dialog.ShowDialog();

            
          
            try
            {
                if (dialogresult == System.Windows.Forms.DialogResult.OK)
                {


                   
                       WorldController controller= WorldController.Load(dialog.SelectedPath);

                        //It works .Some bugs from past
                    controller.CurrentMap =controller.CurrentMap;
                    this.Hide();
                    var WorldScreen = new Windows.World(controller);
                    WorldScreen.Show();
                    
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Файл "+ex.FileName +" не е намерен.");
                return;
            }          
            catch (Exception ex)
            {
                MessageBox.Show("Това е странна грешка");
                MessageBox.Show(ex.Message);
             
                return;

            }


        }

        private void EnviromentButton_Click(object sender, RoutedEventArgs e)
        {
           var choise= MessageBox.Show("Искате ли да създадете нов свят? \n (Да за нов/Не за зареждане)", "Светове", MessageBoxButton.YesNo);

            if (choise == MessageBoxResult.Yes)
                NewWorld();  
            else
                LoadWorld();


        }

        private void NewAIButton_Click(object sender, RoutedEventArgs e)
        {
            var tempo = new Windows.ChooseAIType();
            tempo.Show();
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key==System.Windows.Input.Key.Escape)
            {
                var result = MessageBox.Show("Сигурни ли сте?", "Изход", MessageBoxButton.YesNo);
                if(result==MessageBoxResult.Yes)
                {
                    this.Close();
                }
                else
                {

                }
            }
            else if(e.Key==System.Windows.Input.Key.H)
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\Instruct.docx");
            }
        }
    }
}
