using HexGame.Models;
using System;
using System.Collections.Generic;

namespace HexGame.Engine
{
    internal interface IAlgorithm
    {
        (int, int) CalculateNextMove(GameState state);
    }
}