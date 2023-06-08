namespace HexGame.Models
{
    internal struct GameMove
    {
        public int Row;
        public int Column;

        public GameMove(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
