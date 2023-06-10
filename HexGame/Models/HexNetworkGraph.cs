using HexGame.Enums;
using HexGame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Models
{
    internal class HexNetworkGraph
    {
        GameField[,][] connections;
        double[,][] distances;
        double[,] partialPathDistances;

        GameState gameState;
        HexStateEnum player;

        public HexNetworkGraph(GameState gameState, HexStateEnum player)
        {
            this.gameState = gameState;
            this.player = player;

            CreateConnectionsAndDistances(player);
            InitializeDistances(gameState, player);
            InitializePartialPathDistances(gameState, player);
        }

        private void CreateConnectionsAndDistances(HexStateEnum player)
        {
            connections = new GameField[GameState.Size, GameState.Size][];
            distances = new double[GameState.Size, GameState.Size][];

            for (int i = 0; i < GameState.Size; i++)
            {
                for (int j = 0; j < GameState.Size; j++)
                {
                    connections[i, j] = GameState.GetNeighbours(i, j);
                    distances[i, j] = new double[connections[i, j].Length];
                }
            }
        }

        private void InitializeDistances(GameState gameState, HexStateEnum player)
        {
            for (int i = 0; i < GameState.Size; i++)
            {
                for (int j = 0; j < GameState.Size; j++)
                {
                    for(int k = 0; k < connections[i, j].Length; k++)
                    {
                        HexStateEnum color1 = gameState.Board[i][j];
                        HexStateEnum color2 = gameState.Board[connections[i, j][k].Row][connections[i, j][k].Column];

                        if((color1 == color2 && color2 == player) || (color1 == HexStateEnum.None && color2 == player))
                        {
                            distances[i, j][k] = 0.0;
                        }
                        else if(color1 == HexTypeHelper.Not(player) || color2 == HexTypeHelper.Not(player))
                        {
                            distances[i, j][k] = double.PositiveInfinity;
                        }
                        else
                        {
                            distances[i, j][k] = 1.0;
                        }
                    }
                }
            }
        }

        private void InitializePartialPathDistances(GameState gameState, HexStateEnum player)
        {
            partialPathDistances = new double[GameState.Size, GameState.Size];
            for (int i = 0; i < partialPathDistances.GetLength(0); i++)
                for (int j = 0; j < partialPathDistances.GetLength(1); j++)
                    partialPathDistances[i, j] = double.MaxValue;

            if (player == HexStateEnum.Red)
            {
                for (int i = 0; i < partialPathDistances.GetLength(0); i++)
                {
                    if (gameState.Board[0][i] == HexStateEnum.Red)
                    {
                        partialPathDistances[0, i] = 0.0;
                    }
                    else if(gameState.Board[0][i] == HexStateEnum.None)
                    {
                        partialPathDistances[0, i] = 1.0;
                    }
                }
            }
            else if(player == HexStateEnum.Blue)
            {
                for (int i = 0; i < partialPathDistances.GetLength(1); i++)
                {
                    if (gameState.Board[i][0] == HexStateEnum.Blue)
                    {
                        partialPathDistances[i, 0] = 0.0;
                    }
                    else if (gameState.Board[i][0] == HexStateEnum.None)
                    {
                        partialPathDistances[i, 0] = 1.0;
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public double FindShortestPath()
        {
            for (int l = 0; l < GameState.Size * GameState.Size; l++)
            {
                for (int i = 0; i < GameState.Size; i++)
                {
                    for (int j = 0; j < GameState.Size; j++)
                    {
                        for (int k = 0; k < connections[i, j].Length; k++)
                        {
                            if(partialPathDistances[connections[i, j][k].Row, connections[i, j][k].Column] > 
                                partialPathDistances[i, j] + distances[i, j][k])
                            {
                                partialPathDistances[connections[i, j][k].Row, connections[i, j][k].Column] =
                                partialPathDistances[i, j] + distances[i, j][k];
                            }
                        }
                    }
                }
            }

            if (player == HexStateEnum.Red)
            {
                double len = double.MaxValue;
                for (int i = 0; i < partialPathDistances.GetLength(0); i++)
                {
                    double candidate = partialPathDistances[partialPathDistances.GetLength(0) - 1, i];
                    if (candidate < len)
                        len = candidate;
                }
                return len;
            }
            else if (player == HexStateEnum.Blue)
            {
                double len = double.MaxValue;
                for (int i = 0; i < partialPathDistances.GetLength(1); i++)
                {
                    double candidate = partialPathDistances[i, partialPathDistances.GetLength(1) - 1];
                    if (candidate < len)
                        len = candidate;
                }
                return len;
            }

            throw new Exception();
        }
    }
}
