using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Engine
{
    internal class MASTAlgorithm : BasicMCTSAlgorithm
    {
        public MASTAlgorithm(int seed, int iterations, double explorationConstant) : base(seed, iterations, explorationConstant)
        {
        }
    }
}
