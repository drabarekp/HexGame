using HexGame.Engine.Nodes;
using HexGame.Enums;
using HexGame.Models;

namespace HexGame.Engine
{
    internal class RAVEAlgorithm : BasicMCTSAlgorithm
    {
        private readonly double Beta;

        public override string AlgorithmName() => AlgorithmTypeEnum.RAVE.ToString();


        public RAVEAlgorithm(int seed, int iterations, double explorationConstant, double beta = 0.5) : base(seed, iterations, explorationConstant)
        {
            Beta = beta;
        }

        public RAVEAlgorithm(int seed, int iterations, double beta = 0.5) : base(seed, iterations)
        {
            Beta = beta;
        }

        public new IAlgorithm Copy(int seed) => new RAVEAlgorithm(seed, Iterations, ExplorationConstant, Beta);

        public override INode CreateRoot(GameState state) => new RaveNode((GameState)state.Clone());

        protected override void Backpropagation(INode? node, double result)
        {
            var rNode = (RaveNode?)node;

            while (rNode != null)
            {
                rNode.Visits++;
                rNode.Wins += result;

                var rNodeParent = (RaveNode?)rNode.Parent;

                if (rNodeParent != null)
                {

                    if (rNodeParent.RaveWins.ContainsKey(rNode.State.LastMove))
                        rNodeParent.RaveWins[rNode.State.LastMove] += result;
                    else
                        rNodeParent.RaveWins[rNode.State.LastMove] = result;
                }

                rNode = rNodeParent;
            }
        }

        protected override INode BestChild(INode node)
        {
            INode? bestChild = null;
            double bestValue = double.NegativeInfinity;

            foreach (INode child in node.Children)
            {
                double value = child.Q(ExplorationConstant);

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
