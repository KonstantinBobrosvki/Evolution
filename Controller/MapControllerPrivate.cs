using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;

namespace Controller
{
    public partial class MapController
    {
        private void GenerateBorder()
        {
            var width = Map.GetLength(0);
            var height = Map.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1)
                        Map[x, y] = new Wall();
                    if (y == 0 || y == height - 1)
                        Map[x, y] = new Wall();

                }
            }
        }

        /// <summary>
        /// Create random walls on map
        /// </summary>
        /// <param name="count">Count of walls</param>
        private void GenerateWalls(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();

            Random rnd = new Random(Seed);
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(0, Width - 1);
                int y = rnd.Next(0, Height - 1);
                if(Map[x,y]==null)
                {
                    Map[x, y] = new Wall(x, y);
                }
                else
                {
                    i--;
                    continue;
                }
            }
        }

        private void GenerateFood(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();

            Random rnd = new Random(Seed);
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(0, Width - 1);
                int y = rnd.Next(0, Height - 1);
                if (Map[x, y] == null)
                {
                    Map[x, y] = new Food(x, y);
                }
                else
                {
                    i--;
                    continue;
                }
            }

        }
    }
}
