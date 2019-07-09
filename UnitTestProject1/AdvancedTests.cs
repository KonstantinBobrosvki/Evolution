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
                CreatureController.Map = new MapController(world, new Random().Next());


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
                Assert.AreEqual(controller.Body.Health, 10);
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
                        else if (i == 2 && j == 1)
                            continue;
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random().Next());


                var controller = new CreatureController(2, 1);

                //Go on Food cell
                controller.Move(1, 1);
                Assert.AreEqual(1, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(20, controller.Body.Health);
                //Return on start
                controller.Move(2, 1);
                Assert.AreEqual(2, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(20, controller.Body.Health);
            }
            #endregion

            #region Check with Poison
            {
                var world = new WorldObject[4, 3];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 2 && j == 1)
                            world[i, j] = new Poison(i, j);
                        else if (i == 1 && j == 1)
                            continue;
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random().Next());


                var controller = new CreatureController(1, 1);
                bool died = false;
                controller.Body.DieEvent += (s, o) => died = true;
                //Go on Poison cell cell
                controller.Move(2, 1);
                Assert.AreEqual(1, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(0, controller.Body.Health);
                Assert.IsTrue(died);
            }
            #endregion
        }

        [TestMethod]
        public void CatchTest()
        {

            #region Check with food
            {
                var world = new WorldObject[4, 3];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i == 1 && j == 1) || (i == 2 && j == 1))
                            world[i, j] = new Food(i, j);
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random().Next());


                var controller = new CreatureController(2, 1);

                //Go on Food cell
                controller.Catch(1, 1);
                Assert.AreEqual(2, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(20, controller.Body.Health);
                Assert.AreEqual(null, CreatureController.Map[1, 1]);


            }
            #endregion

            #region Check with Poison
            {
                var world = new WorldObject[4, 3];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i == 2 && j == 1) || (i == 1 && j == 1))
                            world[i, j] = new Poison(i, j);
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random().Next());


                var controller = new CreatureController(1, 1);

                //Go on Poison cell cell
                controller.Catch(2, 1);
                Assert.AreEqual(1, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(typeof(Food), CreatureController.Map[2, 1].GetType());
                Assert.AreEqual(10, controller.Body.Health);

            }
            #endregion

            #region Check with Free space
            {
                var world = new WorldObject[4, 3];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i == 2 && j == 1) || ((i == 1 && j == 1)))
                            world[i, j] = null;
                        else
                            world[i, j] = new Wall(i, j);
                    }
                }
                CreatureController.Map = new MapController(world, new Random().Next());


                var controller = new CreatureController(1, 1);

                //Go on free cell cell
                controller.Catch(2, 1);
                Assert.AreEqual(1, controller.Body.X);
                Assert.AreEqual(1, controller.Body.Y);
                Assert.AreEqual(null, CreatureController.Map[2, 1]);
                Assert.AreEqual(10, controller.Body.Health);

            }
            #endregion
        }

        [TestMethod]
        public void CounterTest()
        {
            MapController map = new MapController(100, 100, DateTime.Now.Millisecond);
            int startfoodcount = map.FoodOnMap;
            int startposioncount = map.PoisonOnMap;
            int startfreecount = map.EmpetyCells;
            //startcheck
            {
                int currfood = 0;
                int currpoison = 0;
                int currfree = 0;

                foreach (var item in map)
                {
                    if (item is Food)
                        currfood++;
                    if (item is Poison)
                        currpoison++;
                    if (item is null)
                        currfree++;
                }
                Assert.AreEqual(startfoodcount, currfood);
                Assert.AreEqual(startposioncount, currpoison);
                Assert.AreEqual(startfreecount, currfree);

            }

            //Randomazing
            {
                Random rnd = new Random();
                var forfod = rnd.Next(1, 20);
                var forPoison = rnd.Next(1, 20);
                var fornull = rnd.Next(1, 20);
                for (int i = 0; i < forfod; i++)
                {
                    var t = map.FreePosition();
                    map[t.Item1, t.Item2] = new Food();
                    if (i % 2 == 0)
                        map[t.Item1, t.Item2] = null;
                }
                for (int i = 0; i < forPoison; i++)
                {
                    var t = map.FreePosition();
                    map[t.Item1, t.Item2] = new Poison();
                    if (i % 2 == 0)
                        map[t.Item1, t.Item2] = null;
                }
                for (int i = 0; i < fornull; i++)
                {
                    var t = map.FreePosition();
                    map[t.Item1, t.Item2] = null;
                }
              
            }
            //Finish check
            {
                int currfood = 0;
                int currpoison = 0;
                int currfree = 0;
                for (int x = 0; x < map.Width; x++)
                {
                    for (int y = 0; y < map.Height; y++)
                    {
                        var item = map[x, y];



                        if (item is Food)
                            currfood++;
                        if (item is Poison)
                            currpoison++;
                        if (item is null)
                            currfree++;
                        if (item != null)
                        {
                            Assert.AreEqual(item.X, x);
                            Assert.AreEqual(item.Y, y);
                        }
                    }
                }

                Assert.AreEqual(map.FoodOnMap, currfood);
                Assert.AreEqual(map.PoisonOnMap, currpoison);
                Assert.AreEqual(map.EmpetyCells, currfree);
            }
        }

        [TestMethod]
        public void RotateTest()
        {
            MapController mapController = new MapController(20, 20, DateTime.Now.Millisecond);
            CreatureController.Map = mapController;
            var temp = mapController.FreePosition();
            var controller = new CreatureController(temp.Item1, temp.Item2);

            for (int i = 1; i < 8; i++)
            {
                controller.Rotate(1);
                Assert.AreEqual(i, (int)controller.Body.Sight);
            }
            controller.Rotate(1);
            Assert.AreEqual(0, (int)controller.Body.Sight);
            for (int i = 0; i < 16; i+=2)
            {
                
                if(i<8)
                Assert.AreEqual(i, (int)controller.Body.Sight);
                if (i == 8)
                    Assert.AreEqual(0, (int)controller.Body.Sight);
                if(i>8)
                 Assert.AreEqual(i-8, (int)controller.Body.Sight);

                controller.Rotate(2);
            }
            Assert.AreEqual(0, (int)controller.Body.Sight);
            controller.Rotate(7);
            Assert.AreEqual(7, (int)controller.Body.Sight);
            controller.Rotate(2);
            Assert.AreEqual(1, (int)controller.Body.Sight);

        }

        [TestMethod]
        public void IndexOfCellTest()
        {
            MapController mapController = new MapController(20, 20, DateTime.Now.Millisecond);
            CreatureController.Map = mapController;
            var temp = mapController.FreePosition();
            var controller = new CreatureController(temp.Item1, temp.Item2);


            Assert.AreEqual(controller.IndexOfCell(0), new Tuple<int, int>(controller.X, controller.Y - 1));
            controller.Rotate(1);
            Assert.AreEqual(controller.IndexOfCell(0), new Tuple<int, int>(controller.X+1, controller.Y - 1));
            controller.Rotate(7);
            Assert.AreEqual(controller.IndexOfCell(0), new Tuple<int, int>(controller.X, controller.Y - 1));
            controller.Rotate(7);
            Assert.AreEqual(controller.IndexOfCell(2), new Tuple<int, int>(controller.X+1, controller.Y - 1));

        }

        [TestMethod]
        public void EqualsTest()
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
            MapController map1 = new MapController(world, new Random().Next());

            if (!map1.Equals(new MapController(world, new Random().Next())))
                Assert.Fail();
        }

        [TestMethod]
        public void MapControllerConstructorTest()
        {
            Random random = new Random();
            MapController map = new MapController(random.Next(60, 120), random.Next(70, 130), random.Next());
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var element = map[x, y];
                    if(x==0||y==0||x==map.Width-1||y==map.Height-1)
                    {
                        if (element is Wall)
                        {
                            Assert.AreEqual(x, element.X);
                            Assert.AreEqual(y, element.Y);
                        }
                        else
                            Assert.Fail();
                    }
                    else
                    {
                        if(!(element is null))
                        {
                            Assert.AreEqual(x, element.X);
                            Assert.AreEqual(y, element.Y);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SeedTest()
        {
            var seed = new Random().Next();
            var width = new Random().Next(70, 140);
            var heigh = new Random().Next(60, 120);
            MapController map1 = new MapController(width, heigh, seed);
            Assert.AreEqual(map1, new MapController(width, heigh, seed));
        }

    }

}
