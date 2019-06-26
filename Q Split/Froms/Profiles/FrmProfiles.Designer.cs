namespace QSplit
{
    partial class FrmProfiles
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProfiles));
            this.Lv = new System.Windows.Forms.ListView();
            this.pName = new System.Windows.Forms.ColumnHeader();
            this.pSize = new System.Windows.Forms.ColumnHeader();
            this.pDescription = new System.Windows.Forms.ColumnHeader();
            this.Img = new System.Windows.Forms.ImageList(this.components);
            this.Ts = new System.Windows.Forms.ToolStrip();
            this.Add = new System.Windows.Forms.ToolStripButton();
            this.Edit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Delete = new System.Windows.Forms.ToolStripButton();
            this.Sb = new System.Windows.Forms.StatusStrip();
            this.SbItem = new System.Windows.Forms.ToolStripStatusLabel();
            this.Ts.SuspendLayout();
            this.Sb.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lv
            // 
            this.Lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pName,
            this.pSize,
            this.pDescription});
            this.Lv.FullRowSelect = true;
            this.Lv.Location = new System.Drawing.Point(12, 41);
            this.Lv.MultiSelect = false;
            this.Lv.Name = "Lv";
            this.Lv.Size = new System.Drawing.Size(411, 217);
            this.Lv.SmallImageList = this.Img;
            this.Lv.TabIndex = 1;
            this.Lv.UseCompatibleStateImageBehavior = false;
            this.Lv.View = System.Windows.Forms.View.Details;
            this.Lv.SelectedIndexChanged += new System.EventHandler(this.Lv_SelectedIndexChanged);
            this.Lv.DoubleClick += new System.EventHandler(this.Lv_DoubleClick);
            // 
            // pName
            // 
            this.pName.Text = "Name";
            this.pName.Width = 100;
            // 
            // pSize
            // 
            this.pSize.Text = "Part Size";
            // 
            // pDescription
            // 
            this.pDescription.Text = "Description";
            this.pDescription.Width = 220;
            // 
            // Img
            // 
            this.Img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Img.ImageStream")));
            this.Img.TransparentColor = System.Drawing.Color.Transparent;
            this.Img.Images.SetKeyName(0, "circle.png");
            // 
            // Ts
            // 
            this.Ts.AutoSize = false;
            this.Ts.Dock = System.Windows.Forms.DockStyle.None;
            this.Ts.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Add,
            this.Edit,
            this.toolStripSeparator1,
            this.Delete});
            this.Ts.Location = new System.Drawing.Point(12, 9);
            this.Ts.Name = "Ts";
            this.Ts.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Ts.Size = new System.Drawing.Size(411, 29);
            this.Ts.TabIndex = 0;
            this.Ts.Text = "toolStrip1";
            // 
            // Add
            // 
            this.Add.AutoToolTip = false;
            this.Add.Image = ((System.Drawing.Image)(resources.GetObject("Add.Image")));
            this.Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(86, 26);
            this.Add.Text = "Add Profile";
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Edit
            // 
            this.Edit.AutoToolTip = false;
            this.Edit.Enabled = false;
            this.Edit.Image = ((System.Drawing.Image)(resources.GetObject("Edit.Image")));
            this.Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(47, 26);
            this.Edit.Text = "Edit";
            this.Edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // Delete
            // 
            this.Delete.AutoToolTip = false;
            this.Delete.Enabled = false;
            this.Delete.Image = ((System.Drawing.Image)(resources.GetObject("Delete.Image")));
            this.Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(60, 26);
            this.Delete.Text = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Sb
            // 
            this.Sb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SbItem});
            this.Sb.Location = new System.Drawing.Point(0, 269);
            this.Sb.Name = "Sb";
            this.Sb.Size = new System.Drawing.Size(435, 22);
            this.Sb.SizingGrip = false;
            this.Sb.TabIndex = 2;
            // 
            // SbItem
            // 
            this.SbItem.Name = "SbItem";
            this.SbItem.Size = new System.Drawing.Size(0, 17);
            // 
            // FrmProfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 291);
            this.Controls.Add(this.Sb);
            this.Controls.Add(this.Ts);
            this.Controls.Add(this.Lv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProfiles";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profiles";
            this.Load += new System.EventHandler(this.FrmProfiles_Load);
            this.Ts.ResumeLayout(false);
            this.Ts.PerformLayout();
            this.Sb.ResumeLayout(false);
            this.Sb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView Lv;
        private System.Windows.Forms.ColumnHeader pName;
        private System.Windows.Forms.ColumnHeader pSize;
        private System.Windows.Forms.ColumnHeader pDescription;
        private System.Windows.Forms.ImageList Img;
        private System.Windows.Forms.ToolStrip Ts;
        private System.Windows.Forms.ToolStripButton Add;
        private System.Windows.Forms.ToolStripButton Edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton Delete;
        private System.Windows.Forms.StatusStrip Sb;
        private System.Windows.Forms.ToolStripStatusLabel SbItem;
    }
}