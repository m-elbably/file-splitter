using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QSplit.Tools;

namespace QSplit
{
    public partial class FrmSfxOptions : Form
    {
        public FrmSfxOptions()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            if (Sfx.CustomFolder)
            {
                OptCustom.Checked = true;
            }

            SpTxtFolder.Text = Sfx.OutPath;
            TxtComment.Text = Sfx.Comment;
        }

        private void SaveData()
        {
            if (OptCustom.Checked)
                Sfx.OutPath = SpTxtFolder.Text;
            else
            {
                Sfx.OutPath = "";
            }

            Sfx.CustomFolder = OptCustom.Checked;
            Sfx.Comment = TxtComment.Text;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SaveData();
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmSfxOptions_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SpBtnFileBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SpTxtFolder.Text = dlg.SelectedPath;
            }
        }

        private void OptParent_CheckedChanged(object sender, EventArgs e)
        {
            SetControls(OptCustom.Checked);
        }

        private void OptCustom_CheckedChanged(object sender, EventArgs e)
        {
            SetControls(OptCustom.Checked);
        }

        private void SetControls(bool status)
        {
            SpTxtFolder.Enabled = status;
            SpBtnFileBrowse.Enabled = status;
        }

    }
}
