using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HexGame.Engine
{
    internal class BasicMCTS : IAlgorithm
    {
        private Random Random = new();
        private int Iterations = 1000;
        private static readonly double ExplorationConstant = Math.Sqrt(2);

        public BasicMCTS(int seed, int iterations)
        {
            Random = new Random(seed);
            Iterations = iterations;
        }

        public GameMove CalculateNextMove(GameState state)
        {
            GameState initialState = (GameState)state.Clone();

            Node rootNode = new(initialState);

            for (int i = 0; i < Iterations; i++)
            {
                Node node = SelectNode(rootNode);
                double score = Simulate(node.State);
                Backpropagate(node, score);
            }

            Node bestChild = rootNode.Children.OrderByDescending(c => c.Visits).First();
            return bestChild.State.LastMove;
        }

        private Node SelectNode(Node rootNode)
        {
            Node? node = rootNode;

            while (!node!.State.IsTerminal() && node.IsFullyExpanded())
            {
                node = node.SelectChild(ExplorationConstant);
            }

            if (!node.State.IsTerminal())
            {
                List<GameMove> untriedMoves = node.State.GetPossibleMoves().Except(node.Children.Select(c => c.State.LastMove)).ToList();
                GameMove randomMove = untriedMoves[Random.Next(untriedMoves.Count)];
                GameState newState = node.State.GetNextState(randomMove);
                node = node.AddChild(newState);
            }

            return node;
        }

        private double Simulate(GameState state)
        {
            GameState currentState = (GameState)state.Clone();

            while (!currentState.IsTerminal())
            {
                List<GameMove> possibleMoves = currentState.GetPossibleMoves();
                GameMove randomMove = possibleMoves[Random.Next(possibleMoves.Count)];
                currentState = currentState.GetNextState(randomMove);
            }

            return currentState.GetScore();
        }

        private void Backpropagate(Node? node, double score)
        {
            while (node != null)
            {
                node.Visits++;
                node.Wins += score;
                node = node.Parent;
            }
        }
    }
}
