using System;
using System.Collections.Generic;


namespace Modal
{
    /// <summary>
    /// Life thing for evolution
    /// </summary>
    public class Creature :WorldObject
    {
        /// <summary>
        /// Health of Creature
        /// <para>Max health is 100</para>
        /// <para>Min is 0 and creature die</para>
        /// </summary>
        public int Health {
            get => health;
            set
            {
                
                health = value;
                if (Health > 90)
                    Health = 90;
                if (Health < 0)
                    Health = 0;
            }
        }
        private int health;


        /// <summary>
        /// Direction of creature (It is turned like clock , 1 is top)
        /// </summary>
        public int SeeDirection {
            get => seedirection;

            set
            {
                if (value < 1 )
                    throw new ArgumentOutOfRangeException("Value must be more than 1 ");
                //Max value is 8 . If it is bigger we set it smaller
                 while (value > 8)
                    value -= 8;
                seedirection = value;
            }
                
                }
        private int seedirection;

       


        /// <summary>
        /// Constructor
        /// </summary>
        public Creature(int x,int y):base(false,0)
        {
            X = x;
            Y = y;
            Health = 10;
            
        }
        public Creature():this(0,0)
        {

        }
    }
}
