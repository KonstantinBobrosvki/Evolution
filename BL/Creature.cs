using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public  class Creature:WorldObject
    {
        private int health;
        public int Health {
            get => health;
            private  set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                health = value;
            }
        }
        
      
        public void Think(WorldObject[,] map)
        public override void Interact(WorldObject interacter,Actions action)
        {
           if(interacter is Creature creature)
            {
                switch (action)
                {
                    case Actions.Attack:
                        this.Heal(5);
                        creature.TakeDamage(5);
                        break;
                    case Actions.Grab:
                        this.TakeDamage(5);
                        creature.Heal(5);
                        break;
                    case Actions.Move:
                        break;
                        case Actions.See

                }

            }
        }

        public void Heal(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException();
            Health += value;
        }

        public void TakeDamage(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException();
            Health -= value;
        }
    }
}
