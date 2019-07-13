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
        int MapWidth=new Random().Next(90,120);
        int MapHeight =new Random(Guid.NewGuid().GetHashCode()).Next(80,110);

        public WorldOptions()
        {
            InitializeComponent();

            WidthInput.Text = MapWidth.ToString();
            HeightInput.Text = MapHeight.ToString();

            SeedInput.TextChanged += SeedInput_TextChanged;
            WidthInput.TextChanged += WidthInput_TextChanged;
            HeightInput.TextChanged += HeightInput_TextChanged;
        }

        private void HeightInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var me = ((TextBox)sender);
            for (int i = 0; i < SeedInput.Text.Length; i++)
            {
                var symbol = me.Text[i];
                if (!char.IsNumber(symbol))
                {
                    me.Text = me.Text.Remove(i, 1);
                }
            }
            me.CaretIndex = me.Text.Length;

            if (!String.IsNullOrWhiteSpace(me.Text))
            {
                if (int.TryParse(me.Text, out int temp))
                {
                    if (me.Text.Length > 2)
                    {
                        if (temp < 3)
                        {
                            MessageBox.Show("Try bigger value");
                            return;
                        }
                          
                        
                    }
                    MapHeight = temp;
                }
                
            }
      
        }

        private void WidthInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var me = ((TextBox)sender);
            for (int i = 0; i < SeedInput.Text.Length; i++)
            {
                var symbol = me.Text[i];
                if (!char.IsNumber(symbol))
                {
                    me.Text = me.Text.Remove(i, 1);
                }
            }
            me.CaretIndex = me.Text.Length;

            if (!String.IsNullOrWhiteSpace(me.Text))
            {
                if (int.TryParse(me.Text, out int temp))
                { 
                    if (me.Text.Length > 2)
                    {
                        if (temp < 3)
                        {
                            MessageBox.Show("Try bigger value");
                            return;
                        }
                    }
                    MapWidth = temp;
                }
            }

        }

        private void SeedInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var me = ((TextBox)sender);
            for (int i = 0; i < SeedInput.Text.Length; i++)
            {
                var symbol = me.Text[i];
               if(!char.IsNumber(symbol))
                {
                    me.Text= me.Text.Remove(i, 1);
                }
            }
            if(!String.IsNullOrWhiteSpace(me.Text))
            {
                if (int.TryParse(me.Text, out int temp))
                    Seed = temp;
                else
                    MessageBox.Show("Try other numbers");
            }
            me.CaretIndex = me.Text.Length;

        }

        private void NewWorld_Click(object sender, RoutedEventArgs e)
        {
            int square = MapWidth * MapHeight - 2 * MapWidth - 2 * MapHeight + 4;
            if (square< 64)
            {
                MessageBox.Show("Area of the world is too small. Try bigger numbers.");
               
                return;

            }
                App.Map = new Controller.MapController(MapWidth, MapHeight, Seed ?? new Random().Next(-100,100),0,0,square/80);


            try
            {
                App.WorldScreen = new World();

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Input data have errors.Try again");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                App.WorldScreen.Show();
                this.Close();
                return;
            }


            App.WorldScreen.Show();
            App.MainScreen.Hide();
            this.Close();
        }
    }
}
