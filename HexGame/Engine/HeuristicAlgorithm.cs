using HexGame.Engine.Nodes;
using HexGame.Enums;
using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace HexGame.Engine
{
    internal class HeuristicAlgorithm : IAlgorithm
    {
        public readonly int Iterations;

        public HeuristicAlgorithm(int iterations)
        {
            Iterations = iterations;
        }

        public string AlgorithmName() => AlgorithmTypeEnum.Heuristic.ToString();

        public IAlgorithm Copy(int seed) => new HeuristicAlgorithm(Iterations);

        public GameMove CalculateNextMove(GameState state, PlayerEnum player)
        {
            var root = new HeuristicNode((GameState)state.Clone());

            Queue<HeuristicNode> nodes = new Queue<HeuristicNode>();
            nodes.Enqueue(root);

            //creating move tree
            for (int i = 0; i < Iterations; )
            {
                try
                {
                    var node = nodes.Dequeue();
                
                    foreach(var move in node.State.GetPossibleMoves())
                    {
                        HeuristicNode childNode = (HeuristicNode)node.AddChild(node.State.GetNextState(move));
                        nodes.Enqueue(childNode);
                        i++;
                    }
                }
                catch (Exception) { break; }
            }

            MinMaxRecursively(root);

            try
            {
                if (player == PlayerEnum.Red)
                {
                    var maxChild = root.Children.MaxBy(n => ((HeuristicNode)n).Heuristic);
                    return maxChild!.State.LastMove;
                }
                else if (player == PlayerEnum.Blue)
                {
                    var minChild = root.Children.MinBy(n => ((HeuristicNode)n).Heuristic);
                    return minChild!.State.LastMove;
                }
            }
            catch(Exception e)
            {
                if (root.State is not null)
                {
                    var moves = root.State.GetPossibleMoves();
                    if(moves.Count > 0)
                        return moves[0];

                    //error
                    return new GameMove(0, 0);
                }
            }

            throw new ArgumentException();
        }

        private void MinMaxRecursively(HeuristicNode root)
        {
            if(root.Children.Count == 0)
            {
                root.Heuristic = CalculateHeuristic(root.State);
            }
            else
            {
                if(root.State.CurrentMove == HexStateEnum.Red)
                {
                    foreach (var childNode in root.Children)
                    {
                        MinMaxRecursively((HeuristicNode)childNode);
                    }
                    double current = double.MinValue;
                    foreach (HeuristicNode childNode in root.Children)
                    {
                        if(childNode.Heuristic > current)
                        {
                            current = childNode.Heuristic;
                        }
                    }
                    root.Heuristic = current;
                }
                else if (root.State.CurrentMove == HexStateEnum.Blue)
                {
                    foreach (var childNode in root.Children)
                    {
                        MinMaxRecursively((HeuristicNode)childNode);
                    }
                    double current = double.MaxValue;
                    foreach (HeuristicNode childNode in root.Children)
                    {
                        if (childNode.Heuristic < current)
                        {
                            current = childNode.Heuristic;
                        }
                    }
                    root.Heuristic = current;
                }
                
            }
        }

        private double CalculateHeuristic(GameState state)
        {
            HexNetworkGraph redGraph = new HexNetworkGraph(state, HexStateEnum.Red);
            HexNetworkGraph blueGraph = new HexNetworkGraph(state, HexStateEnum.Blue);
            double redLength = redGraph.FindShortestPath();
            double blueLength = blueGraph.FindShortestPath();

            return blueLength - redLength; // + red good, - blue good
        }
    }
}
