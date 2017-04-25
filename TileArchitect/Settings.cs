using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileArchitect
{
    public static class Settings
    {
        public static int Size = 32;
        public static int ImagePointSize = 24;
        public static int ImageSize = ImagePointSize * 5;
        public static int SizeOnPage = 256;
        public static Point offset = new Point(32, 32);
    }
}
