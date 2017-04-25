using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileArchitect
{
    class TilePoint
    {
        public int X, Y;
        public bool status = false;
        public Button button;
        public Form1 form;

        public TilePoint(int X, int Y, Form1 form)
        {
            this.X = X;
            this.Y = Y;

            BuildButton();

            this.form = form;
        }

        public void BuildButton()
        {
            button = new Button();
            button.Width = Settings.Size;
            button.Height = Settings.Size;
            button.Location = new Point(X * Settings.Size + Settings.offset.X, Y * Settings.Size + Settings.offset.Y);
            button.Click += click;
            button.Parent = form;
            button.BackColor = status ? Color.Black : Color.White;
        }

        public void Destroy()
        {
            form.Controls.Remove(button);
            button = null;

        }

        private void click(object sender, System.EventArgs e)
        {
            this.status = !this.status;
            if(status)
            {
                button.BackColor = Color.Black;
            }
            else
            {
                button.BackColor = Color.White;
            }
        }
    }
}
