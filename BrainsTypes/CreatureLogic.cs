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
    public abstract class CreatureBrain
    {
        /// <summary>
        /// Do next action
        /// </summary>
        public abstract Modal.Action Think(WorldObject[] near,CreatureBody.SeeDirection direction);

        /// <summary>
        /// Clone current brain
        /// </summary>
        /// <returns>this</returns>
        public abstract CreatureBrain Clone();

        /// <summary>
        /// Returns type of worldobjects what was interacted
        /// </summary>
        /// <param name="near">All near elements</param>
        /// <param name="direction"></param>
        /// <param name="rotateTimes"></param>
        /// <returns></returns>
        protected static WorldObject WhatInCell(WorldObject[] near, CreatureBody.SeeDirection direction, Modal.Action.RotateTimes rotateTimes)
        {
            if (near.Length != 8)
                throw new Exception("Must be 8 near");

            int temp = (int)direction + (int)rotateTimes;

            while (temp >= 8)
                temp -= 8;

            if (temp >= near.Length)
                throw new Exception("WTF");

            WorldObject forret = near[temp];

           
            

            return forret;
        }

        public CreatureBrain()
        {

        }

    }
   


   
}
