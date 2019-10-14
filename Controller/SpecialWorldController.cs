using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;
namespace Controller
{
   public class SpecialWorldController:WorldController
    {
        /// <summary>
        /// Controller for special tests
        /// </summary>
        /// <param name="creature">Creature with logic and without body</param>
        /// <param name="map">Map</param>
        public SpecialWorldController(CreatureController creature,MapController map):base(map,0)
        {
         
            CurrentMap = StartMap.Clone();
            Creatures = new List<CreatureController>();
            creature.MustEvolve = false;
            var bodyPosition = CurrentMap.FreePosition();
            CurrentMap[bodyPosition.Item1, bodyPosition.Item2] = new CreatureBody(bodyPosition.Item1, bodyPosition.Item2);
            creature.Body = CurrentMap[bodyPosition.Item1, bodyPosition.Item2] as CreatureBody;
            Creatures.Add(creature);
        }
        public override void WorldLive(Drawer drawer)
        {
            CurrentTurns++;
           var result =Creatures[0].Think();
           
            if(Creatures[0].Health==0)
            {
                Restart();
                if (drawer != null)
                {
                    for (int x = 1; x < CurrentMap.Width - 1; x++)
                    {
                        for (int y = 1; y < CurrentMap.Height - 1; y++)
                        {
                            drawer(x, y);
                        }
                    }
                }
                return;
            }
            if (drawer != null)
            {
                foreach (var item in result)
                {
                    drawer(item.X, item.Y);
                }
            }
        }
    }
}
