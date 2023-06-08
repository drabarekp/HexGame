using HexGame.Engine;
using HexGame.Models;
using System;
using System.Drawing;
using HexGame.Enums;

namespace HexGame.GameServices
{
    internal class TopGameService
    {
        public GameState GameState { get; private set; }
        public Rectangle[][] Positions { get; private set; }

        public int NumberOfRows;
        public int Margin;
        public int Diameter;
        public int HalfDiameter => Diameter / 2;

        private readonly IAlgorithm Algorithm;

        public TopGameService(int viewWidth, int viewHeight)
        {
            GameState = new GameState();
            NumberOfRows = GameState.Board.Length;

            Margin = 10;
            Diameter = viewHeight / (NumberOfRows) - (2 * Margin);

            Positions = CreateGameFields();

            Algorithm = new BasicMCTS(100, 3000, Math.Sqrt(2));
        }

        public void NewGame()
        {
            GameState = new GameState();
        }

        private Rectangle[][] CreateGameFields()
        {

            var positions = new Rectangle[NumberOfRows][];
            for (int i = 0; i < NumberOfRows; i++)
            {
                positions[i] = new Rectangle[NumberOfRows];
            }

            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    positions[i][j] = new Rectangle(Margin + j * (Diameter + Margin) + i * HalfDiameter, Margin + i * (Diameter + Margin), Diameter, Diameter);
                }
            }

            return positions;
        }

        public void DrawGameFields(Graphics g, Rectangle canvas)
        {
            var result = GameState.GetGameResult();
            switch (result)
            {
                case Enums.GameResultEnum.RedVictory: g.FillRectangle(Brushes.IndianRed, canvas); break;
                case Enums.GameResultEnum.BlueVictory: g.FillRectangle(Brushes.RoyalBlue, canvas); break;
                case Enums.GameResultEnum.InconclusiveYet: g.FillRectangle(Brushes.White, canvas); break;
            }

            var pen = new Pen(Color.Black, 8);
            var brushPlayer1 = new SolidBrush(Color.IndianRed);
            var brushPlayer2 = new SolidBrush(Color.RoyalBlue);
            var brushPlayerNone = new SolidBrush(Color.LightGray);
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
                        return;
                    }
                }
            }
        }

        public void PerformAIMove()
        {
            var botMove = Algorithm.CalculateNextMove(GameState, PlayerEnum.Blue);
            GameState.PerformMove(botMove);
        }


    }
}
