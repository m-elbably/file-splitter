using System;
using QSplit.Types;
using System.Windows.Forms;
using XmlLib;
using System.IO;
using QSplit.Tools;

namespace QSplit
{
    static class SCore
    {
        private static TextBox txtFileName;
        private static ComboBox txtFolderName;
        private static LinkLabel lnkFileName;
        private static Label lFileSize;
        private static Label lPartsNumber;
        private static Label lPartSize;
        private static ComboBox cmbSize;
        private static ComboBox cmbUnit;

        public static FileSplitter Splitter;
        public static int NProfiles;
        public static bool InvalidSize = true;
        public static bool InvalidFile = true;

        public static TextBox TxtFileName
        {
            get { return SCore.txtFileName; }
            set { SCore.txtFileName = value; }
        }

        public static ComboBox TxtFolderName
        {
            get { return SCore.txtFolderName; }
            set { SCore.txtFolderName = value; }
        }

        public static ComboBox CmbSize
        {
            set { cmbSize = value; }
        }

        public static ComboBox CmbUnit
        {
            set { cmbUnit = value; }
        }

        public static Label LPartSize
        {
            set { lPartSize = value; }
        }

        public static Label LPartsNumber
        {
            set { lPartsNumber = value; }
        }

        public static LinkLabel LnkFileName
        {
            set { lnkFileName = value; }
        }

        public static Label LFileSize
        {
            set { lFileSize = value; }
        }

        public static void UpdateCmbUnit()
        {
            Profile buffer = new Profile();

            if (cmbSize.SelectedIndex == -1 || cmbSize.SelectedIndex > NProfiles + 3)
                cmbUnit.Enabled = true;
            else
            {
                if (cmbSize.SelectedIndex >= 0 && cmbSize.SelectedIndex <= 3)
                {
                    switch (cmbSize.SelectedIndex)
                    {
                        case 0:
                            cmbUnit.SelectedIndex = 0;
                            break;
                        case 1:
                            cmbUnit.SelectedIndex = 1;
                            break;
                        case 2:
                        case 3:
                            cmbUnit.SelectedIndex = 2;
                            break;
                        default:
                            break;
                    }
                }
                else if (cmbSize.SelectedIndex > 3 && cmbSize.SelectedIndex < cmbSize.Items.Count - 1)
                {
                    buffer.ReadProfile(cmbSize.Text);
                    cmbUnit.SelectedIndex = buffer.SizeIndex;
                }

                cmbUnit.Enabled = false;
            }
        }

        public static void CheckMainFile()
        {
            if (File.Exists(txtFileName.Text))
            {
                lnkFileName.Text = Path.GetFileName(txtFileName.Text);

                if (IoTools.FileOpened(txtFileName.Text))
                {
                    InvalidFile = true;
                    lFileSize.Text = "File Size: Access Denied.";
                    return;
                }

                lFileSize.Text = "File Size: " + IoTools.ParseFileSize(txtFileName.Text);
                lnkFileName.Enabled = true;
                InvalidFile = false;
            }
            else
            {
                InvalidFile = true;
                if (txtFileName.Text.Length > 0)
                    lnkFileName.Text = "File does not exists.";
                else
                    lnkFileName.Text = "No files selected.";

                lnkFileName.Enabled = false;
                lFileSize.Text = "File Size: N/A";
            }

            UpdatePartsInfo();
        }

        public static void UpdatePartsInfo()
        {
            long pSize = 0;
            if (File.Exists(txtFileName.Text))
            {
                pSize = GetPartSize();
                Splitter.FileName = txtFileName.Text;
                Splitter.SegmentSize = pSize;

                if (pSize > 0)
                {
                    lPartsNumber.Text = "Parts Number: " + Splitter.Info.Segments.ToString();
                    lPartSize.Text = "Part Size: " + IoTools.ParseSize(Splitter.SegmentSize);
                }
            }
            else
            {
                lPartsNumber.Text = "Parts Number: N/A";
                lPartSize.Text = "Part Size: N/A";
            }

        }

        public static long GetPartSize()
        {
            InvalidSize = true;
            double pSize;
            long sz = 0;
            Profile buffer = new Profile();

            if (cmbSize.SelectedIndex == 0)
                sz = 1457664;
            else if (cmbSize.SelectedIndex == 1)
                sz = 100431872;
            else if (cmbSize.SelectedIndex == 2)
                sz = 734003200;
            else if (cmbSize.SelectedIndex == 3)
                sz = 4698669056;
            else if (cmbSize.SelectedIndex > 3 && cmbSize.SelectedIndex < cmbSize.Items.Count - 1)
            {
                buffer.ReadProfile(cmbSize.Text);
                sz = buffer.Size;
                cmbUnit.SelectedIndex = buffer.SizeIndex;
            }
            else
            {
                if (double.TryParse(cmbSize.Text, out pSize))
                {
                    long fSize = (long)(pSize * Math.Pow(1024, cmbUnit.SelectedIndex));
                    if (fSize <= long.MaxValue)
                        sz = fSize;
                    else
                    {
                        sz = 0;
                        InvalidPartSize();
                    }

                    if (sz <= 0)
                    {
                        sz = 0;
                        InvalidPartSize();
                    }
                }
                else
                {
                    sz = 0;
                    InvalidPartSize();
                }

            }

            return sz;
        }

        public static void InvalidPartSize()
        {
            lPartsNumber.Text = "Parts Number: Invalid part size";
            lPartSize.Text = "Part Size: Invalid part size";
            InvalidSize = false;
        }

        public static bool CheckSplitter(FrmBase frm)
        {
            //Check if the file existed
            if (!File.Exists(txtFileName.Text))
            {
                string msg = "Invalid source file.";
                if (txtFileName.Text.Length > 0)
                    msg = "The file :" + "\n\'" + txtFileName.Text + "\' " + "does not exists.";

                frm.ShowMessage(msg, MsgType.Error);
                return false;
            }
            else
            {
                if (IoTools.FileOpened(txtFileName.Text))
                {
                    frm.ShowMessage("Source file Access Denied, file is being used by another process.", MsgType.Error);
                    return false;
                }
            }

            if (GetPartSize() <= 0)
            {
                frm.ShowMessage("Invalid part size.", MsgType.Error);
                return false;
            }

            if (!Directory.Exists(txtFolderName.Text))
            {
                string msg = "Invalid destination folder name.";
                if (txtFolderName.Text.Length > 0)
                    msg = "Destination folder:" + "\n\'" + txtFolderName.Text + "\' " + "does not exists.";

                frm.ShowMessage(msg, MsgType.Error);
                return false;
            }

            return true;
        }
    }
}
