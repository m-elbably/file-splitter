using System;
using System.Windows.Forms;
using QSplit.Types;
using XmlLib;
using System.IO;

namespace QSplit
{
    public partial class FrmProfiles : Form
    {
        XmlSettings Ini = new XmlSettings(Path.Combine(Application.StartupPath, "config.xml"));
        Profile buffer = new Profile();

        public FrmProfiles()
        {
            InitializeComponent();
        }

        private void FrmProfiles_Load(object sender, EventArgs e)
        {
            LoadData();

        }

        private void LoadData()
        {
            Lv.Items.Clear();
            string[] profiles = Ini.GetKeys("profiles");

            if (profiles == null)
                return;

            for (int i = 0; i < profiles.Length; i++)
            {
                AddProfile(profiles[i]);
            }

            UpdateStatus();
        }

        private void AddProfile(string profileSection)
        {
            buffer.ReadProfile(profileSection);

            ListViewItem lvi = Lv.Items.Add(buffer.Name, 0);
            lvi.SubItems.Add(buffer.GetSize());
            lvi.SubItems.Add(buffer.Description);
        }

        private void UpdateProfile(int index, string profileSection)
        {
            Ini.Refresh();
            buffer.ReadProfile(profileSection);

            Lv.Items[index].Text = buffer.Name;
            Lv.Items[index].SubItems[1].Text = buffer.GetSize();
            Lv.Items[index].SubItems[2].Text = buffer.Description;
        }

        private void RefreshProfiles()
        {
            Ini.Refresh();
            string[] profiles = Ini.GetKeys("profiles");

            if (profiles == null)
                return;

            int count = profiles.Length;

            if (count > Lv.Items.Count)
            {
                AddProfile(profiles[profiles.Length - 1]);
            }

            UpdateStatus();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FrmNewProfile frm = new FrmNewProfile(false, buffer);
            frm.ShowDialog(this);
            frm.Dispose();
            RefreshProfiles();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            buffer.ReadProfile(Lv.SelectedItems[0].Text);
            FrmNewProfile frm = new FrmNewProfile(true, buffer);
            frm.ShowDialog(this);
            frm.Dispose();
            UpdateProfile(Lv.SelectedIndices[0], Lv.SelectedItems[0].Text);
        }

        private void Lv_DoubleClick(object sender, EventArgs e)
        {
            if (Lv.SelectedItems.Count > 0)
                Edit_Click(sender, e);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Ini.DeleteKey("profiles", Lv.SelectedItems[0].Text);
            Ini.DeleteSection(Lv.SelectedItems[0].Text);
            //Refresh
            Lv.Items.RemoveAt(Lv.SelectedIndices[0]);
            UpdateStatus();
        }

        private void Lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Lv.SelectedItems.Count > 0)
            {
                Delete.Enabled = true;
                Edit.Enabled = true;
            }
            else
            {
                Delete.Enabled = false;
                Edit.Enabled = false;
            }
        }

        private void UpdateStatus()
        {
            SbItem.Text = "Profiles Number: " + Lv.Items.Count.ToString();
        }





    }
}
