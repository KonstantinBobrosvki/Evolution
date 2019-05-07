using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public abstract class WorldObject
    {
        public enum Actions
        {
            Move,
            Grab,
            See,
            Rotate,
            Attack
        }

        public abstract void Interact(WorldObject interacter,Actions action);

    }
}
