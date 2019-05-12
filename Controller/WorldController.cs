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
            foreach (var item in Population)
            {
                item.Do();
            }
        }
    }
}
