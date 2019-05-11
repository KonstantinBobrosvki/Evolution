using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{

    /// <summary>
    /// Creature can not walk trought wall
    /// </summary>
    public class Wall:WorldObject
    {

        public Wall():this(0,0)
        {

        }

        public Wall(int x,int y):base(false,0)
        {
            X = x;
            Y = y;
        }
    }
}
