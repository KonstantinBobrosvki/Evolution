using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    [Serializable]
    /// <summary>
    /// This class is food for creatures
    /// </summary>
    public class Food:WorldObject
    {
        public Food(int x,int y):this()
        {
            X = x;
            Y = y;
        }
        public Food() : base(true, 10)
        {

        }
    }
}
