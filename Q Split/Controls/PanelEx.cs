using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QSplit.Controls
{
    public partial class PanelEx : Panel
    {
        private Color startColor;
        private Color endColor;
        private int startColorOpacity;
        private int endColorOpacity;
        private LinearGradientMode gradientMode;
        private Color borderColor;
        private int borderColorOpacity;
        private int borderWidth;
        private int cornerArc;
        private Color shadowColor;
        private int shadowDepth;
        private int shadowOpacity;
        private EffectType effect;
        private int glowDepth;

        private string panelText;
        private Font textFont;
        private TextPosition textAlign;
        private Color textColor;

        private bool selected = false;

        public enum EffectType
        {
            Shadow,
            Glow
        }

        public enum TextPosition
        {
            Left,
            Center
        }

        [Browsable(true), Category("Panel Appearance")]
        public Color StartColor
        {
            get { return startColor; }
            set { startColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public Color EndColor
        {
            get { return endColor; }
            set { endColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public LinearGradientMode GradientMode
        {
            get { return gradientMode; }
            set { gradientMode = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
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

        [Browsable(true), Category("Panel Appearance")]
        public int CornerArc
        {
            get { return cornerArc; }
            set { cornerArc = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public Color ShadowColor
        {
            get { return shadowColor; }
            set { shadowColor = value; Invalidate(); }
        }

        [Description("Opacity Value:\n1 - 255"), Browsable(true), Category("Panel Appearance")]
        public int ShadowOpacity
        {
            get { return shadowOpacity; }
            set
            {
                if (value > 255 || value < 0)
                    shadowOpacity = 180;
                else
                    shadowOpacity = value;

                Invalidate();
            }
        }

        [Description("Opacity Value:\n1 - 255"), Browsable(true), Category("Panel Appearance")]
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

        [Description("Opacity Value:\n1 - 255"), Browsable(true), Category("Panel Appearance")]
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

        [Browsable(true), Category("Panel Appearance")]
        public int ShadowDepth
        {
            get { return shadowDepth; }
            set { shadowDepth = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public EffectType Effect
        {
            get { return effect; }
            set { effect = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public int GlowDepth
        {
            get { return glowDepth; }
            set { glowDepth = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public string PanelText
        {
            get { return panelText; }
            set { panelText = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public Font TextFont
        {
            get { return textFont; }
            set { textFont = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Panel Appearance")]
        public TextPosition TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        [Browsable(true), Category("Panel Appearance")]
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public PanelEx()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.UserPaint |
                          ControlStyles.SupportsTransparentBackColor, true);

            startColor = Color.White;
            endColor = Color.White;
            gradientMode = LinearGradientMode.ForwardDiagonal;
            borderColor = Color.DarkGray;
            borderColorOpacity = 180;
            borderWidth = 1;
            cornerArc = 5;
            shadowColor = Color.DimGray;
            shadowDepth = 5;
            shadowOpacity = 180;
            startColorOpacity = 255;
            endColorOpacity = 255;

            panelText = "";
            textFont = this.Font;
            textColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.Width > 1 && this.Height > 1)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle mainRect;
                Rectangle shadowRect;

                if (effect == EffectType.Shadow)
                {
                    mainRect = new Rectangle(0, 0, this.Width - shadowDepth - 1, this.Height - shadowDepth - 1);
                    shadowRect = new Rectangle(shadowDepth, shadowDepth, this.Width - shadowDepth - 1, this.Height - shadowDepth - 1);
                }
                else
                {
                    mainRect = new Rectangle(glowDepth, glowDepth, this.Width - (glowDepth * 2) - 1, this.Height - (glowDepth * 2) - 1);
                    shadowRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                }

                GraphicsPath gShadowPath = GetRoundPath(shadowRect, cornerArc);
                GraphicsPath gMainPath = GetRoundPath(mainRect, cornerArc);

                if (cornerArc > 0)
                {
                    using (PathGradientBrush gBrush = new PathGradientBrush(gShadowPath))
                    {
                        gBrush.WrapMode = WrapMode.Clamp;
                        ColorBlend colorBlend = new ColorBlend(3);

                        colorBlend.Colors = new Color[]{Color.Transparent, 
													Color.FromArgb(shadowOpacity, shadowColor), 
													Color.FromArgb(shadowOpacity, shadowColor)};

                        colorBlend.Positions = new float[] { 0f, .1f, 1f };

                        gBrush.InterpolationColors = colorBlend;
                        e.Graphics.FillPath(gBrush, gShadowPath);
                    }
                }

                // Draw backgroup
                LinearGradientBrush brh = null;
                if (mainRect.Width > 1 && mainRect.Height > 1)
                {
                    brh = new LinearGradientBrush(mainRect, Color.FromArgb(startColorOpacity, startColor),
                                                  Color.FromArgb(endColorOpacity, endColor), gradientMode);
                    e.Graphics.FillPath(brh, gMainPath);
                    e.Graphics.DrawPath(new Pen(Color.FromArgb(borderColorOpacity, borderColor), borderWidth), gMainPath);

                    //Draw Text
                    if (panelText.Length > 0)
                    {
                        Size sZ = e.Graphics.MeasureString(panelText, textFont).ToSize();
                        Point pnt = new Point();
                        if (textAlign == TextPosition.Left)
                            pnt = new Point(5, (this.Height - sZ.Height) / 2);
                        else if (textAlign == TextPosition.Center)
                            pnt = new Point((this.Width - sZ.Width) / 2, (this.Height - sZ.Height) / 2);

                        SolidBrush textBrush = new SolidBrush(textColor);

                        e.Graphics.DrawString(panelText, textFont, textBrush, pnt);
                        textBrush.Dispose();
                    }

                }


                //Clean
                if (brh != null)
                    brh.Dispose();

                gMainPath.Dispose();
                gShadowPath.Dispose();
            }
        }

        #region Required Methods...

        private GraphicsPath GetRoundPath(Rectangle r, int depth)
        {
            GraphicsPath gPath = new GraphicsPath();

            if (depth <= 0)
            {
                gPath.AddRectangle(r);
                return gPath;
            }

            gPath.AddArc(r.X, r.Y, depth, depth, 180, 90);
            gPath.AddArc(r.X + r.Width - depth, r.Y, depth, depth, 270, 90);
            gPath.AddArc(r.X + r.Width - depth, r.Y + r.Height - depth, depth, depth, 0, 90);
            gPath.AddArc(r.X, r.Y + r.Height - depth, depth, depth, 90, 90);
            gPath.AddLine(r.X, r.Y + r.Height - depth, r.X, r.Y + depth / 2);

            return gPath;
        }




        #endregion

    }
}
