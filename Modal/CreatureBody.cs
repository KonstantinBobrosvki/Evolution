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
        /// Event for diying creature
        /// </summary>
        public event EventHandler DieEvent;
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
        public SeeDirection Sight { get; set; }

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

        public override bool Equals(object obj)
        {
            
            if(obj is CreatureBody temp)
            {
                if (temp.X == this.X & temp.Y == Y)
                    return true;
               
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X*Y+Health;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreatureBody(int x,int y):this()
        {
            X = x;
            Y = y;
           
        }

        public CreatureBody(): base(false, 0)
        {
           
            Health = 10;
            Sight = SeeDirection.Top;
        }
    }
}
