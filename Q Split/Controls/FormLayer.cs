using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Trans_Layer_on_Form.Control
{
    public partial class FormLayer : Panel
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public delegate void PanelShowDelegete(object sender, EventArgs e);
        public event PanelShowDelegete PaneShow;

        public delegate void PanelHideDelegete(object sender, EventArgs e);
        public event PanelHideDelegete PaneHide;

        private Bitmap memoryImage;
        private Bitmap blendImage;

        private int layerOpacity;
        private Color layerColor;
        private bool blendEffect;
        private int tOpacity;

        public FormLayer()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            layerOpacity = 140;
            layerColor = Color.White;
        }

        public int LayerOpacity
        {
            get { return layerOpacity; }
            set { layerOpacity = value; }
        }

        public Color LayerColor
        {
            get { return layerColor; }
            set { layerColor = value; }
        }

        public bool BlendEffect
        {
            get { return blendEffect; }
            set { blendEffect = value; }
        }

        private void CaptureScreen()
        {
            if (this.Parent is Form)
            {
                Form frm = (Form)this.Parent;
                Graphics gr = frm.CreateGraphics();
                Size sZ = frm.Size;
                memoryImage = new Bitmap(sZ.Width, sZ.Height, gr);
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                IntPtr dc1 = gr.GetHdc();
                IntPtr dc2 = memoryGraphics.GetHdc();
                BitBlt(dc2, 0, 0, frm.ClientRectangle.Width, frm.ClientRectangle.Height, dc1, 0, 0, 13369376);
                gr.ReleaseHdc(dc1);
                memoryGraphics.ReleaseHdc(dc2);

                gr.Dispose();
            }
        }

        public void UpdateLayer()
        {
            CaptureScreen();

            if (memoryImage == null)
                return;

            Graphics gr = Graphics.FromImage(memoryImage);
            gr.DrawImage(memoryImage, 0, 0);

            if (!blendEffect)
            {
                SolidBrush brh = new SolidBrush(Color.FromArgb(layerOpacity, layerColor));
                gr.FillRectangle(brh, new Rectangle(0, 0, memoryImage.Width, memoryImage.Height));

                brh.Dispose();
            }

            this.Location = new Point(0, 0);
            this.Size = new Size(memoryImage.Width, memoryImage.Height);
            this.BackgroundImage = memoryImage;

            if (blendEffect)
            {
                this.Visible = true;
                tOpacity = 0;
                blendImage = (Bitmap)memoryImage.Clone();
                Tmr.Enabled = true;
            }
        }

        public new void Show()
        {
            if (!this.Visible)
                UpdateLayer();

            PaneShow(this, new EventArgs());
            this.Visible = true;
        }

        public new void Hide()
        {
            Tmr.Enabled = false;
            PaneHide(this, new EventArgs());
            this.Visible = false;
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(blendImage);
            gr.Clear(Color.Transparent);
            gr.DrawImage(memoryImage, 0, 0);

            SolidBrush brh = new SolidBrush(Color.FromArgb(tOpacity, layerColor));
            gr.FillRectangle(brh, new Rectangle(0, 0, blendImage.Width, blendImage.Height));

            brh.Dispose();
            gr.Dispose();
            tOpacity += 2;

            this.BackgroundImage = blendImage;
            this.Invalidate();

            if (tOpacity >= layerOpacity)
                Tmr.Enabled = false;

        }



    }
}
