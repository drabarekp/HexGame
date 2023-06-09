using HexGame.Enums;

namespace HexGame.Engine
{
    internal class RAVEAlgorithm : BasicMCTSAlgorithm
    {
        public new string AlgorithmName => AlgorithmTypeEnum.RAVE.ToString();

        public RAVEAlgorithm(int seed, int iterations, double explorationConstant) : base(seed, iterations, explorationConstant)
        {
        }

        public RAVEAlgorithm(int seed, int iterations) : base(seed, iterations)
        {
        }
    }
}
