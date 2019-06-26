using System;
using System.Windows.Forms;
using QSplit.Tools;
using QSplit.Types;
using XmlLib;
using System.Drawing;
using System.IO;

namespace QSplit
{
    public partial class FrmNewProfile : Form
    {
        private bool editMode;
        private Profile editProfile;
        private XmlSettings Ini = new XmlSettings(Path.Combine(Application.StartupPath, "config.xml"));

        public FrmNewProfile(bool editMode, Profile buffer)
        {
            InitializeComponent();

            if (editMode)
            {
                this.Text = "Edit Profile";
                TxtProfile.Enabled = false;
                TxtProfile.Text = buffer.Name;
                TxtSize.Text = Math.Round(buffer.Size / Math.Pow(1024, buffer.SizeIndex),4).ToString();
                CmbUnit.SelectedIndex = buffer.SizeIndex;
                TxtDesc.Text = buffer.Description;
            }
            else
            {
                CmbUnit.SelectedIndex = 2;
            }

            this.editMode = editMode;
            editProfile = buffer;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            bool error;
            string pName = TxtProfile.Text;
            long pSize = 0;
            double dblSize = 0;
            double.TryParse(TxtSize.Text, out dblSize);
            pSize = (long)(dblSize * Math.Pow(1024, CmbUnit.SelectedIndex));

            if (pSize > (long)Math.Pow(1024, 5))
            {
                MessageBox.Show("Invalid size, Part size is too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            error = CheckErrors();

            if (!error)
            {
                if (!editMode)
                {
                    if (Ini.KeyExists("profiles", pName))
                    {
                        MessageBox.Show("Profile name '" + pName + "' is already existed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    Ini.WriteValue("profiles", pName, IoTools.ParseSize(pSize));
                }

                Ini.WriteValue(pName, "name", TxtProfile.Text);
                Ini.WriteValue(pName, "size", pSize.ToString());
                Ini.WriteValue(pName, "index", CmbUnit.SelectedIndex);
                Ini.WriteValue(pName, "description", TxtDesc.Text);

                this.Close();
            }

        }

        private bool CheckErrors()
        {
            if (TxtProfile.Text.Length == 0)
            {
                ShowError("Profile name is missing", TxtProfile);
                return true;
            }

            if (TxtSize.Text.Length == 0)
            {
                ShowError("Part size is missing", TxtSize);
                return true;
            }

            return false;
        }

        private void FrmNewProfile_Load(object sender, EventArgs e)
        {
        }

        private void TxtProfile_TextChanged(object sender, EventArgs e)
        {
            Header.Text = TxtProfile.Text + " Profile";
            HideError();
        }

        private void TxtSize_TextChanged(object sender, EventArgs e)
        {
            HideError();
        }

        private void ShowError(string message, Control ctl)
        {
            ErLabel.Text = message;
            ErPanel.Width = ErLabel.Width + 25;
            ErPanel.Location = new Point(ctl.Left, ctl.Top + ctl.Height);
            ErPanel.Visible = true;
        }

        private void HideError()
        {
            ErPanel.Visible = false;
        }

        private void Controls_Enter(object sender, EventArgs e)
        {
            HideError();
        }


    }
}
