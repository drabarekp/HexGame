using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Models
{
    internal struct GameField
    {
        public GameField(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public int Row { get; init; }
        public int Column { get; init; }
    }
}
