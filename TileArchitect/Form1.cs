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
        Tile tile;
        List<Tile> printTiles = new List<Tile>();
        int printedTiles = 0;

        private Font printFont;

        public Form1()
        {
            InitializeComponent();
            tile = new Tile("Tile" + tileList.Items.Count,this);

            imageList1.ImageSize = new Size(Tile.ImageSize, Tile.ImageSize);
            tileList.LargeImageList = imageList1;

            saveTile.Enabled = false;
        }

        private void AddTileButton_Click(object sender, EventArgs e)
        {
            imageList1.Images.Add(tile.name, tile.toImage());
            var listViewItem = tileList.Items.Add(tile.name);
            listViewItem.ImageKey = tile.name;
            listViewItem.Name = tile.name;
            listViewItem.Tag = tile;
            tile.Destroy();
            tile = new Tile("Tile" + tileList.Items.Count,this);
            saveTile.Enabled = false;
        }

        private void saveTile_Click(object sender, EventArgs e)
        {
            var listViewItem = tileList.Items.Find(tile.name,false).First<ListViewItem>();
            listViewItem.Tag = tile;
            imageList1.Images.RemoveByKey(tile.name);
            imageList1.Images.Add(tile.name, tile.toImage());
            tileList.Refresh();
        }

        private void tileList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            tile.Destroy();
            tile = new Tile((Tile)e.Item.Tag);
            saveTile.Enabled = true;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printTiles.Clear();
            foreach(ListViewItem item in tileList.Items)
            {
                printTiles.Add((Tile)item.Tag);
            }
            printFont = new Font("Arial", 10);
            printedTiles = 0;
            PrintDocument doc = new PrintDocument();
            doc.DocumentName = "Tiles";
            doc.PrintPage += new PrintPageEventHandler
                   (this.pd_PrintPage);
            printPreviewDialog1.Document = doc;
            printPreviewDialog1.ShowDialog();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if(!ev.HasMorePages)
            {
                printedTiles = 0;
            }
            float yPos = 0;
            float xPos = 0;

            Rectangle bounds = ev.PageBounds;

            while(yPos + Tile.SizeOnPage < bounds.Height)
            {
                while(xPos + Tile.SizeOnPage < bounds.Width)
                {
                    if(printTiles.Count <= printedTiles)
                    {
                        return;
                    }
                    Tile temp = printTiles[printedTiles];
                    temp.Print(ev, xPos, yPos);
                    ev.Graphics.DrawRectangle(new Pen(Brushes.Black), xPos, yPos, Tile.SizeOnPage + 1, Tile.SizeOnPage + 1);

                    xPos += Tile.SizeOnPage + 1;
                    printedTiles++;
                }
                xPos = 0;
                yPos += Tile.SizeOnPage+1;
            }

            if (printedTiles != printTiles.Count)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string result = "";
            foreach (ListViewItem item in tileList.Items)
            {
                Tile temp = (Tile)item.Tag;
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
            for(int i=0;i<loaded.Length;i+=6)
            {
                Tile temp = new Tile(this);
                string joined = String.Join("\n", loaded, i, 6);
                temp.Load(joined);
                imageList1.Images.Add(temp.name, temp.toImage());
                var listViewItem = tileList.Items.Add(temp.name);
                listViewItem.ImageKey = temp.name;
                listViewItem.Name = temp.name;
                listViewItem.Tag = temp;
            }

            tile.Destroy();
            tile = new Tile("Tile" + tileList.Items.Count, this);

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
