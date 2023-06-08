using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HexGame.Engine
{
    internal class BasicMCTS
    {
        private static readonly Random Random = new();
        private static readonly double ExplorationConstant = Math.Sqrt(2);

        public static GameMove CalculateNextMove(GameState initialState, int iterations)
        {
            Node rootNode = new(initialState);

            for (int i = 0; i < iterations; i++)
            {
                Node node = SelectNode(rootNode);
                double score = Simulate(node.State);
                Backpropagate(node, score);
            }

            Node bestChild = rootNode.Children.OrderByDescending(c => c.Visits).First();
            return bestChild.State.LastMove;
        }

        private static Node SelectNode(Node rootNode)
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

        private static double Simulate(GameState state)
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

        private static void Backpropagate(Node? node, double score)
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
