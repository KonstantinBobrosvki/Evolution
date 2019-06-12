using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    /// <summary>
    /// Poison damages creatures
    /// </summary>
    public class Poison:WorldObject
    {
        public Poison(int x,int y):this()
        {
            X = x;
            Y = y;

        }

        public Poison() : base(false, -90)
        {

        }
    }
}
