using System.Drawing;
using System.Drawing.Printing;

namespace TileArchitect
{
    class Tile
    {
        public TilePoint[,] Points;
        readonly Form1 _form;
        public Image Image;
        public string Name;

        public Tile(Form1 form)
        {
            this._form = form;
            Points = new TilePoint[5, 5];
        }

        public Tile(string name, Form1 form)
        {
            this._form = form;
            Name = name;
            Points = new TilePoint[5, 5];
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Points[x, y] = new TilePoint(x, y, form);
                    Points[x, y].BuildButton();
                }
            }
        }

        public Tile(Tile tile)
        {
            this._form = tile._form;
            this.Name = tile.Name;
            this.Points = tile.Points;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Points[x, y].BuildButton();
                }
            }
        }

        public void Load(string tile)
        {
            string[] lines = tile.Split('\n');
            this.Name = lines[0];
            for (int y = 0; y < 5; y++)
            {
                string line = lines[y + 1];
                string[] statuses = line.Split(' ');
                for (int x = 0; x < 5; x++)
                {
                    Points[x, y] = new TilePoint(x, y, _form);
                    Points[x, y].Status = statuses[x] == "1" ? true : false;
                }
            }
        }

        public void Destroy()
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Points[x, y].Destroy();
                }
            }
        }


        public string Save()
        {
            string saved = "";
            saved += Name + "\n";
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    saved += (Points[x, y].Status ? "1" : "0") + " ";
                }
                saved += "\n";
            }

            return saved;
        }

        public Image ToImage()
        {
            Bitmap temp = new Bitmap(Settings.ImagePointSize * 5, Settings.ImagePointSize * 5);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    TilePoint point = Points[x, y];
                    for (int py = 0; py < Settings.ImagePointSize; py++)
                    {
                        for (int px = 0; px < Settings.ImagePointSize; px++)
                        {
                            temp.SetPixel(x * Settings.ImagePointSize + px, y * Settings.ImagePointSize + py,
                                point.Status ? Color.Black : Color.White);
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
                    TilePoint point = Points[x, y];
                    float offset = ((float) Settings.SizeOnPage) / 5;
                    ev.Graphics.DrawRectangle(new Pen(Color.Black), xpos + x * offset, ypos + y * offset, offset,
                        offset);
                    if (point.Status)
                    {
                        ev.Graphics.FillRectangle(Brushes.Gray, xpos + x * offset, ypos + y * offset, offset, offset);
                    }
                }
            }
        }
    }
}