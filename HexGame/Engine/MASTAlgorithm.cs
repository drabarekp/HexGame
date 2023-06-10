using HexGame.Engine.Nodes;
using HexGame.Enums;
using HexGame.Helpers;
using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace HexGame.Engine
{
    internal class MASTAlgorithm : BasicMCTSAlgorithm
    {
        public override string AlgorithmName() => AlgorithmTypeEnum.MAST.ToString();
        (double, double)[,] redEvaluations;
        (double, double)[,] blueEvaluations;

        public MASTAlgorithm(int seed, int iterations, double explorationConstant) : base(seed, iterations, explorationConstant)
        {
            redEvaluations = new (double, double)[GameState.Size, GameState.Size];
            blueEvaluations = new (double, double)[GameState.Size, GameState.Size];
        }
        public MASTAlgorithm(int seed, int iterations) : base(seed, iterations)
        {
            redEvaluations = new (double, double)[GameState.Size, GameState.Size];
            blueEvaluations = new (double, double)[GameState.Size, GameState.Size];
        }

        public new IAlgorithm Copy(int seed) => new MASTAlgorithm(seed, Iterations, ExplorationConstant);

        protected override double Simulation(GameState state)
        {
            GameState currentState = (GameState)state.Clone();

            while (!currentState.IsTerminal())
            {
                List<GameMove> possibleMoves = currentState.GetPossibleMoves();

                double[] evals = new double[possibleMoves.Count];
                for(int i = 0; i < evals.Length; i++)
                {
                    if (currentState.CurrentMove == HexStateEnum.Red)
                        evals[i] = ExpMi( redEvaluations[possibleMoves[i].Row, possibleMoves[i].Column] );
                    else
                        evals[i] = ExpMi( blueEvaluations[possibleMoves[i].Row, possibleMoves[i].Column] );
                }
                int moveIndex = Roulette(evals);
                GameMove chosenMove = possibleMoves[moveIndex];
                currentState = currentState.GetNextState(chosenMove);
            }

            return currentState.GetEndScore(Player);
        }

        protected override void Backpropagation(INode? node, double result)
        {
            PlayerEnum whoWon = result == 1.0 ? Player : HexTypeHelper.Not(Player);

            while (node != null)
            {
                node.Visits++;
                node.Wins += result;

                var movedLast = HexTypeHelper.Not(node.State.CurrentMove);
                if(movedLast == HexStateEnum.Red)
                {
                    (double q, double n) currentStats = redEvaluations[node.State.LastMove.Row, node.State.LastMove.Column];
                    redEvaluations[node.State.LastMove.Row, node.State.LastMove.Column] = 
                        (whoWon == PlayerEnum.Red ? currentStats.q + 1.0 : currentStats.q, currentStats.n + 1.0);
                }
                else
                {
                    (double q, double n) currentStats = blueEvaluations[node.State.LastMove.Row, node.State.LastMove.Column];
                    blueEvaluations[node.State.LastMove.Row, node.State.LastMove.Column] = 
                        (whoWon == PlayerEnum.Blue ? currentStats.q + 1.0 : currentStats.q, currentStats.n + 1.0);
                }

                node = node.Parent;
            }
        }

        public int Roulette(double[] evals)
        {
            double sumOfProbability = 0.0;

            for(int i=0; i< evals.Length; i++)
            {
                sumOfProbability += evals[i];
            }

            double choice = sumOfProbability * Random.NextDouble();
            double currentlyOn = 0.0;

            for (int i = 0; i < evals.Length; i++)
            {
                currentlyOn += evals[i];

                if(currentlyOn >= choice)
                    return i;
            }

            throw new ArgumentException();
        }

        double ExpMi((double q, double n) a)
        {
            return ExpMi(a.q, a.n);
        }
        double ExpMi(double q /*reward*/, double n/*visitCount*/)
        {
            return Math.Exp(miFunction(q, n) * 5.0);
        }
        double miFunction((double q, double n) a)
        {
            return miFunction(a.q, a.n);
        }

        double miFunction(double q /*reward*/, double n/*visitCount*/)
        {
            if (n < 0) throw new ArgumentException();
            if (n == 0) return 1.0;

            return q / n;
        }


    }
}
