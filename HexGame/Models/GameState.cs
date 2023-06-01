using HexGame.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HexGame.Models
{
    internal class GameState
    {
        const int SIZE = 11;
        public HexStateEnum[][] Board { get; init; }

        public GameState()
        {
            Board = new HexStateEnum[SIZE][];
            for(int i = 0; i< SIZE; i++)
            {
                Board[i] = new HexStateEnum[SIZE];
            }

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Board[i][j] = HexStateEnum.None;
                }
            }
        }

        public void PlayerRedMove(int row, int column)
        {
            Board[row][column] = HexStateEnum.Red;
        }

        private HexStateEnum GetColor(GameField field)
        {
            return Board[field.Row][field.Column];
        }

        public GameField[] GetNeighbours(int row, int column)
        {
            List<GameField> neighbours = new List<GameField>();

            neighbours.Add(new GameField(row - 1, column));
            neighbours.Add(new GameField(row - 1, column + 1));
            neighbours.Add(new GameField(row, column - 1));
            neighbours.Add(new GameField(row, column + 1));
            neighbours.Add(new GameField(row + 1, column - 1));
            neighbours.Add(new GameField(row + 1, column));

            return neighbours.Where(n => n.Column >= 0 && n.Column < SIZE && n.Row >= 0 && n.Row < SIZE).ToArray();
        }

        public GameResultEnum GetGameResult()
        {
            {
                // checking red victory
                bool[][] redVisited = new bool[SIZE][];
                for (int i = 0; i < SIZE; i++) redVisited[i] = new bool[SIZE];

                var redStack = new Stack<GameField>();
                for (int i = 0; i < SIZE; i++)
                {
                    redStack.Push(new GameField(0, i));
                    redVisited[0][i] = true;
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
                            if (neighbouringField.Row == SIZE - 1) return GameResultEnum.RedVictory;

                            redStack.Push(neighbouringField);
                            redVisited[neighbouringField.Row][neighbouringField.Column] = true;
                        }
                    }
                }
            }
            {
                // checking blue victory
                bool[][] blueVisited = new bool[SIZE][];
                for (int i = 0; i < SIZE; i++) blueVisited[i] = new bool[SIZE];

                var blueStack = new Stack<GameField>();
                for (int i = 0; i < SIZE; i++)
                {
                    blueStack.Push(new GameField(i, 0));
                    blueVisited[i][0] = true;
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
                            if (neighbouringField.Column == SIZE - 1) return GameResultEnum.BlueVictory;

                            blueStack.Push(neighbouringField);
                            blueVisited[neighbouringField.Row][neighbouringField.Column] = true;
                        }
                    }
                }
            }

            return GameResultEnum.InconclusiveYet;
        }
    }
}
