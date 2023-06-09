using HexGame.Enums;

namespace HexGame.Engine
{
    internal class MASTAlgorithm : BasicMCTSAlgorithm
    {
        public new string AlgorithmName => AlgorithmTypeEnum.MAST.ToString();

        public MASTAlgorithm(int seed, int iterations, double explorationConstant) : base(seed, iterations, explorationConstant)
        {
        }

        public MASTAlgorithm(int seed, int iterations) : base(seed, iterations)
        {
        }
    }
}
