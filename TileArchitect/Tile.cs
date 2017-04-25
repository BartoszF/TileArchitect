using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileArchitect
{
    class Tile
    {
        public static readonly int Size = 32;
        public static readonly int ImagePointSize = 24;
        public static readonly int ImageSize = ImagePointSize * 5;
        public static readonly int SizeOnPage = 256;
        public static readonly Point offset = new Point(32, 32);
        public TilePoint[,] points;
        Form1 form;
        public Image image;
        public string name;

        public Tile(Form1 form)
        {
            this.form = form;
            points = new TilePoint[5, 5];
        }
        public Tile(string name, Form1 form)
        {
            this.form = form;
            this.name = name;
            points = new TilePoint[5, 5];
            for(int y = 0;y<5;y++)
            {
                for(int x=0;x<5;x++)
                {
                    points[x, y] = new TilePoint(x,y, form);
                    points[x, y].BuildButton();
                }
            }
        }

        public Tile(Tile tile)
        {
            this.form = tile.form;
            this.name = tile.name;
            this.points = tile.points;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    points[x, y].BuildButton();
                }
            }
        }

        public void Load(string tile)
        {
            string[] lines = tile.Split('\n');
            this.name = lines[0];
            for(int y=0;y<5;y++)
            {
                string line = lines[y + 1];
                string[] statuses = line.Split(' ');
                for(int x=0;x<5;x++)
                {
                    points[x, y] = new TilePoint(x, y, form);
                    points[x, y].status = statuses[x] == "1" ? true : false;
                }
            }
        }

        public void Destroy()
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    points[x, y].Destroy();
                }
            }
        }
       

        public string Save()
        {
            string saved = "";
            saved += name + "\n";
            for(int y =0;y<5;y++)
            {
                for(int x=0;x<5;x++)
                {
                    saved += (points[x, y].status ? "1" : "0") + " ";
                }
                saved += "\n";
            }

            return saved;
        }

        public Image toImage()
        {
            Bitmap temp = new Bitmap(Tile.ImagePointSize * 5, Tile.ImagePointSize * 5);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    TilePoint point = points[x, y];
                    for(int py=0;py<Tile.ImagePointSize;py++)
                    {
                        for (int px = 0; px < Tile.ImagePointSize; px++)
                        {
                            temp.SetPixel(x* Tile.ImagePointSize + px, y* Tile.ImagePointSize + py, point.status ? Color.Black : Color.White);
                        }
                    }
                }
            }
            return temp;
        }

        public void Print(PrintPageEventArgs ev, float xpos, float ypos)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    TilePoint point = points[x, y];
                    float offset = ((float)Tile.SizeOnPage) / 5;
                    ev.Graphics.DrawRectangle(new Pen(Color.Black), xpos + x * offset, ypos+ y * offset, offset, offset);
                    if (point.status)
                    {
                        ev.Graphics.FillRectangle(Brushes.Gray, xpos+x * offset, ypos+y * offset, offset, offset);
                    }
                }
            }
        }
    }
}
