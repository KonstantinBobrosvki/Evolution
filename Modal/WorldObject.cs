using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    /// <summary>
    /// This is parent class for all game types in this application
    /// </summary>
    public abstract class WorldObject
    {
        
        /// <summary>
        /// Can we move on cell with this
        /// </summary>
        public  bool CanMoveTrought { get => canmovetrought; }

        /// <summary>
        /// How will be changed health after interacting with this
        /// </summary>
        public  int HealthAfterInteract { get => healthafterinteract; }

        private readonly bool canmovetrought;
        private readonly int healthafterinteract;

        protected WorldObject(bool CanMoveTrought, int HealthAfterInteract)
        {
            this.canmovetrought = CanMoveTrought;
            this.healthafterinteract = HealthAfterInteract;
        }
    }
}
