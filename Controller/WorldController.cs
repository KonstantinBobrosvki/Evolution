using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;

namespace Controller
{
    public static class WorldController
    {
        public static MapController Map { get;private set; }
        public static List<CreatureController> Population { get => Map.Population; }

       
        public static void Start()
        {

            Map = new MapController(100, 50);
            CreatureController.WorldMap = Map;

           
           
        }
        public static void NextTurn()
        {
            for (int i = 0; i < Population.Count; i++)
            {
                var item = Population[i];
                if (item.Body == null)
                {
                    Population.Remove(item);
                    i--;
                    continue;
                }
                item.Live();
                if (item.Body == null)
                {
                    Population.Remove(item);
                    i--;
                   
                }
            }

            if(Population.Count<=8)
            {
                Reset();
            }
            
        }
        private static void Reset()
        {
            BrainsTypes.CreatureBrain[] creatureBrains = new BrainsTypes.CreatureBrain[64];
            int i = 0;
            foreach (var item in Population)
            {
                for (int j = 0; j < 8; j++)
                {
                    creatureBrains.SetValue(item.Brain.Clone(), i++);

                }
                creatureBrains[i - 1].Mutate();
            }
            while(i<64)
            { Random r = new Random();
                var te = Population[r.Next(0, Population.Count)].Brain.Clone();
                te.Mutate();
                creatureBrains.SetValue(te, i++);
            }
            Map.Reset(creatureBrains);
        }

       
    }

   
}
