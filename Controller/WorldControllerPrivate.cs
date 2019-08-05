using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;

namespace Controller
{ 
    [Serializable]
   public partial class WorldController
   {
        public MapController CurrentMap {
            get => currentmap;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                currentmap = value;
                CreatureController.Map = value;
            }
        }
        private MapController currentmap;
        public readonly MapController StartMap;

        public List<CreatureController> Creatures { get; private set; }

        #region Properties
        public long MaxTurns
        {
            get => maxturns;
            private set
            {
                maxturns = value;
              
            }
        }
        private long maxturns;

        public long CurrentTurns
        {
            get => currentTurns;
            private set
            {
                currentTurns = value;
                
            }
        }
        private long currentTurns;

        public long AvarangeTurns
        {
            get => AllTurns / GenerationsCount;
        }

        public long GenerationsCount
        {
            get => generationsCount;
            private set
            {
                generationsCount = value;
            }
        }
        private long generationsCount=1;

        public int MinFood
        {
            get => minfood;
            private set
            {
                if (value < 0)
                    throw new ArgumentException();
                minfood = value;
                CheckMinimum();
            }
        }
        private int minfood;

        public int MinPoison
        {
            get => minpoison;
            private set
            {
                if (value < 0)
                    throw new ArgumentException();
                minpoison = value;
                CheckMinimum();
            }
        }
        private int minpoison;

        public long AllTurns
        {
            get => allturns;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                allturns = value;
            }
        }
        private long allturns;
        #endregion

        #region Methods

        private List<(int, int)> CheckMinimum()
        {
            List<(int, int)> result = new List<(int, int)>();
            if (CurrentMap.FoodOnMap < MinFood)
            {
                
                result.AddRange( CurrentMap.GenerateFood(CurrentMap.EmpetyCells/30));              
            }
            if (CurrentMap.PoisonOnMap < MinPoison)
            {
                var changed = CurrentMap.GeneratePoison(CurrentMap.EmpetyCells/50);
                result.AddRange(changed);
            }
            return result;
        }

        /// <summary>
        /// Actions of creatures
        /// </summary>
        /// <returns>Changed cell or null when all cells are changed</returns>

        private void Restart()
        {
            if (MaxTurns < CurrentTurns)
            {
                MaxTurns = CurrentTurns;
            }

            AllTurns += CurrentTurns;


            if (RestartEvent != null)
            {
                RestartEvent.Invoke(this, new NewGenerationEventArgs(Creatures, CurrentTurns));
            }
            LastRestart = new NewGenerationEventArgs(Creatures, CurrentTurns);

            CurrentTurns = 0;

            GenerationsCount++;


            var newpopulation = new List<CreatureController>(64);
            for (int i = 0; i < 8; i++)
            {
                var item = Creatures[i];
                newpopulation.AddRange(item.GetChildrens(8, 2));
            }



            CurrentMap = StartMap.Clone();



            foreach (var item in newpopulation)
            {
                var place = CreatureController.Map.FreePosition();
                if (place is null)
                {
                    throw new InvalidOperationException();
                }
                var body = new CreatureBody();
                CreatureController.Map[place.Item1, place.Item2] = body;
                item.Body = body;
            }
            for (int i = newpopulation.Count; i < 64; i++)
            {

                for (int x = 1; x < CurrentMap.Width - 1; x++)
                {
                    for (int y = 1; y < CurrentMap.Height - 1; y++)
                    {
                        var cell = CurrentMap[x, y];
                        if (cell is Wall || cell is CreatureBody)
                        {

                        }
                        else
                        {
                            CurrentMap[x, y] = new CreatureBody();

                            newpopulation[i++].Body = (CreatureBody)CurrentMap[x, y];
                        }
                    }
                }
            }

           

            Creatures = newpopulation;
            CheckMinimum();
        }
        #endregion
    }
}
