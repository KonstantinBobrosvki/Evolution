using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{

    public interface ICreature
    {
        /// <summary>
        /// This Method should interact with celss and return their cordinates
        /// </summary>
        /// <returns>Changed cells</returns>
        List<(int, int)> Think();

        /// <summary>
        /// 
        /// </summary>
        void Evolve();

        List<CreatureController> GetChildrens(int count, int mutatecount);

        int GenerationsWithoutEvolution { get; }
    }
}
