using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modal;

namespace Controller
{
   public partial class CreatureController
    {
       
        #region Some things for AI
        /// <summary>
        /// Main Brain
        /// </summary>
        private int[] LogicBlocks = new int[64];
        /// <summary>
        /// For easier work
        /// </summary>
        private int Current { get => NotUseCurrent; set { if (value < 0) throw new ArgumentOutOfRangeException(); if (value >= 64) value -= 64; NotUseCurrent = value; } } 
        /// <summary>
        /// Do not use it anywhere
        /// </summary>
        private int NotUseCurrent=0;
        #endregion

        /// <summary>
        /// Do some action 
        /// </summary>
        /// <returns>Interacted Cells</returns>
        private List<(int,int)> ThinkPrivate()
        {
            var result = new List<(int, int)>(2);
            result.Add((X, Y));
            for(int i=0;i<10;i++)
            {
                var actioncode = LogicBlocks[Current];

                if (actioncode < 0)
                {
                    throw new Exception("WTF");
                }
                //For rotating
                else if (actioncode < 8)
                {
                    Rotate(actioncode);
                    Current++;
                }
                //For seeing
                else if (actioncode < 16)
                {
                    var type = See(actioncode - 8);
                    if (type is null)
                        Current++;
                    else if (type is CreatureBody)
                        Current += 2;
                    else if (type is Food)
                        Current += 3;
                    else if (type is Poison)
                        Current += 4;
                    else if (type is Wall)
                        Current += 5;
                    else
                        throw new Exception("WTF");


                }
                //For Move
                else if (actioncode < 24)
                {
                    var where = IndexOfCell(actioncode - 16);
                    Move(where.Item1, where.Item2);
                    result.Add((where.Item1, where.Item2));
                    break;
                }
                //For Catch
                else if (actioncode < 32)
                {
                    var where = IndexOfCell(actioncode - 24);
                    Catch(where.Item1, where.Item2);
                    result.Add((where.Item1, where.Item2));
                    break;
                }
                //Only adding to current
                //If u want new functions write them here
                else if (actioncode < 64)
                {
                    Current += actioncode;

                }
                //Exception
                else
                    throw new Exception("In ThinkPrivate bigger than 64");
            }
            Body.Health--;
            return result;
        }

        private void EvolvePrivate()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            LogicBlocks[rnd.Next(0, 64)] = rnd.Next(0, 64);
        }

        private List<CreatureController> GetChilds(int count,int mutatecount)
        {
            if (count < 0 || count < mutatecount)
                throw new ArgumentOutOfRangeException();
            var result = new List<CreatureController>(count);
            for (int i = 0; i < count; i++)
            {
                CreatureController c = new CreatureController();
                c.LogicBlocks =(int[]) LogicBlocks.Clone();
                if (mutatecount-- > 0)
                    c.Evolve();
                c.Current = 0;
                result.Add(c);
            }
            return result;
        }
    }
}
