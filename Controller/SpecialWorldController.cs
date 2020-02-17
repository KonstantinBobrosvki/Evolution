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
        /// Casuses on change of EatsAllAvinableFood or HaveLoops
        /// </summary>
        public event EventHandler SpecialInfoChanged;

        /// <summary>
        /// Does subject eats food
        /// </summary>
       public bool EatsAllAvinableFood {
            get =>eatsAllFood;
            private set {
                if (value != eatsAllFood)
                {
                    eatsAllFood = value;
                    SpecialInfoChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        private bool eatsAllFood;
        /// <summary>
        /// Does subject have bugs in logic
        /// </summary>
        public bool HaveLoops {
            get =>haveLoops;
            private set
            {
                if (value != haveLoops)
                {
                    haveLoops = value;
                    SpecialInfoChanged?.Invoke(this, new EventArgs());
                }

            }
        }
        private bool haveLoops;
        /// <summary>
        /// Main creature
        /// </summary>
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



            #region Crutch for optimization
            var nearFood = 0;

            var temp_X = Subject.X;
            var temp_Y = Subject.Y;
            var temp_Health = Subject.Health;
            //Checks if subject it all food

            if (CurrentMap[Subject.X - 1, Subject.Y - 1] is Food)
                nearFood++;
            if (CurrentMap[Subject.X - 1, Subject.Y] is Food)
                nearFood++;
            if (CurrentMap[Subject.X - 1, Subject.Y + 1] is Food)
                nearFood++;
            if (CurrentMap[Subject.X, Subject.Y - 1] is Food)
                nearFood++;
            if (CurrentMap[Subject.X, Subject.Y + 1] is Food)
                nearFood++;
            if (CurrentMap[Subject.X + 1, Subject.Y + 1] is Food)
                nearFood++;
            if (CurrentMap[Subject.X + 1, Subject.Y - 1] is Food)
                nearFood++;
            if (CurrentMap[Subject.X + 1, Subject.Y] is Food)
                nearFood++;
            #endregion

           var result =Subject.Think();

           
            #region Another part of crutch
            if (nearFood==0)
            {

            }
            else
            {
                var newnearFood = 0;

                if (CurrentMap[temp_X- 1, temp_Y- 1] is Food)
                   newnearFood++; 
                if (CurrentMap[temp_X - 1,temp_Y] is Food)
                    newnearFood++;
                if (CurrentMap[temp_X - 1,temp_Y + 1] is Food)
                    newnearFood++;
                if (CurrentMap[temp_X,    temp_Y- 1] is Food)
                    newnearFood++;
                if (CurrentMap[temp_X,    temp_Y+ 1] is Food)
                    newnearFood++;
                if (CurrentMap[temp_X + 1,temp_Y + 1] is Food)
                    newnearFood++;
                if (CurrentMap[temp_X + 1,temp_Y - 1] is Food)
                    newnearFood++;
                if (CurrentMap[temp_X + 1,temp_Y] is Food)
                    newnearFood++;   
                
                //If count of food still is same=> subject did not eat that
                if(newnearFood==nearFood)
                {
                    EatsAllAvinableFood = false;
                }
            }
            #endregion


            if (result.Count==1)
            {
                this.HaveLoops = true;
            }

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
            EatsAllAvinableFood = true;
            HaveLoops = false;
            Restart();
        }

        public WorldController ConvertTo()
        {
            var creatures = new List<CreatureController>(64);
            creatures.AddRange(Subject.GetChildrens(32, 0));
            for (int i = 0; i < 32; i++)
            {
                creatures.Add(new CreatureController());
            }
            HashSet<(int, int)> fuck = new HashSet<(int, int)>();
            for (int i = 0; i < 64; i++)
            {
                var item = creatures[i];
                var te = StartMap.FreePosition().ToValueTuple();
                if (!fuck.Contains(te))
                {
                    item.Body = new CreatureBody(te.Item1, te.Item2);
                    fuck.Add(te);
                    continue;
                }
                i--;
            }
           

            var result = new WorldController(this.StartMap, creatures);
            return result;
        }
    }
}
