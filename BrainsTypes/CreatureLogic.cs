using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;
namespace BrainsTypes
{
    /// <summary>
    /// Base class for all types creature logic
    /// </summary>
    public abstract class CreatureLogic
    {
       /// <summary>
       /// Check next action
       /// </summary>
       /// <returns>What is next action</returns>
        public abstract Doing Think();

        /// <summary>
        /// We see that we have type of recived after Doing cell
        /// </summary>
        public abstract void ReciveResult(WorldObject typeobject);

        protected bool RecivedResult;

        /// <summary>
        /// Shows is it ready
        /// </summary>
        public bool Ready { get; protected set; }

        /// <summary>
        /// Clone current brain
        /// </summary>
        /// <returns>this</returns>
        public abstract CreatureLogic Clone();

        public CreatureLogic()
        {

        }

    }

    public class Doing
    {
        public enum Actions
        {
            See,
            Rotate,
            Catch,
            Go,
            Nothing

        }
        public enum Rotate
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight

        }

        public readonly Actions actions;
        public readonly Rotate rotate;
        public Doing(Actions act,Rotate rot)
        {
           
            actions = act;
            rotate=rot;
        }

    }
}
