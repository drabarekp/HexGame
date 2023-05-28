using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Helpers
{
    internal static class VisualHelper
    {
        public static int Distance(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }
}
