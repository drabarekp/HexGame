using HexGame.Models;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HexGame.GameServices
{
    internal class TopGameService
    {
        public GameState GameState { get; private set; }

        public TopGameService()
        {
            GameState = new GameState();
        }

        public void NewGame()
        {
            GameState = new GameState();
        }
    }
}
