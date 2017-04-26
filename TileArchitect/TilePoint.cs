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
        public bool Status = false;
        public Button Button;
        public Form1 Form;

        public TilePoint(int X, int Y, Form1 form)
        {
            this.X = X;
            this.Y = Y;

            BuildButton();

            Form = form;
        }

        public void BuildButton()
        {
            Button = new Button
            {
                Width = Settings.Size,
                Height = Settings.Size,
                Location = new Point(X * Settings.Size + Settings.Offset.X, Y * Settings.Size + Settings.Offset.Y)
            };
            Button.Click += Click;
            Button.Parent = Form;
            Button.BackColor = Status ? Color.Black : Color.White;
        }

        public void Destroy()
        {
            Form.Controls.Remove(Button);
            Button = null;
        }

        private void Click(object sender, System.EventArgs e)
        {
            this.Status = !this.Status;
            Button.BackColor = Status ? Color.Black : Color.White;
        }
    }
}