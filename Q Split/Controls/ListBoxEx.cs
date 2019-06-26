using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;

namespace QSplit.Controls
{
    public partial class ListBoxEx : ListBox, IDisposable
    {
        public enum ItemState
        {
            Normal,
            SelectedFocused,
            SelectedNotFocused,
            Disabled
        }

        public ListBoxEx()
        {
            InitializeComponent();
            this.DrawMode = DrawMode.OwnerDrawFixed;

            if (this.ItemHeight < 20)
                this.ItemHeight = 20;
        }

        ImageList imgList;
        int textLeft = 22;
        List<int> itemsEx = new List<int>();

        public ImageList ImageList
        {
            get { return imgList; }
            set { imgList = value; }
        }

        /// <summary>
        /// Items Icons
        /// </summary>
        public List<int> ItemsEx
        {
            get { return itemsEx; }
            set { itemsEx = value; }
        }

        public int TextLeft
        {
            get { return textLeft; }
            set { textLeft = value; Invalidate(); }
        }

        /// <summary>
        /// Add Extended Item
        /// </summary>
        protected void AddItem(string item, int icon)
        {
            Items.Add(item);
            itemsEx.Add(icon);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.Index >= 0) && (e.Index < this.Items.Count))
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    eDrawItem(e, null, ItemState.SelectedFocused);
                }
                else
                {
                    eDrawItem(e, null, ItemState.Normal);
                }



            }
            else
                base.OnDrawItem(e);
        }

        private void eDrawItem(DrawItemEventArgs e, Bitmap icon, ItemState state)
        {

            if (state == ItemState.SelectedFocused)
            {
                Pen topLinePen = new Pen(Color.FromArgb(19, 124, 215));
                Pen bottomLinePen = new Pen(Color.FromArgb(55, 90, 213));
                LinearGradientBrush brh = new LinearGradientBrush(e.Bounds,
                    Color.FromArgb(65, 153, 227), Color.FromArgb(0, 94, 204), LinearGradientMode.Vertical);

                e.Graphics.FillRectangle(brh, e.Bounds);
                e.Graphics.DrawLine(topLinePen, new Point(0, e.Bounds.Top), new Point(e.Bounds.Width, e.Bounds.Top));
                e.Graphics.DrawLine(bottomLinePen, new Point(0, e.Bounds.Top + e.Bounds.Height - 1), new Point(e.Bounds.Width, e.Bounds.Top + e.Bounds.Height - 1));

                DrawItemIcon(e, state);
                DrawText(e, this.Items[e.Index].ToString(), textLeft, Color.White);

                topLinePen.Dispose();
                bottomLinePen.Dispose();
                brh.Dispose();
            }
            else if (state == ItemState.Normal)
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                DrawItemIcon(e, state);
                DrawText(e, this.Items[e.Index].ToString(), textLeft, Color.Black);
            }



        }

        private void DrawItemIcon(DrawItemEventArgs e, ItemState state)
        {
            if (e.Index < itemsEx.Count && imgList != null)
            {
                if (itemsEx[e.Index] > -1 && itemsEx[e.Index] < imgList.Images.Count)
                    DrawIcon(e, imgList.Images[itemsEx[e.Index]], 4, state);
            }
        }

        private void DrawIcon(DrawItemEventArgs e, Image img, float left, ItemState state)
        {
            e.Graphics.DrawImage(img, left, e.Bounds.Top + (e.Bounds.Height - img.Height) / 2);
        }

        private void DrawText(DrawItemEventArgs e, string str, int left, Color color)
        {
            SolidBrush brh = new SolidBrush(color);
            SizeF sz = e.Graphics.MeasureString(str, this.Font);
            e.Graphics.DrawString(str, this.Font, brh, left, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2);

            brh.Dispose();
        }

    }
}
