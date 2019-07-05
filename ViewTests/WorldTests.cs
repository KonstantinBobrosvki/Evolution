using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using Modal;
using Controller;
using System.Windows.Media;

namespace Genetic_Algorith_View.Windows.Tests
{
    [TestClass()]
    public class WorldTests
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        [TestInitialize]
        public void Initialize()
        {
           
            App.MainScreen = new MainWindow();
            App.Map = new MapController(random.Next(70,120),random.Next(70,100),random.Next());
        }

        [TestMethod()]
        public void RedrawTest()
        {
                App.WorldScreen = new World(new MapController(random.Next(20, 100), random.Next(30, 100), 10), true);
            for (int i = 0; i < 1000; i++)
            {
              
                App.WorldScreen.WorldLive();
                for (int x = 0; x < CreatureController.Map.Width; x++)
                {
                    for (int y = 0; y < CreatureController.Map.Height; y++)
                    {
                        var element = CreatureController.Map[x, y];
                        var rect = App.WorldScreen.MapField1.Children[World.GetOneRankIndex(x, y)] as System.Windows.Controls.Grid;
                        SolidColorBrush brush=null;

                        if (element is Wall)
                        {
                            brush = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                        }
                        else if (element is CreatureBody)
                        {

                            brush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                         

                        }
                        else if (element is Food)
                        {
                            brush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                        }
                        else if (element is Poison)
                        {
                            brush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        }
                        else if (element is null)
                        {
                            brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        }

                        Assert.AreEqual(rect.Background.ToString(), brush.ToString(),x.ToString()+" "+y.ToString());
                    }
                }
            }



        }


    }
}