using System.Windows.Forms;

namespace QSplit
{
    public class FrmBase : Form
    {
        public FrmBase()
        {
        }

        public virtual void ShowMessage(string message,MsgType mType)
        {
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmBase
            // 
            this.ClientSize = new System.Drawing.Size(154, 35);
            this.Name = "FrmBase";
            this.ResumeLayout(false);

        }
    }
}
