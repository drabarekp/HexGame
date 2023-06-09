using HexGame.Engine;
using HexGame.Enums;
using HexGame.Models;
using System;
using System.Drawing;
using System.Windows;

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

        private IAlgorithm Algorithm = new BasicMCTSAlgorithm(100, 1000, Math.Sqrt(2));
        private bool IsPlayerStart = true;
        private bool MessageShown = false;

        public TopGameService(int viewWidth, int viewHeight)
        {
            GameState = new GameState();
            NumberOfRows = GameState.Board.Length;

            Margin = 10;
            Diameter = viewHeight / (NumberOfRows) - (2 * Margin);

            Positions = CreateGameFields();
        }
        public void NewGame(int iterations, bool isPlayerStart, AlgorithmTypeEnum algorithmType, int seed)
        {
            IsPlayerStart = isPlayerStart;
            MessageShown = false;

            switch (algorithmType)
            {
                case AlgorithmTypeEnum.BasicMCTS:
                    Algorithm = new BasicMCTSAlgorithm(seed, iterations);
                    break;

                case AlgorithmTypeEnum.RAVE:
                    Algorithm = new RAVEAlgorithm(seed, iterations);
                    break;

                case AlgorithmTypeEnum.MAST:
                    Algorithm = new MASTAlgorithm(seed, iterations);
                    break;

                case AlgorithmTypeEnum.Heuristic:
                    Algorithm = new HeuristicAlgorithm();
                    break;
            }

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

            const string VictoryMessage = "Gratulacje, wygrałeś z AI";
            const string DefeatMessage = "Niestety przegrałeś z AI";
            const string Caption = "Koniec gry";

            switch (result)
            {
                case GameResultEnum.RedVictory:
                    g.FillRectangle(Brushes.IndianRed, canvas);
                    if (!MessageShown)
                    {
                        if (IsPlayerStart)
                            MessageBox.Show(VictoryMessage, Caption, MessageBoxButton.OK);
                        else
                            MessageBox.Show(DefeatMessage, Caption, MessageBoxButton.OK);

                        MessageShown = true;
                    }
                    break;

                case GameResultEnum.BlueVictory:
                    g.FillRectangle(Brushes.RoyalBlue, canvas);
                    if (!MessageShown)
                    {
                        if (IsPlayerStart)
                            MessageBox.Show(DefeatMessage, Caption, MessageBoxButton.OK);
                        else
                            MessageBox.Show(VictoryMessage, Caption, MessageBoxButton.OK);

                        MessageShown = true;
                    }
                    break;

                case GameResultEnum.InconclusiveYet: g.FillRectangle(Brushes.White, canvas); break;
            }

            var pen = new Pen(Color.Black, 8);
            var brushPlayer1 = new SolidBrush(Color.IndianRed);
            var brushPlayer2 = new SolidBrush(Color.RoyalBlue);
            var brushPlayerNone = new SolidBrush(Color.LightGray);
            int numberOfRows = GameState.Board.Length;

            var TL = new System.Drawing.Point(5, 5);
            var TR = new System.Drawing.Point(canvas.Width / 2, 5);
            var BL = new System.Drawing.Point(200, canvas.Height - 100);
            var BR = new System.Drawing.Point(canvas.Width / 2 + 200, canvas.Height - 100);

            g.DrawLine(new Pen(brushPlayer1, 5), TL, TR);
            g.DrawLine(new Pen(brushPlayer1, 5), BL, BR);
            g.DrawLine(new Pen(brushPlayer2, 5), TL, BL);
            g.DrawLine(new Pen(brushPlayer2, 5), TR, BR);




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
            if (GameState.GetGameResult() != GameResultEnum.InconclusiveYet) return;

            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    if (Positions[i][j].Contains(new System.Drawing.Point(x, y)))
                    {
                        GameState.PerformMove(new GameMove(i, j));
                        return;
                    }
                }
            }
        }

        public void PerformAIMove()
        {
            if (GameState.GetGameResult() != GameResultEnum.InconclusiveYet) return;

            var botMove = Algorithm.CalculateNextMove(GameState, PlayerEnum.Blue);
            GameState.PerformMove(botMove);
        }


    }
}
