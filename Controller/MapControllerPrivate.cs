using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;
using BrainsTypes;

namespace Controller
{
    public partial class MapController
    {
        private int FoodOnMap { get; set; } = 0;
        private int PoisonOnMap { get; set; } = 0;

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
            EmpetyCells -= width * 2 + Height * 2 + 4;
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

        /// <summary>
        /// Create random food on map
        /// </summary>
        /// <param name="count">Count of food</param>
        private void GenerateFood(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");
            EmpetyCells -= count;

            FoodOnMap += count;

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

        /// <summary>
        /// Create random poison on map
        /// </summary>
        /// <param name="count">Count of poison</param>
        private void GeneratePoison(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");
            EmpetyCells -= count;

            PoisonOnMap += count;

            Random rnd = new Random(Seed);
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(0, Width - 1);
                int y = rnd.Next(0, Height - 1);
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

        private void GenerateCreatures(int count,CreatureBrain[]Brains=null)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();


            if (EmpetyCells < count)
                throw new ArgumentOutOfRangeException("Not enoght space");
            EmpetyCells -= count;

            Population.Clear();

            Random rnd = new Random(Seed);
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(0, Width - 1);
                int y = rnd.Next(0, Height - 1);
                if (Map[x, y] == null)
                {
                    var creature = new CreatureBody(x, y);
                    CreatureController CreatureControl;
                    if (Brains != null)
                    {
                        CreatureControl = new CreatureController(creature, Brains[i]);
                    }
                    else
                    {
                        CreatureControl = new CreatureController(creature);
                    }
                    Population.Add(CreatureControl);
                    Map[x, y] = creature;
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
