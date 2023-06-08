using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Engine
{
    internal class RAVEAlgorithm : BasicMCTSAlgorithm
    {
        public RAVEAlgorithm(int seed, int iterations, double explorationConstant) : base(seed, iterations, explorationConstant)
        {
        }
    }
}
