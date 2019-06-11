using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modal;
using Controller;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tests
{

    [TestClass]
    public class AdvancedTests
    {
       
        [TestMethod()]
        public void MoveTest()
        {
            #region Check with walls
            {
                var world = new WorldObject[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            continue;
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random());


                var controller = new CreatureController(1, 1);


                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            continue;
                        else
                        {
                            controller.Move(i, j);
                            Assert.AreEqual(1, controller.Body.X);
                            Assert.AreEqual(1, controller.Body.Y);
                        }
                    }
                }
            }
            #endregion

            #region Check with food
            {
                var world = new WorldObject[4, 3];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            world[i, j] = new Food(i, j);
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random());


                var controller = new CreatureController(2, 1);

                //Go on Food cell
                controller.Move(1, 1);
                Assert.AreEqual(1, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(19, controller.Body.Health);
                //Return on start
                controller.Move(2, 1);
                Assert.AreEqual(2, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
            }
            #endregion

            #region Check with Poison
            {
                var world = new WorldObject[4, 3];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            world[i, j] = new Poison(i, j);
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random());


                var controller = new CreatureController(2, 1);

                //Go on Poison cell cell
                controller.Move(1, 1);
                Assert.AreEqual(2, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(0, controller.Body.Health);
            }
            #endregion
        }






        [TestMethod]
        public void CatchTest()
        {
           
        }
        

     }

}
