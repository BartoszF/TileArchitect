using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileArchitect
{
    public partial class Form1 : Form
    {
        Tile _tile;
        List<Tile> _printTiles = new List<Tile>();
        int _printedTiles = 0;

        private Font _printFont;

        private SettingsForm _settingsForm;

        public Form1()
        {
            InitializeComponent();
            _tile = new Tile("Tile" + tileList.Items.Count, this);

            imageList1.ImageSize = new Size(Settings.ImageSize, Settings.ImageSize);
            tileList.LargeImageList = imageList1;

            saveTile.Enabled = false;

            _settingsForm = new SettingsForm();
            _settingsForm.Hide();
        }

        private void AddTileButton_Click(object sender, EventArgs e)
        {
            imageList1.Images.Add(_tile.Name, _tile.ToImage());
            var listViewItem = tileList.Items.Add(_tile.Name);
            listViewItem.ImageKey = _tile.Name;
            listViewItem.Name = _tile.Name;
            listViewItem.Tag = _tile;
            _tile.Destroy();
            _tile = new Tile("Tile" + tileList.Items.Count, this);
            saveTile.Enabled = false;
        }

        private void saveTile_Click(object sender, EventArgs e)
        {
            var listViewItem = tileList.Items.Find(_tile.Name, false).First<ListViewItem>();
            listViewItem.Tag = _tile;
            imageList1.Images.RemoveByKey(_tile.Name);
            imageList1.Images.Add(_tile.Name, _tile.ToImage());
            tileList.Refresh();
        }

        private void tileList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _tile.Destroy();
            _tile = new Tile((Tile) e.Item.Tag);
            saveTile.Enabled = true;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _printTiles.Clear();
            foreach (ListViewItem item in tileList.Items)
            {
                _printTiles.Add((Tile) item.Tag);
            }
            _printFont = new Font("Arial", 10);
            _printedTiles = 0;
            PrintDocument doc = new PrintDocument();
            doc.DocumentName = "Tiles";
            doc.PrintPage += new PrintPageEventHandler
                (this.pd_PrintPage);
            printPreviewDialog1.Document = doc;
            printPreviewDialog1.ShowDialog();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if (!ev.HasMorePages)
            {
                _printedTiles = 0;
            }
            float yPos = 0;
            float xPos = 0;

            Rectangle bounds = ev.PageBounds;

            while (yPos + Settings.SizeOnPage < bounds.Height)
            {
                while (xPos + Settings.SizeOnPage < bounds.Width)
                {
                    if (_printTiles.Count <= _printedTiles)
                    {
                        return;
                    }
                    Tile temp = _printTiles[_printedTiles];
                    temp.Print(ev, xPos, yPos);
                    ev.Graphics.DrawRectangle(new Pen(Brushes.Black), xPos, yPos, Settings.SizeOnPage + 1,
                        Settings.SizeOnPage + 1);

                    xPos += Settings.SizeOnPage + 1;
                    _printedTiles++;
                }
                xPos = 0;
                yPos += Settings.SizeOnPage + 1;
            }

            ev.HasMorePages = _printedTiles != _printTiles.Count;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string result = "";
            foreach (ListViewItem item in tileList.Items)
            {
                Tile temp = (Tile) item.Tag;
                result += temp.Save();
            }

            File.WriteAllText(saveFileDialog1.FileName, result);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            tileList.Items.Clear();
            string[] loaded = File.ReadAllLines(openFileDialog1.FileName);
            for (int i = 0; i < loaded.Length; i += 6)
            {
                Tile temp = new Tile(this);
                string joined = String.Join("\n", loaded, i, 6);
                temp.Load(joined);
                imageList1.Images.Add(temp.Name, temp.ToImage());
                var listViewItem = tileList.Items.Add(temp.Name);
                listViewItem.ImageKey = temp.Name;
                listViewItem.Name = temp.Name;
                listViewItem.Tag = temp;
            }

            _tile.Destroy();
            _tile = new Tile("Tile" + tileList.Items.Count, this);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_settingsForm == null)
            {
                _settingsForm = new SettingsForm();
            }
            else
            {
                _settingsForm.Show();
            }
        }
    }
}