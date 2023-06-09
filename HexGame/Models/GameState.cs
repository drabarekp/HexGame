using HexGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexGame.Models
{
    internal class GameState : ICloneable
    {
        const int Size = 11;
        public HexStateEnum[][] Board { get; init; }
        public HexStateEnum CurrentMove { get; private set; }
        public GameMove LastMove { get; private set; }

        public GameState()
        {
            Board = new HexStateEnum[Size][];
            for (int i = 0; i < Size; i++)
            {
                Board[i] = new HexStateEnum[Size];
            }

            InitializeBoard();
            CurrentMove = HexStateEnum.Red;
        }

        public object Clone() => new GameState(Board, CurrentMove, LastMove);

        public double GetEndScore(PlayerEnum player)
        {
            var result = GetGameResult();

            if (player == PlayerEnum.Red)
            {
                return result == GameResultEnum.RedVictory ? 1 : 0;
            }

            if (player == PlayerEnum.Blue)
            {
                return result == GameResultEnum.BlueVictory ? 1 : 0;
            }

            throw new InvalidOperationException();
        }

        public GameState GetNextState(GameMove move)
        {
            var newState = (GameState)Clone();
            newState.PerformMove(move);
            return newState;
        }

        public List<GameMove> GetPossibleMoves()
        {
            var possibleMoves = new List<GameMove>();

            if (GetGameResult() != GameResultEnum.InconclusiveYet)
                return possibleMoves;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Board[i][j] == HexStateEnum.None)
                    {
                        possibleMoves.Add(new GameMove(i, j));
                    }
                }
            }

            return possibleMoves;
        }

        public void PerformMove(GameMove move)
        {
            if (Board[move.Row][move.Column] != HexStateEnum.None) return;

            Board[move.Row][move.Column] = CurrentMove;

            if (CurrentMove == HexStateEnum.Red)
            {
                CurrentMove = HexStateEnum.Blue;
            }
            else
            {
                CurrentMove = HexStateEnum.Red;
            }

            LastMove = move;
        }

        public static GameField[] GetNeighbours(int row, int column)
        {
            var neighbours = new List<GameField>
            {
                new GameField(row - 1, column),
                new GameField(row - 1, column + 1),
                new GameField(row, column - 1),
                new GameField(row, column + 1),
                new GameField(row + 1, column - 1),
                new GameField(row + 1, column)
            };

            return neighbours.Where(n => n.Column >= 0 && n.Column < Size && n.Row >= 0 && n.Row < Size).ToArray();
        }

        public GameResultEnum GetGameResult()
        {
            {
                // checking red victory
                bool[][] redVisited = new bool[Size][];
                for (int i = 0; i < Size; i++) redVisited[i] = new bool[Size];

                var redStack = new Stack<GameField>();
                for (int i = 0; i < Size; i++)
                {
                    if (Board[0][i] == HexStateEnum.Red)
                    {
                        redStack.Push(new GameField(0, i));
                        redVisited[0][i] = true;
                    }
                }

                while (redStack.Count > 0)
                {
                    GameField currentField = redStack.Pop();
                    var neighbours = GetNeighbours(currentField.Row, currentField.Column);
                    foreach (var neighbouringField in neighbours)
                    {
                        // if red and not visited yet
                        if (GetColor(neighbouringField) == HexStateEnum.Red && !redVisited[neighbouringField.Row][neighbouringField.Column])
                        {
                            if (neighbouringField.Row == Size - 1) return GameResultEnum.RedVictory;

                            redStack.Push(neighbouringField);
                            redVisited[neighbouringField.Row][neighbouringField.Column] = true;
                        }
                    }
                }
            }
            {
                // checking blue victory
                bool[][] blueVisited = new bool[Size][];
                for (int i = 0; i < Size; i++) blueVisited[i] = new bool[Size];

                var blueStack = new Stack<GameField>();
                for (int i = 0; i < Size; i++)
                {
                    if (Board[i][0] == HexStateEnum.Blue)
                    {
                        blueStack.Push(new GameField(i, 0));
                        blueVisited[i][0] = true;
                    }
                }

                while (blueStack.Count > 0)
                {
                    GameField currentField = blueStack.Pop();
                    var neighbours = GetNeighbours(currentField.Row, currentField.Column);
                    foreach (var neighbouringField in neighbours)
                    {
                        // if blue and not visited yet
                        if (GetColor(neighbouringField) == HexStateEnum.Blue && !blueVisited[neighbouringField.Row][neighbouringField.Column])
                        {
                            if (neighbouringField.Column == Size - 1) return GameResultEnum.BlueVictory;

                            blueStack.Push(neighbouringField);
                            blueVisited[neighbouringField.Row][neighbouringField.Column] = true;
                        }
                    }
                }
            }

            return GameResultEnum.InconclusiveYet;
        }

        public bool IsTerminal() => GetGameResult() != GameResultEnum.InconclusiveYet;

        private HexStateEnum GetColor(GameField field)
        {
            return Board[field.Row][field.Column];
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Board[i][j] = HexStateEnum.None;
                }
            }
        }

        private GameState(HexStateEnum[][] board, HexStateEnum currentMove, GameMove lastMove)
        {
            Board = new HexStateEnum[Size][];
            for (int i = 0; i < Size; i++)
            {
                Board[i] = new HexStateEnum[Size];
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Board[i][j] = board[i][j];
                }
            }

            CurrentMove = currentMove;
            LastMove = new(lastMove.Row, lastMove.Column);
        }
    }
}
