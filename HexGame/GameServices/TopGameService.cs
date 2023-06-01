using HexGame.Models;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using SD = System.Drawing;
using System.Windows.Media;
using HexGame.Helpers;

namespace HexGame.GameServices
{
    internal class TopGameService
    {
        public GameState GameState { get; private set; }
        public SD.Rectangle[][] Positions { get; private set; }

        int numberOfRows;
        int MARGIN;
        int DIAMETER;
        int HALF_DIAMETER;

        public TopGameService(int viewWidth, int viewHeight)
        {
            GameState = new GameState();
            numberOfRows = GameState.Board.Length;

            MARGIN = 10;
            DIAMETER = viewHeight / (numberOfRows) - (int)(2 * MARGIN);
            HALF_DIAMETER = (int)(DIAMETER / 2);

            Positions = CreateGameFields();
        }

        public void NewGame()
        {
            GameState = new GameState();
        }

        private SD.Rectangle[][] CreateGameFields()
        {

            var positions = new SD.Rectangle[numberOfRows][];
            for (int i = 0; i < numberOfRows; i++)
            {
                positions[i] = new SD.Rectangle[numberOfRows];
            }

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    positions[i][j] = new SD.Rectangle(MARGIN + j * (DIAMETER + MARGIN) + i * HALF_DIAMETER, MARGIN + i * (DIAMETER + MARGIN), DIAMETER, DIAMETER);
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
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    if(Positions[i][j].Contains(new Point(x, y)))
                    {
                        GameState.PlayerMove(i, j);
                        break;
                    }
                }
            }
        }

        
    }
}
