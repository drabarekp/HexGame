using HexGame.Models;

namespace HexGame.Engine
{
    internal interface IAlgorithm
    {
        GameMove CalculateNextMove(GameState state);
    }
}