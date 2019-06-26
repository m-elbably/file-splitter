namespace QSplit
{
    partial class FrmNewProfile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewProfile));
            this.label1 = new System.Windows.Forms.Label();
            this.CmbUnit = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtDesc = new System.Windows.Forms.TextBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.ErPanel = new QSplit.Controls.PanelEx();
            this.ErLabel = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.TxtSize = new QSplit.TextBoxEx();
            this.TxtProfile = new QSplit.TextBoxEx();
            this.panelEx1 = new QSplit.Controls.PanelEx();
            this.Header = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ErPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Profile Name:";
            // 
            // CmbUnit
            // 
            this.CmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbUnit.FormattingEnabled = true;
            this.CmbUnit.Items.AddRange(new object[] {
            "Bytes",
            "KB",
            "MB",
            "GB",
            "TB"});
            this.CmbUnit.Location = new System.Drawing.Point(99, 127);
            this.CmbUnit.Name = "CmbUnit";
            this.CmbUnit.Size = new System.Drawing.Size(90, 21);
            this.CmbUnit.TabIndex = 2;
            this.CmbUnit.Enter += new System.EventHandler(this.Controls_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Part Size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Profile Description:";
            // 
            // TxtDesc
            // 
            this.TxtDesc.BackColor = System.Drawing.SystemColors.Window;
            this.TxtDesc.Location = new System.Drawing.Point(15, 175);
            this.TxtDesc.MaxLength = 128;
            this.TxtDesc.Name = "TxtDesc";
            this.TxtDesc.Size = new System.Drawing.Size(285, 20);
            this.TxtDesc.TabIndex = 3;
            this.TxtDesc.Enter += new System.EventHandler(this.Controls_Enter);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(225, 211);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.SystemColors.Control;
            this.BtnOk.Location = new System.Drawing.Point(144, 211);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 4;
            this.BtnOk.Text = "&Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // ErPanel
            // 
            this.ErPanel.BorderColor = System.Drawing.Color.Gray;
            this.ErPanel.BorderColorOpacity = 180;
            this.ErPanel.BorderWidth = 1;
            this.ErPanel.Controls.Add(this.ErLabel);
            this.ErPanel.Controls.Add(this.pictureBox2);
            this.ErPanel.CornerArc = 0;
            this.ErPanel.Effect = QSplit.Controls.PanelEx.EffectType.Shadow;
            this.ErPanel.EndColor = System.Drawing.SystemColors.Control;
            this.ErPanel.EndColorOpacity = 255;
            this.ErPanel.GlowDepth = 0;
            this.ErPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.ErPanel.Location = new System.Drawing.Point(15, 211);
            this.ErPanel.Name = "ErPanel";
            this.ErPanel.PanelText = "";
            this.ErPanel.Selected = false;
            this.ErPanel.ShadowColor = System.Drawing.Color.DimGray;
            this.ErPanel.ShadowDepth = 0;
            this.ErPanel.ShadowOpacity = 180;
            this.ErPanel.Size = new System.Drawing.Size(123, 22);
            this.ErPanel.StartColor = System.Drawing.Color.White;
            this.ErPanel.StartColorOpacity = 255;
            this.ErPanel.TabIndex = 70;
            this.ErPanel.TextAlign = QSplit.Controls.PanelEx.TextPosition.Left;
            this.ErPanel.TextColor = System.Drawing.Color.Black;
            this.ErPanel.TextFont = new System.Drawing.Font("Tahoma", 8F);
            this.ErPanel.Visible = false;
            // 
            // ErLabel
            // 
            this.ErLabel.AutoSize = true;
            this.ErLabel.BackColor = System.Drawing.Color.Transparent;
            this.ErLabel.Location = new System.Drawing.Point(22, 4);
            this.ErLabel.Name = "ErLabel";
            this.ErLabel.Size = new System.Drawing.Size(31, 13);
            this.ErLabel.TabIndex = 1;
            this.ErLabel.Text = "Error";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::QSplit.Properties.Resources.error;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // TxtSize
            // 
            this.TxtSize.AllowNegative = false;
            this.TxtSize.Location = new System.Drawing.Point(15, 128);
            this.TxtSize.MaxLength = 16;
            this.TxtSize.Name = "TxtSize";
            this.TxtSize.Size = new System.Drawing.Size(78, 20);
            this.TxtSize.TabIndex = 1;
            this.TxtSize.Type = QSplit.TextBoxEx.TBoxType.Float;
            this.TxtSize.TextChanged += new System.EventHandler(this.TxtSize_TextChanged);
            this.TxtSize.Enter += new System.EventHandler(this.Controls_Enter);
            // 
            // TxtProfile
            // 
            this.TxtProfile.AllowNegative = true;
            this.TxtProfile.Location = new System.Drawing.Point(15, 82);
            this.TxtProfile.MaxLength = 32;
            this.TxtProfile.Name = "TxtProfile";
            this.TxtProfile.Size = new System.Drawing.Size(174, 20);
            this.TxtProfile.TabIndex = 0;
            this.TxtProfile.Type = QSplit.TextBoxEx.TBoxType.StartWithChar;
            this.TxtProfile.TextChanged += new System.EventHandler(this.TxtProfile_TextChanged);
            this.TxtProfile.Enter += new System.EventHandler(this.Controls_Enter);
            // 
            // panelEx1
            // 
            this.panelEx1.BorderColor = System.Drawing.Color.LightGray;
            this.panelEx1.BorderColorOpacity = 180;
            this.panelEx1.BorderWidth = 1;
            this.panelEx1.Controls.Add(this.Header);
            this.panelEx1.Controls.Add(this.pictureBox1);
            this.panelEx1.CornerArc = 8;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Effect = QSplit.Controls.PanelEx.EffectType.Glow;
            this.panelEx1.EndColor = System.Drawing.Color.White;
            this.panelEx1.EndColorOpacity = 255;
            this.panelEx1.GlowDepth = 3;
            this.panelEx1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.PanelText = "";
            this.panelEx1.Selected = false;
            this.panelEx1.ShadowColor = System.Drawing.Color.Gainsboro;
            this.panelEx1.ShadowDepth = 5;
            this.panelEx1.ShadowOpacity = 180;
            this.panelEx1.Size = new System.Drawing.Size(314, 56);
            this.panelEx1.StartColor = System.Drawing.Color.White;
            this.panelEx1.StartColorOpacity = 255;
            this.panelEx1.TabIndex = 67;
            this.panelEx1.TextAlign = QSplit.Controls.PanelEx.TextPosition.Left;
            this.panelEx1.TextColor = System.Drawing.Color.Black;
            this.panelEx1.TextFont = new System.Drawing.Font("Tahoma", 8F);
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.BackColor = System.Drawing.Color.Transparent;
            this.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Header.Location = new System.Drawing.Point(50, 22);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(50, 13);
            this.Header.TabIndex = 1;
            this.Header.Text = "Profile";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // FrmNewProfile
            // 
            this.AcceptButton = this.BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(314, 245);
            this.Controls.Add(this.ErPanel);
            this.Controls.Add(this.TxtSize);
            this.Controls.Add(this.TxtProfile);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.CmbUnit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtDesc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNewProfile";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Profile";
            this.Load += new System.EventHandler(this.FrmNewProfile_Load);
            this.ErPanel.ResumeLayout(false);
            this.ErPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CmbUnit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtDesc;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private QSplit.Controls.PanelEx panelEx1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Header;
        private TextBoxEx TxtProfile;
        private TextBoxEx TxtSize;
        private QSplit.Controls.PanelEx ErPanel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label ErLabel;
    }
}

