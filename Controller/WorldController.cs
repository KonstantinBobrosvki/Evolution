using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;

namespace Controller
{
    public  partial class WorldController
    {
        public delegate void Drawer(int x, int y);

        public event Action<object, NewGenerationEventArgs> RestartEvent;

        public NewGenerationEventArgs LastRestart { get; private set; }

        /// <summary>
        ///  One turn existing
        /// </summary>
        /// <param name="drawer">Delegate for drawing.Set null=no drawing</param>
        public void WorldLive(Drawer drawer)
        {
            CurrentTurns++;
            for (int i = 0; i < Creatures.Count; i++)
            {
                var item = Creatures[i];

                var Interacted = item.Think();

                if (item.Health == 0)
                {
                    CreatureController.Map[item.X, item.Y] = null;


                    Creatures.RemoveAt(i);
                    i--;



                    if (Creatures.Count == 8)
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

                }

                Interacted.AddRange(CheckMinimum());

                if (drawer != null)
                {
                    foreach (var position in Interacted)
                    {
                        drawer(position.X, position.Y);
                    }
                }


            }
            

        }


        public WorldController(MapController map,List<CreatureController> creatures)
        {
            if (creatures == null)
                throw new ArgumentNullException();

            if (map == null)
                throw new ArgumentNullException();

            if (map.EmpetyCells < 80)
                throw new ArgumentException("Too small map");

            this.StartMap = map.Clone();
            CurrentMap = StartMap.Clone();

            MinFood = CurrentMap.EmpetyCells / 30;
            MinPoison = CurrentMap.EmpetyCells / 60;

            Creatures = creatures;
        }
        public WorldController(MapController map)
        {
            if (map == null)
                throw new ArgumentNullException();

            if (map.EmpetyCells < 80)
                throw new ArgumentException("Too small map");

            this.StartMap = map.Clone();
            CurrentMap = StartMap.Clone();
            Creatures = new List<CreatureController>(64);

            for (int i = 0; i < 64; i++)
            {
                var temp = map.FreePosition();
                Creatures.Add(new CreatureController(temp.Item1, temp.Item2));
            }

            MinFood = CurrentMap.EmpetyCells / 30;
            MinPoison = CurrentMap.EmpetyCells / 60;
        }
    }
}
