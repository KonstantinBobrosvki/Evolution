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
        public readonly bool CanMoveTrought;
        public readonly int HealthAfterInteract;

        protected WorldObject(bool CanMoveTrought, int HealthAfterInteract)
        {
            this.CanMoveTrought = CanMoveTrought;
            this.HealthAfterInteract = HealthAfterInteract;
        }
    }
}
