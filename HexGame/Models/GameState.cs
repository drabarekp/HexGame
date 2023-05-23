using HexGame.Enums;
using System;
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
    }
}
