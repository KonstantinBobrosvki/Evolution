using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
   public partial class CreatureController
    {
        private int[] LogicCode;
        private int Current {
            get => current;
            set
            {
                if (value >= 64)
                    value -= 64;
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                current = value;
            }
        }
        private int current;

        private CreatureController()
        {
            LogicCode = new int[64];

        }

        private (int,int) PrivateThink()
        {

            return (0,0);
        }
    }
}
