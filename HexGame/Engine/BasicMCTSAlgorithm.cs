using HexGame.Enums;
using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexGame.Engine
{
    internal class BasicMCTSAlgorithm : IAlgorithm
    {
        protected readonly int Seed;
        protected readonly Random Random;
        protected readonly int Iterations;
        protected readonly double ExplorationConstant;

        protected Node? root;
        protected PlayerEnum Player;

        public virtual string AlgorithmName() => AlgorithmTypeEnum.BasicMCTS.ToString();

        public BasicMCTSAlgorithm(int seed, int iterations, double explorationConstant = 1.41421356237)
        {
            Seed = seed;
            Random = new Random(Seed);
            Iterations = iterations;
            ExplorationConstant = explorationConstant;
        }

        public GameMove CalculateNextMove(GameState state, PlayerEnum player)
        {
            root = new Node((GameState)state.Clone());
            Player = player;

            for (int i = 0; i < Iterations; i++)
            {
                Node node = Selection();
                double result = Simulation(node.State);
                Backpropagation(node, result);
            }

            return BestChild(root).State.LastMove;
        }

        public IAlgorithm Copy(int seed) => new BasicMCTSAlgorithm(seed, Iterations, ExplorationConstant);

        private Node Selection()
        {
            Node? node = root;

            while (!node!.State.IsTerminal())
            {
                if (!node.IsFullyExpanded())
                {
                    return Expansion(node);
                }

                node = node.SelectChild(ExplorationConstant);
            }

            return node;
        }

        private Node Expansion(Node node)
        {
            List<GameMove> untriedMoves = node.State.GetPossibleMoves().Except(node.Children.Select(c => c.State.LastMove)).ToList();
            var newMove = untriedMoves[Random.Next(untriedMoves.Count)];
            var newState = node.State.GetNextState(newMove);

            return node.AddChild(newState);
        }

        private double Simulation(GameState state)
        {
            GameState currentState = (GameState)state.Clone();

            while (!currentState.IsTerminal())
            {
                List<GameMove> possibleMoves = currentState.GetPossibleMoves();
                GameMove randomMove = possibleMoves[Random.Next(possibleMoves.Count)];
                currentState = currentState.GetNextState(randomMove);
            }

            return currentState.GetEndScore(Player);
        }

        private void Backpropagation(Node? node, double result)
        {
            while (node != null)
            {
                node.Visits++;
                node.Wins += result;
                node = node.Parent;
            }
        }

        private Node BestChild(Node node)
        {
            if (node.Children.Count == 0)
                return node;

            return node.Children.OrderByDescending(child => child.Visits).First();
        }
    }
}
