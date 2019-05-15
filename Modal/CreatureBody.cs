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
                    health = 90;
                if (Health <= 0)
                {
                    health = 0;
                    DieEvent?.Invoke(this, new EventArgs());
                }
            }
        }
        private int health;


        /// <summary>
        /// Direction of creature sight
        /// </summary>
        public SeeDirection Sight {
            get =>sight;
            set {
                sight = value;
                TurnedEvent?.Invoke(this, new EventArgs());
            }
        }
        private SeeDirection sight;

        public enum SeeDirection
        {
            Top,
            TopRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
            TopLeft
        }

        public event EventHandler TurnedEvent;

        public event EventHandler MovedEvent;

        public event EventHandler DieEvent;


        public override int X {
            get => base.X;
            set
            {
                if (value == X)
                    return;
                base.X = value;

                if (MovedEvent != null)
                    MovedEvent(this, new EventArgs());
            }
        }

        public override int Y {
            get => base.Y;
            set {
                if (value == Y)
                    return;

                base.Y = value;

                MovedEvent?.Invoke(this, new EventArgs());

            }
        }

        public override bool Equals(object obj)
        {
            if(obj==null || !(obj is CreatureBody))
            {
                return false;
            }
            if(obj is CreatureBody temp)
            {
                if (temp.X == this.X & temp.Y == Y)
                    return true;
               
            }
            return false;
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
