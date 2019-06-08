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

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                       Map[x, y] = new Wall(x,y);
                    

                }
            }
            EmpetyCells -= width * 2 + height * 2 - 4;
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

           
            for (int i = 0; i < count; i++)
            {
                int x = Seed.Next(1, Width - 1);
                int y = Seed.Next(1, Height - 1);
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
        public void GenerateFood(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");

            FoodOnMap += count;
            EmpetyCells -= count;
           
            for (int i = 0; i < count; i++)
            {
                int x = Seed.Next(0, Width - 1);
                int y = Seed.Next(0, Height - 1);
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

        /// <summary>
        /// Create random poison on map
        /// </summary>
        /// <param name="count">Count of poison</param>
        public void GeneratePoison(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");

            PoisonOnMap += count;
            EmpetyCells -= count;
           
            for (int i = 0; i < count; i++)
            {
                int x = Seed.Next(0, Width - 1);
                int y = Seed.Next(0, Height - 1);
                if (Map[x, y] == null)
                {
                    Map[x, y] = new Poison(x, y);
                }
                else
                {
                    i--;
                    continue;
                }
            }
        }
        #endregion

        
    }
}
