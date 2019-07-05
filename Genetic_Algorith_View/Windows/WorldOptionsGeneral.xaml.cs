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
using Genetic_Algorith_View.Windows;

namespace Genetic_Algorith_View
{
    /// <summary>
    /// Логика взаимодействия для WorldOptions.xaml
    /// </summary>
    public partial class WorldOptions : Window
    {
        Int32? Seed=null;
        Int32 Width=new Random().Next(60,120);
        Int32 Height=new Random(Guid.NewGuid().GetHashCode()).Next(80,130);

        public WorldOptions()
        {
            InitializeComponent();
            SeedInput.TextChanged += SeedInput_TextChanged;
            WidthInput.TextChanged += WidthInput_TextChanged;
            HeightInput.TextChanged += HeightInput_TextChanged;
        }

        private void HeightInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(HeightInput.Text.Replace(" ", ""), out Height))
            {
                if (HeightInput.Text.Length < 2)
                    return;
                if (Height <= 2)
                {
                    MessageBox.Show("This value must be greater than 3");
                    Height = 3;
                    HeightInput.Text = 3.ToString();
                }
            }
            else if (String.IsNullOrWhiteSpace(HeightInput.Text)||String.IsNullOrEmpty(HeightInput.Text))
            {


            }
            else
            {
                MessageBox.Show("This block contains only integers");
               HeightInput.Text = HeightInput.Text.Remove(HeightInput.Text.Length - 1);
            }
        }

        private void WidthInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(WidthInput.Text.Replace(" ",""), out Width))
            {
                if (WidthInput.Text.Length < 2)
                    return;
                
                if(Width<=2)
                {
                    MessageBox.Show("This value must be greater than 3");
                    Width = 3;
                    WidthInput.Text = "3";
                }
            }
            else if(String.IsNullOrWhiteSpace(WidthInput.Text)||String.IsNullOrEmpty(WidthInput.Text))
            {
                
              
            }
            else
            {
                MessageBox.Show("This block contains only integers");
                WidthInput.Text = WidthInput.Text.Remove(WidthInput.Text.Length - 1);
            }
        }

        private void SeedInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(int.TryParse(SeedInput.Text.Replace(" ", ""), out int temp))
            {
                Seed = temp;
            }
            else if (String.IsNullOrWhiteSpace(SeedInput.Text))
            {


            }
            else
            {
                MessageBox.Show("This block contains only integers");
                SeedInput.Text = SeedInput.Text.Remove(SeedInput.Text.Length - 1);
            }
        }

        private void NewWorld_Click(object sender, RoutedEventArgs e)
        {
            if (Width * Height - 2 * Width - 2 * Height + 4 < 64 +10)
            {
                MessageBox.Show("Area of the world is too small. Try bigger numbers.");
               
                return;

            }

            
                if (Width == 0)
                    Width = new Random().Next(50, 150);
                if (Height == 0)
                    Height = new Random(Guid.NewGuid().GetHashCode()).Next(50, 150);
           
                App.Map = new Controller.MapController(Width, Height, Seed ?? new Random().Next(-100,100));


            try { 
                App.WorldScreen = new World(App.Map.Clone(),true);

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                App.MainScreen.Show();
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
                App.WorldScreen.Show();
                return;
            }


            App.WorldScreen.Show();
            App.MainScreen.Hide();
            this.Close();
        }
    }
}
