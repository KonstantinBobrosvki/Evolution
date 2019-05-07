using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    /// <summary>
    /// Contains creature logic
    /// </summary>
    public class CreatureLogic
    {
        /// <summary>
        /// Current command index
        /// </summary>
        private int current;
        public int Current {
            get => current;
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                current = value;

                //Max value for current is 64(Logic.Leght is 64)
                if (Current >= 64)
                    Current -= 64;
            }

        }

        /// <summary>
        /// Brain of creature
        /// </summary>
        public int[] Logic {
            get =>logic;
            set {

                if (value == null)
                    throw new ArgumentNullException();

                if (value.Length != 64)
                    throw new ArgumentException();

                logic = value;
                        }

        }
        private int[] logic;


        public CreatureLogic()
        {
            Current = 0;
            Logic = new int[64];
        }
    }
}
