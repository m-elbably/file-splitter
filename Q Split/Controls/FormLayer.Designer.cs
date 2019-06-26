namespace Trans_Layer_on_Form.Control
{
    partial class FormLayer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Tmr = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Tmr
            // 
            this.Tmr.Interval = 10;
            this.Tmr.Tick += new System.EventHandler(this.Tmr_Tick);
            // 
            // FormLayer
            // 
            this.Visible = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Tmr;
    }
}
