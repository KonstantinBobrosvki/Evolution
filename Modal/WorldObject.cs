using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    [Serializable]
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

        /// <summary>
        /// Position by width
        /// </summary>
        public virtual int X
        {
            get => x;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                x = value;
            }
        }
        private int x;

        /// <summary>
        /// Position on Height
        /// </summary>
        public virtual int Y
        {
            get => y;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                y = value;
            }
        }
        private int y;

        

        protected WorldObject(bool CanMoveTrought, int HealthAfterInteract)
        {
            this.canmovetrought = CanMoveTrought;
            this.healthafterinteract = HealthAfterInteract;
        }
    }
}
