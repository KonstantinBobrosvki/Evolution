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
        public CreatureController Subject {
            get => Creatures[0];
            private set
            {
                Creatures[0] = value;
            }
        }

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
           var result =Subject.Think();
           
            if(Subject.Health<=0)
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
            result.AddRange(CheckMinimum());
            if (drawer != null)
            {
                foreach (var item in result)
                {
                    drawer(item.X, item.Y);
                }
            }
          
        }
        protected override void Restart()
        {
            if(CurrentTurns>MaxTurns)
            {
                MaxTurns = CurrentTurns;
            }
            CurrentTurns = 0;
            CreatureController newCreature = Subject.GetChildrens(1, 0)[0];
            CurrentMap = StartMap.Clone();
            var cell = CurrentMap.FreePosition();
            CurrentMap[cell.Item1, cell.Item2] = new CreatureBody();
            newCreature.Body = CurrentMap[cell.Item1, cell.Item2] as CreatureBody;
            RaiseRestart(new NewGenerationEventArgs(new List<CreatureController>() { Subject }, CurrentTurns));
            Subject = newCreature;
        }
        public void ChangeSubject(int [] brainArray)
        {
            Subject = new CreatureController(brainArray);
            Restart();
        }
    }
}
