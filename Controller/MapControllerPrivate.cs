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
        
        public int FoodOnMap { get; private set; }

        public int PoisonOnMap { get;private set;}

        #region Generate map Methods
        /// <summary>
        /// Create world border
        /// </summary>
        private void GenerateBorder()
        {
            var width = Map.GetLength(0);
            var height = Map.GetLength(1);
            EmpetyCells -= Width * 2 + Height * 2 - 4;

            for (int i = 0; i < Width; i++)
            {
                Map[i, 0] = new Wall(i, 0);
                Map[i, height - 1] = new Wall(i, height - 1);

            }
            for (int y = 1; y < height-1; y++)
            {

                Map[0, y] = new Wall(0, y);
                Map[width-1,y] = new Wall(width - 1, y);

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

            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");

            EmpetyCells -= count;

            Random rnd = new Random(Seed+1);
           
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(1, Width - 1);
                int y = rnd.Next(1, Height - 1);
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

        /// <summary>
        /// Create random food on map
        /// </summary>
        /// <param name="count">Count of food</param> 
        /// <returns>Cell with new food</returns>
        public List<(int ,int)> GenerateFood(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");

            FoodOnMap += count;
            EmpetyCells -= count;

            Random rnd = new Random(Seed + 2);

            var result = new List<(int, int)>(count);
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(1, Width - 1);
                int y = rnd.Next(1, Height - 1);
                if (Map[x, y] == null)
                {
                    Map[x, y] = new Food(x, y);
                    result.Add((x, y));
                }
                else
                {
                    i--;
                    continue;
                }
            }

            return result;
        }

        /// <summary>
        /// Create random poison on map
        /// </summary>
        /// <param name="count">Count of poison</param>
        /// <returns>Cell index with new food</returns>
        public List<(int, int)> GeneratePoison(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");

            var result = new List<(int, int)>(count);

            PoisonOnMap += count;
            EmpetyCells -= count;

            Random rnd = new Random(Seed + 3);
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(0, Width - 1);
                int y = rnd.Next(0, Height - 1);
                if (Map[x, y] == null)
                {
                    Map[x, y] = new Poison(x, y);
                    result.Add((x, y));

                }
                else
                {
                    i--;
                    continue;
                }
            }

            return result;
        }
        #endregion

        
    }
}
