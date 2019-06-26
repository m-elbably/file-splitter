using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace QSplit.Controls
{
    public partial class GLine : UserControl
    {
        public enum ColorsType
        {
            TowColors,
            ThreeColors
        }

        private Color startColor;
        private Color middleColor;
        private Color endColor;
        private ColorsType colors;


        public GLine()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);

            colors = ColorsType.TowColors;
        }

        public Color StartColor
        {
            get { return startColor; }
            set { startColor = value; this.Invalidate(); }
        }

        public Color MiddleColor
        {
            get { return middleColor; }
            set { middleColor = value; this.Invalidate(); }
        }

        public Color EndColor
        {
            get { return endColor; }
            set { endColor = value; this.Invalidate(); }
        }

        [Description("Gradiend colors number \nfrom 2 to 3")]
        public ColorsType Colors
        {
            get { return colors; }
            set { colors = value; this.Invalidate(); }
        }

        private void GLine_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle r1 = new Rectangle(0, 0, this.Width / 2, this.Height);
            Rectangle r2 = new Rectangle(this.Width / 2, 0, this.Width / 2 + 1, this.Height);

            LinearGradientBrush brhA;
            LinearGradientBrush brhB;

            if (colors == ColorsType.TowColors)
            {
                brhA = new LinearGradientBrush(rect, startColor, endColor, LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(brhA, rect);

            }
            else
            {
                brhA = new LinearGradientBrush(r1, startColor, middleColor, LinearGradientMode.Horizontal);
                brhB = new LinearGradientBrush(r2, middleColor, endColor, LinearGradientMode.Horizontal);

                e.Graphics.FillRectangle(brhA, r1);
                e.Graphics.FillRectangle(brhB, r2);
            }
        }

        private void GLine_Load(object sender, EventArgs e)
        {

        }


    }
}
