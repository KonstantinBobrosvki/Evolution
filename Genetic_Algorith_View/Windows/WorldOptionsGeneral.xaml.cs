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
        Int32 Width;
        Int32 Height;

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
                }
            }
            else if (String.IsNullOrWhiteSpace(HeightInput.Text))
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
                }
            }
            else if(String.IsNullOrWhiteSpace(WidthInput.Text))
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
           
           if(Seed==null)
           {
                if (Width == 0)
                    Width = new Random().Next(50, 150);
                if (Height == 0)
                    Height = new Random(Guid.NewGuid().GetHashCode()).Next(50, 150);


                App.Height = Height;
                App.Width = Width;
                App.ChangeMap = true;
            }
           else
            {
                App.Map = new Controller.MapController(Width, Height, (int)Seed);
                App.ChangeMap = false;
            }

            App.WorldScreen = new World();
            App.WorldScreen.Show();
            this.Close();
        }
    }
}
