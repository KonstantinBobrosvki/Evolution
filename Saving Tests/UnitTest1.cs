using Microsoft.VisualStudio.TestTools.UnitTesting;
using Controller;
using Modal;
using System;
using System.IO;
namespace Saving_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MapControllerSave()
        {
            var temp = System.AppContext.BaseDirectory+new System.Random().Next(0,100)+".txt";

            MapController map = new MapController(100, 60, 132);
            for (int i = 0; i < 10; i++)
            {
                var tempo = map.FreePosition();
                map[tempo.Item1, tempo.Item2] = new CreatureBody() { Health=9 };
                if(new Random(Guid.NewGuid().GetHashCode()).Next(0,100)>50)
                    map[tempo.Item1, tempo.Item2] = new CreatureBody() { Health = 9 };
                    else
                    map[tempo.Item1, tempo.Item2] = new CreatureBody() { Health = 12,Sight=CreatureBody.SeeDirection.DownRight };

            }

            MapController.Save(temp , map);
 
            Assert.IsTrue(map.Equals(MapController.Load(temp)));
          
        }

        [TestMethod]
        public void CreatureControllerSave()
        {
            var temp = System.AppContext.BaseDirectory + new System.Random().Next(100, 200) + ".txt";

            MapController map = new MapController(100, 60, 132);
            CreatureController.Map = map;
            for (int i = 0; i < 10; i++)
            {
                var tempo = map.FreePosition();
                

                CreatureController creature = new CreatureController(tempo.Item1, tempo.Item2);
                Assert.IsTrue(creature.Equals(creature));
                creature.Catch(tempo.Item1 + 1, tempo.Item2);
                creature.Move(tempo.Item1 , tempo.Item2+1);
                CreatureController.Save(temp, creature);
                var tempo22= CreatureController.Load(temp);

                Assert.IsTrue(creature.Equals(tempo22));
              
            }

           
            
        }

        [TestMethod]
        public void WorldControllerSave()
        {
            var path = System.AppContext.BaseDirectory +"\\" +new System.Random().Next(100, 200);
            Directory.CreateDirectory(path);
            WorldController controller = new WorldController(new MapController(100, 90, new Random().Next(0, 1000)));
            WorldController.Save(path, controller);
            var actual = WorldController.Load(path);
            Assert.IsTrue(actual.CurrentMap.Equals(controller.CurrentMap));
            Assert.IsTrue(actual.StartMap.Equals(controller.StartMap));

            for (int i = 0; i < 64; i++)
            {
                Assert.IsTrue(actual.Creatures[i].Equals(controller.Creatures[i]));
            }
          
          Assert.IsTrue(controller.Equals(actual));
         
        }

    }
}
