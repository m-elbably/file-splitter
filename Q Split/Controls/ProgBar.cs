using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Controls_Pool.Controls
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class ProgBar : UserControl
    {
        public ProgBar()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint |
              ControlStyles.SupportsTransparentBackColor, true);

        }

        private int max = 100;
        private int value = 0;
        private Color borderColor = SystemColors.ButtonShadow;
        private int borderColorOpacity = 255;
        private Color startColor = Color.White;
        private Color endColor = Color.White;
        private int startColorOpacity = 255;
        private int endColorOpacity = 255;
        LinearGradientMode gradientMode = LinearGradientMode.ForwardDiagonal;


        public int Value
        {
            get { return this.value; }
            set { this.value = value; Invalidate(); }
        }

        public int Max
        {
            get { return max; }
            set { max = value; Invalidate(); }
        }

        [Browsable(true), Category("Appearance")]
        public Color StartColor
        {
            get { return startColor; }
            set { startColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Appearance")]
        public Color EndColor
        {
            get { return endColor; }
            set { endColor = value; Invalidate(); }
        }

        [Description("Opacity Value:\n1 - 255"), Browsable(true), Category("Appearance")]
        public int StartColorOpacity
        {
            get { return startColorOpacity; }
            set
            {
                if (value > 255 || value < 0)
                    startColorOpacity = 180;
                else
                    startColorOpacity = value;

                Invalidate();
            }
        }

        [Description("Opacity Value:\n1 - 255"), Browsable(true), Category("Appearance")]
        public int EndColorOpacity
        {
            get { return endColorOpacity; }
            set
            {
                if (value > 255 || value < 0)
                    endColorOpacity = 180;
                else
                    endColorOpacity = value;

                Invalidate();
            }
        }

        [Browsable(true), Category("Appearance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Appearance")]
        public int BorderColorOpacity
        {
            get { return borderColorOpacity; }
            set
            {
                if (value > 255 || value < 0)
                    borderColorOpacity = 180;
                else
                    borderColorOpacity = value;

                Invalidate();
            }
        }

        [Browsable(true), Category("Appearance")]
        public LinearGradientMode GradientMode
        {
            get { return gradientMode; }
            set { gradientMode = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int vWidth = 0;

            if (value <= max)
                vWidth = value * this.Width / max;
            else
                vWidth = this.Width;

            Pen bPen = new Pen(Color.FromArgb(borderColorOpacity, borderColor), 1);
            SolidBrush sBrh = new SolidBrush(Color.FromArgb(100, Color.White));
            Rectangle borderRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            Rectangle gBorderPath = borderRect;
            e.Graphics.FillRectangle(sBrh, gBorderPath);

            if (vWidth > 2 && this.Height > 2)
            {
                RectangleF vRect = new Rectangle(0, 0, vWidth - 1, this.Height - 1);

                using (LinearGradientBrush brh = new LinearGradientBrush(vRect, Color.FromArgb(startColorOpacity, startColor),
                                  Color.FromArgb(endColorOpacity, endColor), gradientMode))
                {
                    e.Graphics.FillRectangle(brh, vRect);
                }

            }

            e.Graphics.DrawRectangle(bPen, gBorderPath);

            bPen.Dispose();
            sBrh.Dispose();
            base.OnPaint(e);
        }

        //private GraphicsPath GetRoundPath(Rectangle r, int depth)
        //{
        //    int dp = 0;
        //    GraphicsPath gPath = new GraphicsPath();

        //    if (depth <= 0)
        //    {
        //        gPath.AddRectangle(r);
        //        return gPath;
        //    }

        //    gPath.AddArc(r.X, r.Y, depth, depth, 180, 90);
        //    gPath.AddArc(r.X + r.Width - depth, r.Y, depth, depth, 270, 90);
        //    gPath.AddArc(r.X + r.Width - depth, r.Y + r.Height - depth, depth, depth, 0, 90);
        //    gPath.AddArc(r.X, r.Y + r.Height - depth, depth, depth, 90, 90);
        //    //gPath.AddArc(r.X, r.Y+depth/2, depth, depth, 90, 180);

        //    dp = depth / 2;
        //    gPath.AddLine(r.X, r.Y + dp, r.X, r.Y + dp);

        //    return gPath;
        //}


    }
}
