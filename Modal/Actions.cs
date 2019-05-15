using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    /// <summary>
    /// This class is used for Creature logic
    /// </summary>
    public struct Action
    {
        public enum ActionType
        {
            Move,
            Catch,
            Nothing
        }
        public enum RotateTimes
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven
        }

        public readonly ActionType Act;
        public readonly RotateTimes Rotate;

        public Action(ActionType doing, RotateTimes times)
        {
            Act = doing;
            Rotate = times;
        }

    }
}
