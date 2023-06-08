namespace HexGame.Models
{
    internal readonly struct GameField
    {
        public int Row { get; init; }
        public int Column { get; init; }

        public GameField(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
