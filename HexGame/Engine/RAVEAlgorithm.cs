using HexGame.Enums;

namespace HexGame.Engine
{
    internal class RAVEAlgorithm : BasicMCTSAlgorithm
    {
        private readonly double RaveValue;
        private readonly double Beta;

        public override string AlgorithmName() => AlgorithmTypeEnum.RAVE.ToString();


        public RAVEAlgorithm(int seed, int iterations, double explorationConstant, double raveValue = 0, double beta = 0.5) : base(seed, iterations, explorationConstant)
        {
            RaveValue = raveValue;
            Beta = beta;
        }

        public RAVEAlgorithm(int seed, int iterations, double raveValue = 0, double beta = 0.5) : base(seed, iterations)
        {
            RaveValue = raveValue;
            Beta = beta;
        }

        public new IAlgorithm Copy(int seed) => new RAVEAlgorithm(seed, Iterations, ExplorationConstant, RaveValue, Beta);

        protected override void Backpropagation(Node? node, double result)
        {
            while (node != null)
            {
                node.Visits++;
                node.Wins += result;

                if (node.Parent != null)
                {
                    if (node.Parent.RaveWins.ContainsKey(node.State.LastMove))
                        node.Parent.RaveWins[node.State.LastMove] += result;
                    else
                        node.Parent.RaveWins[node.State.LastMove] = result;
                }

                node = node.Parent;
            }
        }

        protected override Node BestChild(Node node)
        {
            Node? bestChild = null;
            double bestValue = double.NegativeInfinity;

            foreach (Node child in node.Children)
            {
                double value = child.GetRAVEValue(node.State.LastMove, RaveValue, Beta);

                if (value > bestValue)
                {
                    bestValue = value;
                    bestChild = child;
                }
            }

            return bestChild!;
        }

    }
}
