using HexGame.Enums;
using HexGame.Models;

namespace HexGame.Engine
{
    internal interface IAlgorithm
    {
        string AlgorithmName { get; }
        GameMove CalculateNextMove(GameState state, PlayerEnum player);

        IAlgorithm Copy(int seed);
    }
}