using System;
using System.IO;
using System.Windows.Forms;
using QSplit.Tools;
using QSplit.Types;

namespace QSplit
{
    static class JCore
    {
        private static TextBox txtFileName;
        private static ComboBox txtFolderName;
        private static Label lFileName;
        private static Label lFileSize;
        private static Label lPartsNumber;
        private static CheckBox chkCrc;
        private static string sfvFile;
        private static long partSize;

        public static FileJoiner Joiner;
        public static string OutFileName;
        public static bool InvalidFile = true;


        public static TextBox TxtFileName
        {
            get { return txtFileName; }
            set { txtFileName = value; }
        }

        public static ComboBox TxtFolderName
        {
            get { return txtFolderName; }
            set { txtFolderName = value; }
        }

        public static Label LFileName
        {
            get { return JCore.lFileName; }
            set { JCore.lFileName = value; }
        }

        public static Label LPartsNumber
        {
            set { lPartsNumber = value; }
        }

        public static Label LFileSize
        {
            set { lFileSize = value; }
        }

        public static CheckBox ChkCrc
        {
            get { return JCore.chkCrc; }
            set { JCore.chkCrc = value; }
        }

        public static string SfvFile
        {
            get { return JCore.sfvFile; }
        }

        public static void CheckMainFile()
        {
            if (File.Exists(txtFileName.Text))
            {
                int pNumber = 0;

                lFileName.Text = "Part Name: " + IoTools.GetFileShortName(Path.GetFileName(txtFileName.Text), 50);

                lFileSize.Text = "Part Size: " + IoTools.ParseFileSize(txtFileName.Text);

                pNumber = GetPartsNumber();

                if (pNumber > 0)
                    lPartsNumber.Text = "Parts Number: " + pNumber.ToString();
                else
                    lPartsNumber.Text = "Parts Number: Some parts are missing";

                InvalidFile = false;
            }
            else
            {
                InvalidFile = true;
                if (txtFileName.Text.Length > 0)
                    lFileName.Text = "Part Name: File does not exists.";
                else
                    lFileName.Text = "Part Name: No files selected.";

                lFileSize.Text = "Part Size: N/A";
                lPartsNumber.Text = "Parts Number: N/A";
            }

            string fileName = GetFirstPart();
            if (fileName == null)
            {
                SetCrcChk(false);
                return;
            }

            string pDir = Directory.GetParent(txtFileName.Text).FullName;
            string fPath = Path.Combine(pDir, fileName);

            //Check if sfv file exists
            if (File.Exists(fPath + ".sfv"))
            {
                sfvFile = fPath + ".sfv";
                SetCrcChk(true);
            }
            else
            {
                SetCrcChk(false);
            }
        }

        private static void SetCrcChk(bool state)
        {
            if (state)
            {
                chkCrc.Text = "CRC-32 Check";
                chkCrc.Enabled = true;
            }
            else
            {
                sfvFile = null;
                chkCrc.Text = "CRC-32 Check (Sfv File Not Available)";
                chkCrc.Checked = false;
                chkCrc.Enabled = false;
            }
        }

        private static int GetPartsNumber()
        {
            Header hdr = new Header();
            string pDir = Directory.GetParent(txtFileName.Text).FullName;
            string fPart = GetFirstPart();
            string fileName = "";

            if (fPart != null)
                fileName = Path.Combine(pDir, fPart + ".1.qsx");
            else
                return 0;

            if (!File.Exists(fileName))
                return 0;

            if (hdr.ReadHeader(fileName))
            {
                int pNum = int.Parse(hdr.HeaderBlock.Rows[0]); //Parts number

                return pNum;
            }

            return 0;
        }

        public static bool LoadParts(FrmBase frm)
        {
            string fileName = GetFirstPart();
            if (fileName == null)
            {
                frm.ShowMessage("Invalid source file name, Can not find first part.", MsgType.Error);
                return false;
            }

            string pDir = Directory.GetParent(txtFileName.Text).FullName;
            OutFileName = Path.Combine(pDir, fileName);

            string fName = fileName + ".1.qsx"; //First part
            string fPath = Path.Combine(pDir, fName);
            partSize = IoTools.GetFileSize(fPath);

            if (partSize == 0)
            {
                frm.ShowMessage("File Access Denied, file is being used by another process.", MsgType.Error);
                return false;
            }

            Header hdr = new Header();

            if (!File.Exists(fPath))
            {
                frm.ShowMessage("Source file: '" + fPath + "' does not exists.", MsgType.Error);
                return false;
            }

            if (IoTools.FileOpened(fPath))
            {
                frm.ShowMessage("File Access Denied, file is being used by another process.", MsgType.Error);
                return false;
            }

            if (hdr.ReadHeader(fPath))
            {
                int pNum = int.Parse(hdr.HeaderBlock.Rows[0]); //Parts number
                string[] files = new string[pNum];
                for (int i = 0; i < pNum; i++)
                {
                    string cFile = Path.Combine(pDir, hdr.HeaderBlock.Rows[i + 1]);
                    if (File.Exists(cFile))
                    {
                        files[i] = cFile;
                    }
                    else
                    {
                        //File 'cFile' Does not exists
                        frm.ShowMessage("Source file: '" + cFile + "' is missing.", MsgType.Error);
                        return false;
                    }
                }

                Joiner.FileNames = files;
                
                if (hdr.HeaderBlock.Rows.Count > pNum + 1)
                {
                    Joiner.OutputSize = long.Parse(hdr.HeaderBlock.Rows[pNum + 1]);
                }
            }
            else
            {
                frm.ShowMessage("'" + fPath + "' is not a valid source file.", MsgType.Error);
                return false;
            }

            return true;
        }

        public static long GetPartSize()
        {
            return partSize;
        }

        private static string GetFirstPart()
        {
            int s, e;
            string file = Path.GetFileName(txtFileName.Text);
            e = file.LastIndexOf('.');
            s = file.Substring(0, e - 1).LastIndexOf('.');

            if (s <= 0)
                return null;

            string fileName = file.Substring(0, s);

            return fileName;
        }

        private static void CheckSfv()
        {

        }

        public static bool CheckJoiner(FrmBase frm)
        {
            //Check if the file existed
            if (!File.Exists(txtFileName.Text))
            {
                string msg = "Invalid source file.";
                if (txtFileName.Text.Length > 0)
                    msg = "The source file :" + "\n\'" + txtFileName.Text + "\' " + "does not exists.";

                frm.ShowMessage(msg, MsgType.Error);
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
