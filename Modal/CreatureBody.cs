using System;
using System.Collections.Generic;


namespace Modal
{
    /// <summary>
    /// Life thing for evolution
    /// </summary>
    public class CreatureBody :WorldObject
    {
        
        /// <summary>
        /// Health of Creature
        /// <para>Max health is 100</para>
        /// <para>Min is 0 when creature die</para>
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

        public event EventHandler Moved;

        public override int X {
            get => base.X;
            set
            {
                if (value == X)
                    return;
                base.X = value;

                if (Moved != null)
                    Moved(this, new EventArgs());
            }
        }

        public override int Y {
            get => base.Y;
            set {
                if (value == Y)
                    return;

                base.Y = value;

                Moved?.Invoke(this, new EventArgs());

            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public CreatureBody(int x,int y):base(false,0)
        {
            X = x;
            Y = y;
            Health = 10;
            
        }
        public CreatureBody():this(0,0)
        {

        }
    }
}
