using HexGame.Enums;
using HexGame.Models;

namespace HexGame.Engine
{
    internal interface IAlgorithm
    {
        public string AlgorithmName();

        public GameMove CalculateNextMove(GameState state, PlayerEnum player);

        public IAlgorithm Copy(int seed);
    }
}