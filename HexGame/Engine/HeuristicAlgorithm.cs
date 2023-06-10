using HexGame.Engine.Nodes;
using HexGame.Enums;
using HexGame.Models;
using System;
using System.Xml.Linq;

namespace HexGame.Engine
{
    internal class HeuristicAlgorithm : BasicMCTSAlgorithm
    {
        public override string AlgorithmName() => AlgorithmTypeEnum.Heuristic.ToString();

        public HeuristicAlgorithm(int seed, int iterations) : base(seed, iterations)
        {

        }

        public override GameMove CalculateNextMove(GameState state, PlayerEnum player)
        {
            var root = CreateRoot(state);
            var children = root.State.GetPossibleMoves();

            double currentHeuristic = double.MinValue;
            GameMove currentMove = new GameMove();

            foreach(var c in children)
            {

                var newState = state.GetNextState(c);
                double heuristic = CalculateHeuristic(newState, player);

                if(heuristic > currentHeuristic)
                {
                    currentHeuristic = heuristic;
                    currentMove = c;
                }

            }

            return currentMove;
        }

        private double CalculateHeuristic(GameState state, PlayerEnum player)
        {
            HexNetworkGraph redGraph = new HexNetworkGraph(state, HexStateEnum.Red);
            HexNetworkGraph blueGraph = new HexNetworkGraph(state, HexStateEnum.Blue);
            double redLength = redGraph.FindShortestPath();
            double blueLength = blueGraph.FindShortestPath();
            double heuristic = 0.0;

            if (player == PlayerEnum.Red)
            {
                heuristic = blueLength - redLength;
            }
            else
            {
                heuristic = redLength - blueLength;
            }

            return heuristic;
        }
    }
}
