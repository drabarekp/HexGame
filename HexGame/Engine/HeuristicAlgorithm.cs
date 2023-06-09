using HexGame.Enums;
using HexGame.Models;
using System;

namespace HexGame.Engine
{
    internal class HeuristicAlgorithm : IAlgorithm
    {
        public string AlgorithmName => AlgorithmTypeEnum.Heuristic.ToString();

        public GameMove CalculateNextMove(GameState state, PlayerEnum player)
        {
            throw new NotImplementedException();
        }

        public IAlgorithm Copy() => new HeuristicAlgorithm();
    }
}
