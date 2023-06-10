using HexGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Helpers
{
    internal static class HexTypeHelper
    {
        public static PlayerEnum StateToPlayer(HexStateEnum hexState)
        {
            switch (hexState)
            {
                case HexStateEnum.Red: return PlayerEnum.Red;
                case HexStateEnum.Blue: return PlayerEnum.Blue;
            }

            throw new ArgumentException();
        }

        public static HexStateEnum PlayerToStata(PlayerEnum player)
        {
            switch (player)
            {
                case PlayerEnum.Red: return HexStateEnum.Red;
                case PlayerEnum.Blue: return HexStateEnum.Blue;
            }

            throw new ArgumentException();
        }
        public static PlayerEnum Not(PlayerEnum player)
        {
            switch (player)
            {
                case PlayerEnum.Red: return PlayerEnum.Blue;
                case PlayerEnum.Blue: return PlayerEnum.Red;
            }

            throw new ArgumentException();
        }

        public static HexStateEnum Not(HexStateEnum hexState)
        {
            switch (hexState)
            {
                case HexStateEnum.Red: return HexStateEnum.Blue;
                case HexStateEnum.Blue: return HexStateEnum.Red;
            }

            throw new ArgumentException();
        }
    }
}
