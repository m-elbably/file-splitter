using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace QSplit
{
    public partial class SplitLine : UserControl
    {
        private Color topColor = Color.White;
        private Color bottomColor = Color.Gray;
        private Color sideColor;
        private int thickness = 1;

        public SplitLine()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.OptimizedDoubleBuffer, true);
        }

        public Color TopColor
        {
            get { return topColor; }
            set { topColor = value; this.Invalidate(); }
        }

        public Color BottomColor
        {
            get { return bottomColor; }
            set { bottomColor = value; this.Invalidate(); }
        }

        public Color SideColor
        {
            get { return sideColor; }
            set { sideColor = value; this.Invalidate(); }
        }

        public int Thickness
        {
            get { return thickness; }
            set { thickness = value; this.Invalidate(); }
        }

        private void SplitLine_Paint(object sender, PaintEventArgs e)
        {
            DrawLine(e.Graphics, topColor, sideColor, 0, thickness);
            DrawLine(e.Graphics, bottomColor, sideColor, thickness, thickness);
        }

        private void DrawLine(Graphics g, Color lineColor, Color subColor, int y, int thickness)
        {
            Rectangle r1 = new Rectangle(0, y, this.Width / 2, thickness);
            Rectangle r2 = new Rectangle(this.Width / 2 - 1, y, this.Width / 2 + 1, thickness);

            LinearGradientBrush brhA = new LinearGradientBrush(r1, subColor, lineColor, LinearGradientMode.Horizontal);
            LinearGradientBrush brhB = new LinearGradientBrush(r2, lineColor, subColor, LinearGradientMode.Horizontal);
            g.FillRectangle(brhA, r1);
            g.FillRectangle(brhB, r2);

            brhA.Dispose();
            brhB.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.Height > 5)
            {
                this.Height = 5;
            }

            base.OnResize(e);
        }
    }
}
