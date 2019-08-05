using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    [Serializable]
    public class NewGenerationEventArgs:EventArgs
    {
        public List<CreatureController> Parents { get; }
        public long CurrentLiveTime { get; }

        public NewGenerationEventArgs(List<CreatureController> parents,long livetime)
        {
            if (parents == null)
                throw new ArgumentNullException();
            if (parents.Count != 8)
                throw new ArgumentException();
            if (livetime < 0)
                throw new ArgumentOutOfRangeException();

            Parents = parents;
            CurrentLiveTime = livetime;
           
        }
    }
}
