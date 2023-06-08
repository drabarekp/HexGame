using HexGame.Models;
using System.Drawing;

using SD = System.Drawing;

namespace HexGame.GameServices
{
    internal class TopGameService
    {
        public GameState GameState { get; private set; }
        public SD.Rectangle[][] Positions { get; private set; }

        public int NumberOfRows;
        public int Margin;
        public int Diameter;
        public int HalfDiameter => Diameter / 2;

        public TopGameService(int viewWidth, int viewHeight)
        {
            GameState = new GameState();
            NumberOfRows = GameState.Board.Length;

            Margin = 10;
            Diameter = viewHeight / (NumberOfRows) - (int)(2 * Margin);

            Positions = CreateGameFields();
        }

        public void NewGame()
        {
            GameState = new GameState();
        }

        private SD.Rectangle[][] CreateGameFields()
        {

            var positions = new SD.Rectangle[NumberOfRows][];
            for (int i = 0; i < NumberOfRows; i++)
            {
                positions[i] = new SD.Rectangle[NumberOfRows];
            }

            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    positions[i][j] = new SD.Rectangle(Margin + j * (Diameter + Margin) + i * HalfDiameter, Margin + i * (Diameter + Margin), Diameter, Diameter);
                }
            }

            return positions;
        }

        public void DrawGameFields(Graphics g, SD.Rectangle canvas)
        {
            var result = GameState.GetGameResult();
            switch (result)
            {
                case Enums.GameResultEnum.RedVictory: g.FillRectangle(SD.Brushes.IndianRed, canvas); break;
                case Enums.GameResultEnum.BlueVictory: g.FillRectangle(SD.Brushes.RoyalBlue, canvas); break;
                case Enums.GameResultEnum.InconclusiveYet: g.FillRectangle(SD.Brushes.White, canvas); break;
            }

            var pen = new SD.Pen(SD.Color.Black, 8);
            var brushPlayer1 = new SD.SolidBrush(SD.Color.IndianRed);
            var brushPlayer2 = new SD.SolidBrush(SD.Color.RoyalBlue);
            var brushPlayerNone = new SD.SolidBrush(SD.Color.LightGray);
            int numberOfRows = GameState.Board.Length;


            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    g.DrawEllipse(pen, Positions[i][j]);

                    switch (GameState.Board[i][j])
                    {
                        case Enums.HexStateEnum.Red: g.FillEllipse(brushPlayer1, Positions[i][j]); break;
                        case Enums.HexStateEnum.Blue: g.FillEllipse(brushPlayer2, Positions[i][j]); break;
                        case Enums.HexStateEnum.None: g.FillEllipse(brushPlayerNone, Positions[i][j]); break;
                    }
                }
            }
        }

        public void Click(int x, int y)
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    if (Positions[i][j].Contains(new Point(x, y)))
                    {
                        GameState.PerformMove(new GameMove(i, j));
                        break;
                    }
                }
            }
        }


    }
}
