using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// This class is creature brain
    /// </summary>
    public class CreatureLogic
    {
        int current;
        int[] Brain;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreatureLogic()
        {
            Brain = new int[64];
            current = 0;
        }

        public Creature.Actions GetAction()
        {
            Creature.Actions? action=null;


            return action;
        }
        public void GetResultFromAction(WorldObject obj)
        {

        }
    }
}
