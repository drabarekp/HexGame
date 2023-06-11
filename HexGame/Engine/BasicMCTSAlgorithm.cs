using HexGame.Engine.Nodes;
using HexGame.Enums;
using HexGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace HexGame.Engine
{
    internal class BasicMCTSAlgorithm : IAlgorithm
    {
        protected readonly int Seed;
        protected readonly Random Random;
        protected readonly int Iterations;
        protected readonly double ExplorationConstant;

        protected INode? root;
        protected PlayerEnum Player;

        public virtual string AlgorithmName() => AlgorithmTypeEnum.BasicMCTS.ToString();

        public BasicMCTSAlgorithm(int seed, int iterations, double explorationConstant = 1.41421356237)
        {
            Seed = seed;
            Random = new Random(Seed);
            Iterations = iterations;
            ExplorationConstant = explorationConstant;
        }

        public virtual GameMove CalculateNextMove(GameState state, PlayerEnum player)
        {
            root = CreateRoot(state);
            Player = player;

            for (int i = 0; i < Iterations; i++)
            {
                INode node = Selection();
                double result = Simulation(node.State);
                Backpropagation(node, result);
            }

            return BestChild(root).State.LastMove;
        }

        public virtual INode CreateRoot(GameState state) => new Node((GameState)state.Clone());

        public IAlgorithm Copy(int seed) => new BasicMCTSAlgorithm(seed, Iterations, ExplorationConstant);

        private INode Selection()
        {
            INode? node = root;

            while (!node!.State.IsTerminal())
            {
                if (!node.IsFullyExpanded())
                {
                    return Expansion(node);
                }

                node = SelectChild(node);
            }

            return node;
        }

        protected virtual INode Expansion(INode node)
        {
            List<GameMove> untriedMoves = node.State.GetPossibleMoves().Except(node.Children.Select(c => c.State.LastMove)).ToList();
            var newMove = untriedMoves[Random.Next(untriedMoves.Count)];
            var newState = node.State.GetNextState(newMove);

            return node.AddChild(newState);
        }

        protected virtual double Simulation(GameState state)
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

        protected virtual void Backpropagation(INode? node, double result)
        {
            while (node != null)
            {
                node.Visits++;
                node.Wins += result;
                node = node.Parent;
            }
        }

        protected virtual INode BestChild(INode node)
        {
            if (node.Children.Count == 0)
                return node;

            return node.Children.OrderByDescending(child => child.Visits).First();
        }

        public INode? SelectChild(INode node)
        {
            INode? bestChild = null;
            double bestScore = double.MinValue;

            foreach (var child in node.Children)
            {
                double score = child.Q(ExplorationConstant);
                
                if (score > bestScore)
                {
                    bestChild = child;
                    bestScore = score;
                }
            }

            return bestChild;
        }
    }
}
