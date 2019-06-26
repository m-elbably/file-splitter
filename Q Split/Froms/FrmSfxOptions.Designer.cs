namespace QSplit
{
    partial class FrmSfxOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtComment = new System.Windows.Forms.TextBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SpTxtFolder = new System.Windows.Forms.ComboBox();
            this.splitLine1 = new QSplit.SplitLine();
            this.SpBtnFileBrowse = new System.Windows.Forms.Button();
            this.OptCustom = new System.Windows.Forms.RadioButton();
            this.OptParent = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Comment:";
            // 
            // TxtComment
            // 
            this.TxtComment.Location = new System.Drawing.Point(9, 132);
            this.TxtComment.MaxLength = 2048;
            this.TxtComment.Multiline = true;
            this.TxtComment.Name = "TxtComment";
            this.TxtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtComment.Size = new System.Drawing.Size(386, 64);
            this.TxtComment.TabIndex = 4;
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(350, 257);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnOk.Location = new System.Drawing.Point(269, 257);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "&Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(413, 235);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SpTxtFolder);
            this.tabPage1.Controls.Add(this.splitLine1);
            this.tabPage1.Controls.Add(this.SpBtnFileBrowse);
            this.tabPage1.Controls.Add(this.OptCustom);
            this.tabPage1.Controls.Add(this.OptParent);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.TxtComment);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(405, 209);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SpTxtFolder
            // 
            this.SpTxtFolder.Enabled = false;
            this.SpTxtFolder.FormattingEnabled = true;
            this.SpTxtFolder.Location = new System.Drawing.Point(9, 73);
            this.SpTxtFolder.Name = "SpTxtFolder";
            this.SpTxtFolder.Size = new System.Drawing.Size(353, 21);
            this.SpTxtFolder.TabIndex = 2;
            // 
            // splitLine1
            // 
            this.splitLine1.BottomColor = System.Drawing.Color.Gray;
            this.splitLine1.Location = new System.Drawing.Point(12, 106);
            this.splitLine1.Name = "splitLine1";
            this.splitLine1.SideColor = System.Drawing.Color.Empty;
            this.splitLine1.Size = new System.Drawing.Size(383, 5);
            this.splitLine1.TabIndex = 9;
            this.splitLine1.Thickness = 1;
            this.splitLine1.TopColor = System.Drawing.Color.White;
            // 
            // SpBtnFileBrowse
            // 
            this.SpBtnFileBrowse.BackColor = System.Drawing.SystemColors.Control;
            this.SpBtnFileBrowse.Enabled = false;
            this.SpBtnFileBrowse.Location = new System.Drawing.Point(368, 71);
            this.SpBtnFileBrowse.Name = "SpBtnFileBrowse";
            this.SpBtnFileBrowse.Size = new System.Drawing.Size(27, 23);
            this.SpBtnFileBrowse.TabIndex = 3;
            this.SpBtnFileBrowse.Text = "...";
            this.SpBtnFileBrowse.UseVisualStyleBackColor = true;
            this.SpBtnFileBrowse.Click += new System.EventHandler(this.SpBtnFileBrowse_Click);
            // 
            // OptCustom
            // 
            this.OptCustom.AutoSize = true;
            this.OptCustom.Location = new System.Drawing.Point(9, 50);
            this.OptCustom.Name = "OptCustom";
            this.OptCustom.Size = new System.Drawing.Size(98, 17);
            this.OptCustom.TabIndex = 1;
            this.OptCustom.Text = "Custom Folder:";
            this.OptCustom.UseVisualStyleBackColor = true;
            this.OptCustom.CheckedChanged += new System.EventHandler(this.OptCustom_CheckedChanged);
            // 
            // OptParent
            // 
            this.OptParent.AutoSize = true;
            this.OptParent.Checked = true;
            this.OptParent.Location = new System.Drawing.Point(9, 28);
            this.OptParent.Name = "OptParent";
            this.OptParent.Size = new System.Drawing.Size(90, 17);
            this.OptParent.TabIndex = 0;
            this.OptParent.TabStop = true;
            this.OptParent.Text = "Parent Folder";
            this.OptParent.UseVisualStyleBackColor = true;
            this.OptParent.CheckedChanged += new System.EventHandler(this.OptParent_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Destination Folder:";
            // 
            // FrmSfxOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(436, 288);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.BtnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSfxOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Self Joiner Options";
            this.Load += new System.EventHandler(this.FrmSfxOptions_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtComment;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RadioButton OptCustom;
        private System.Windows.Forms.RadioButton OptParent;
        private System.Windows.Forms.Label label2;
        private SplitLine splitLine1;
        private System.Windows.Forms.Button SpBtnFileBrowse;
        private System.Windows.Forms.ComboBox SpTxtFolder;
    }
}