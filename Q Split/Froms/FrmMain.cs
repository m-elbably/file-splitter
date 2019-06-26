using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QSplit.Core;
using QSplit.Properties;
using System.Threading;
using QSplit.Controls;
using Microsoft.Win32;
using QSplit.Tools;
using QSplit.Windows;

namespace QSplit
{
    public enum QProcess
    {
        None,
        Split,
        Join
    }

    public enum MsgType
    {
        Normal,
        Caution,
        Error
    }

    public partial class FrmMain : FrmBase
    {
        //Main Var's
        FileSplitter fSplit = new FileSplitter();
        FileJoiner fJoin = new FileJoiner();

        /// <summary>
        /// 0 = Split Process
        /// 1 = Join Process
        /// </summary>
        short currentProc = 0;
        bool pauseState = false;
        bool stoped = false;

        bool crcCheckStatus = false;
        Size mSize = Size.Empty;

        //Options Var's
        short OLstIndex = -1;

        short shutDownCnt = 30;
        bool startupFlag = true;

        long oTick = 0;
        long eTime = 0;


        private int nProfiles;

        public FrmMain(QProcess cP)
        {
            InitializeComponent();
            int hFactor = GetCaptionHeight() - 20;
            hFactor = hFactor > 0 ? hFactor : 0;

            this.Height = 346 + hFactor;

            LoadData();

            switch (cP)
            {
                case QProcess.None:
                    break;
                case QProcess.Split:

                    MainTab.Visible = false;

                    SpBtnFileBrowse.Visible = false;
                    SpTxtFile.Width = 478;
                    SpTxtFile.BackColor = SystemColors.Control;
                    SpTxtFile.Text = App.CurrentFile;
                    SpTxtFile.ReadOnly = true;

                    SpPanel.Parent = this;
                    SpPanel.Location = new Point(12, 9);

                    this.Size = new Size(510, 290 + hFactor);

                    break;
                case QProcess.Join:
                    MainTab.Visible = false;

                    JnBtnFileBrowse.Visible = false;
                    JnTxtFile.Width = 478;
                    JnTxtFile.BackColor = SystemColors.Control;
                    JnTxtFile.Text = App.CurrentFile;
                    JnTxtFile.ReadOnly = true;

                    JnPanel.Parent = this;
                    JnPanel.Location = new Point(12, 9);

                    this.Size = new Size(510, 290 + hFactor);

                    break;
                default:
                    break;
            }

            mSize = this.Size;
        }

        private void FrmBase_Load(object sender, EventArgs e)
        {
            //LoadData();
        }

        #region Main Form...

        private void LoadData()
        {

            //Update Core Split Engine
            SCore.Splitter = fSplit;
            SCore.TxtFileName = SpTxtFile;
            SCore.TxtFolderName = SpTxtFolder;
            SCore.CmbSize = CmbSize;
            SCore.CmbUnit = CmbUnit;
            SCore.LnkFileName = SpLnkFileName;
            SCore.LFileSize = SpLFileSize;
            SCore.LPartsNumber = SpLPartsNumber;
            SCore.LPartSize = SpLPartSize;

            //Update Core Join Engine
            JCore.Joiner = fJoin;
            JCore.TxtFileName = JnTxtFile;
            JCore.TxtFolderName = JnTxtFolder;
            JCore.LFileName = JnLFileName;
            JCore.LFileSize = JnLFileSize;
            JCore.LPartsNumber = JnLPartsNumber;
            JCore.ChkCrc = JnChkCrc;

            UpdateProfiles();

            //Select Unit
            CmbUnit.SelectedIndex = Ini.ReadInt("splitter", "unit", 2, 0, 4);

            SCore.CheckMainFile();

            //Load Options and set there values
            ArrangeOptions();
            LoadOptions();

            startupFlag = false;
        }

        private void UpdateProfiles()
        {
            //3.5":  1 457 664 b
            //Zip-100:   98 078 kb
            //CD-700:  700 mb
            //DVD-R:  4 481 mb
            //Custom Size

            CmbSize.Items.Clear();

            //Fill Split Profilers
            CmbSize.Items.Add("3.5\":  1 457 664 b");
            CmbSize.Items.Add("Zip-100:   98 078 kb");
            CmbSize.Items.Add("CD-700:  700 mb");
            CmbSize.Items.Add("DVD-R:  4 481 mb");

            Ini.Refresh();
            string[] cProfiles = Ini.GetKeys("profiles");

            if (cProfiles != null)
            {
                nProfiles = cProfiles.Length;
                if (cProfiles != null)
                {
                    for (int i = 0; i < nProfiles; i++)
                    {
                        CmbSize.Items.Add(cProfiles[i]);
                    }
                }

            }

            CmbSize.Items.Add("Custom Size");
            CmbSize.SelectedIndex = CmbSize.Items.Count - 1;

            SCore.NProfiles = nProfiles;
        }

        private void FrmBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOptions();

            if (fSplit.IsRunning || fJoin.IsRunning)
            {
                BtnStop_Click(sender, new EventArgs());
            }

            fJoin.Dispose();
            fSplit.Dispose();
        }

        public override void ShowMessage(string message, MsgType mType)
        {
            //Hide other items on the layer
            RunPanel.Visible = false;

            MsgPanel.Parent = BackLayer;

            MsgPanel.Left = (mSize.Width - MsgPanel.Width) / 2;
            MsgPanel.Top = (mSize.Height - MsgPanel.Height) / 2;

            MsgPanelText.Text = message;
            BtnMsgOk.Select();
            this.AcceptButton = BtnMsgOk;

            switch (mType)
            {
                case MsgType.Normal:
                    MsgPanel.ShadowColor = Color.Chartreuse;
                    MsgPic.Image = Resources.Success;
                    break;
                case MsgType.Caution:
                    MsgPanel.ShadowColor = Color.Orange;
                    MsgPic.Image = Resources.Warning;
                    break;
                case MsgType.Error:
                    MsgPanel.ShadowColor = Color.Red;
                    MsgPic.Image = Resources.ErrLarge;
                    break;
                default:
                    break;
            }

            MsgPanel.Visible = true;
            this.Refresh();
            BackLayer.Show();
            this.Refresh();
        }

        private void BtnMsgOk_Click(object sender, EventArgs e)
        {
            if (PicElapsed.Visible)
            {
                PicElapsed.Visible = false;
                LbElapsed.Visible = false;
            }

            BackLayer.Hide();
        }

        private void BrowseFile(TextBox tBox, string filter)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filter; //"All Files (*.*)|*.*";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (dlg.FileName != null)
                    tBox.Text = dlg.FileName;
            }

            dlg.Dispose();
        }

        private void BrowseFolder(ComboBox cmb)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Browse for output folder.";
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (dlg.SelectedPath != null)
                    cmb.Text = dlg.SelectedPath;
            }

            dlg.Dispose();
        }



        private int GetSelectedBuffer()
        {
            switch (OpCmbBuffer.SelectedIndex)
            {
                case 1: //4 KB
                    return 4096;
                case 2: //8 KB
                    return 8192;
                case 3: //16 KB
                    return 16384;
                case 4: //32 KB
                    return 32768;
                case 5: //4 MB
                    return 4194304;
                case 6: //8 MB
                    return 8388608;
                case 7: //16 MB
                    return 16777216;
                case 8: //32 MB
                    return 33554432;
                default:
                    return 4096;
            }
        }

        private int GetBestBuffer(long partSize)
        {
            if (partSize > 102400 && partSize < 10485760) // > 100 KB && < 10 MB
                return 32768; //32 KB
            else if (partSize > 10485760 && partSize < 33554432) // > 10 MB && < 32 MB
                return 4194304; //4 MB
            else if (partSize > 33554432 && partSize < 104857600) // > 32 MB && < 100 MB
                return 16777216; //16 MB
            else if (partSize > 104857600) // > 100 MB && < 1024 MB
                return 33554432; //32 MB
            else  // < 100 KB
                return 4096;  //4 KB
        }

        private ThreadPriority GetPriority()
        {
            switch (OpCmbPriority.SelectedIndex)
            {
                case 0:
                    return ThreadPriority.Highest;
                case 1:
                    return ThreadPriority.AboveNormal;
                case 3:
                    return ThreadPriority.BelowNormal;
                case 4:
                    return ThreadPriority.Lowest;
                default:
                    return ThreadPriority.Normal;
            }
        }

        private void ShowPanel(PanelEx panel, Button btnFocus)
        {
            panel.Parent = BackLayer;
            panel.Left = (mSize.Width - panel.Width) / 2;
            panel.Top = (mSize.Height - panel.Height) / 2 - 18;
            panel.Visible = true;
            BackLayer.Show();
            btnFocus.Focus();
            this.Refresh();
        }

        private void ResetVars()
        {
            stoped = false;
            BtnPause.Enabled = true;
            BtnStop.Enabled = true;
        }

        #endregion

        #region Split Tab...

        private void BtnSplit_Click(object sender, EventArgs e)
        {
            ResetVars();

            if (!SCore.CheckSplitter(this))
                return;

            //BackLayer.Show();
            long pSize;
            currentProc = 0;

            ProgBar.Value = 0;
            fSplit.FileName = SpTxtFile.Text;
            fSplit.OutputFolder = SpTxtFolder.Text;
            fSplit.Priority = GetPriority();
            fSplit.IncludeSfx = ChkSfx.Checked;

            if (OpCmbBuffer.SelectedIndex == 0)
            {
                pSize = SCore.GetPartSize();
                if (pSize > 0)
                    fSplit.BufferSize = GetBestBuffer(pSize);
            }
            else
            {
                fSplit.BufferSize = GetSelectedBuffer();
            }

            ShowRunPanel();
            //Calc time
            StartTime();

            fSplit.Split();
            Tmr.Enabled = true;
        }

        private void SpBtnFileBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(SpTxtFile, "All Files (*.*)|*.*");
        }

        private void SpBtnFolderBrowse_Click(object sender, EventArgs e)
        {
            BrowseFolder(SpTxtFolder);
        }

        private void LnkFileName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!File.Exists(SpTxtFile.Text))
                {
                    ShowMessage("File does not exists.", MsgType.Error);
                    return;
                }

                try
                {
                    Process.Start(SpTxtFile.Text);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private void ShowRunPanel()
        {
            //Hide other items on the layer
            HintPanel.Visible = false;
            MsgPanel.Visible = false;
            this.Refresh();
            ShowPanel(RunPanel, BtnStop);
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            if (currentProc == 0)
            {
                LRunText.Text = "Spliting File...";
                SubProgBar.Value = fSplit.SubProgress;
                ProgBar.Value = fSplit.Progress;
                LbCurrentFile.Text = "Part " + fSplit.CurrentPart.ToString();
                LbPart.Text = fSplit.SubProgress + "%";
                LbOverAll.Text = fSplit.Progress + "%";
                LbParts.Text = (fSplit.CurrentPart == 0 ? 0 : fSplit.CurrentPart - 1).ToString() + " Part(s) Completed.";

                if (fSplit.ErrorMessage != null)
                {
                    BackLayer.Hide();
                    RunPanel.Visible = false;
                    this.Refresh();
                    ShowMessage(fSplit.ErrorMessage, MsgType.Error);

                    Tmr.Enabled = false;
                }

                if (fSplit.Finished)
                {
                    RunPanel.Visible = false;
                    this.Refresh();

                    if (!OpChkTurnOff.Checked &&
                        !OpChkSfv.Checked && !stoped)
                    {
                        ShowMessage("Successfully done.", MsgType.Normal);
                        DisplayTime();
                    }

                    if (!stoped)
                    {
                        CheckSfvGeneration();
                        CheckDeleteOriginal();
                    }

                    if (!OpChkSfv.Checked && !stoped)
                        CheckShutDown();

                    Tmr.Enabled = false;
                }

            }
            else if (currentProc == 1)
            {
                LRunText.Text = "Joining Files...";
                SubProgBar.Value = fJoin.SubProgress;
                ProgBar.Value = fJoin.Progress;
                LbCurrentFile.Text = "Part " + fJoin.CurrentPart.ToString();
                LbPart.Text = fJoin.SubProgress + "%";
                LbOverAll.Text = fJoin.Progress + "%";
                LbParts.Text = (fJoin.CurrentPart == 0 ? 0 : fJoin.CurrentPart - 1).ToString() + " Part(s) Completed.";

                if (fJoin.ErrorMessage != null)
                {
                    BackLayer.Hide();
                    RunPanel.Visible = false;
                    this.Refresh();
                    ShowMessage(fJoin.ErrorMessage, MsgType.Error);
                    Tmr.Enabled = false;
                }

                if (fJoin.Finished)
                {
                    RunPanel.Visible = false;
                    this.Refresh();

                    if (!OpChkDeleteFiles.Checked && !stoped
                        && !OpChkTurnOff.Checked)
                    {
                        ShowMessage("Successfully done.", MsgType.Normal);
                        DisplayTime();
                    }

                    if (!stoped)
                        CheckFilesDelete();

                    if (!stoped)
                        CheckShutDown();

                    Tmr.Enabled = false;
                }
            }

        }

        private void DisplayTime()
        {
            CalcTime();
            LbElapsed.Text = "Elapsed Time = " + GetTime(eTime);

            PicElapsed.Visible = true;
            LbElapsed.Visible = true;
        }

        private string GetTime(long tm)
        {
            float fm = 0;
            if (tm <= 1000)
            {
                fm = (float)tm / 1000;
                return (fm).ToString() + " Second.";
            }
            else if (tm > 1000 && tm <= 60000)
                return (tm / 1000).ToString() + " Seconds.";
            else
                return (tm / 1000 / 60).ToString() + " Min, " + ((tm / 1000) % 60).ToString() + " Sec.";
        }

        private void SpTxtFile_TextChanged(object sender, EventArgs e)
        {
            SCore.CheckMainFile();
            if (!SCore.InvalidFile)
            {
                string path = Directory.GetParent(SpTxtFile.Text).FullName;
                if (!SpTxtFolder.Items.Contains(path))
                    SpTxtFolder.Items.Add(path);
            }
        }

        private void CmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SCore.UpdateCmbUnit();
            SCore.UpdatePartsInfo();
        }

        private void CmbSize_TextChanged(object sender, EventArgs e)
        {
            SCore.UpdateCmbUnit();
            SCore.UpdatePartsInfo();
        }

        private void CmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            SCore.UpdatePartsInfo();
        }



        #endregion

        #region Join Tab...

        private void JnTxtFile_TextChanged(object sender, EventArgs e)
        {
            JCore.CheckMainFile();
            if (!JCore.InvalidFile)
            {
                string path = Directory.GetParent(JnTxtFile.Text).FullName;
                if (!JnTxtFolder.Items.Contains(path))
                    JnTxtFolder.Items.Add(path);
            }

        }

        private void JnBtnFileBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(JnTxtFile, "QSplit Files (*.qsx)|*.qsx");
        }

        private void JnBtnFolderBrowse_Click(object sender, EventArgs e)
        {
            BrowseFolder(JnTxtFolder);
        }

        #endregion

        #region Options Tab...

        private void OptionsLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOptionPanel(OptionsLst.SelectedIndex);
        }

        private void LnkEditProfiles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FrmProfiles frm = new FrmProfiles();
                frm.ShowDialog(this);
                frm.Dispose();

                UpdateProfiles();
            }
        }


        private void ArrangeOptions()
        {
            Options_PGeneral.Parent = Options_Panel;
            Options_PSplit.Parent = Options_Panel;
            Options_PJoin.Parent = Options_Panel;

            Options_PGeneral.Location = new Point(0, 0);
            Options_PGeneral.Size = Options_Panel.Size;
            Options_PSplit.Location = new Point(0, 0);
            Options_PSplit.Size = Options_Panel.Size;
            Options_PJoin.Location = new Point(0, 0);
            Options_PJoin.Size = Options_Panel.Size;
        }

        private void LoadOptionPanel(int index)
        {
            if (OLstIndex == OptionsLst.SelectedIndex)
                return;

            Options_PGeneral.Visible = false;
            Options_PSplit.Visible = false;
            Options_PJoin.Visible = false;

            switch (index)
            {
                case 0:
                    Options_PGeneral.Visible = true;
                    break;
                case 1:
                    Options_PSplit.Visible = true;
                    break;
                case 2:
                    Options_PJoin.Visible = true;
                    break;
                default:
                    break;
            }

            OLstIndex = (short)OptionsLst.SelectedIndex;
        }

        private void SaveOptions()
        {
            if (App.Args != null)
                return;

            //Main item index
            Ini.Write(Ini.ISction.Options, "tab", OptionsLst.SelectedIndex);

            //Performance Panel
            Ini.Write(Ini.ISction.Options, "buffer", OpCmbBuffer.SelectedIndex);
            Ini.Write(Ini.ISction.Options, "priority", OpCmbPriority.SelectedIndex);

            Ini.Write(Ini.ISction.Options, "shell", OpChkShell.Checked);
            Ini.Write(Ini.ISction.Options, "pc", OpChkTurnOff.Checked);
            Ini.Write(Ini.ISction.Options, "sfv", OpChkSfv.Checked);

            //Splitter Panel
            Ini.Write(Ini.ISction.Options, "doriginal", OpChkDeleteOriginal.Checked);

            //Joiner Panel
            Ini.Write(Ini.ISction.Options, "dfiles", OpChkDeleteFiles.Checked);
            Ini.Write(Ini.ISction.Options, "log", OpChkLog.Checked);
            if (JnChkCrc.Enabled)
                Ini.Write(Ini.ISction.Options, "cch", JnChkCrc.Checked);
        }

        private void LoadOptions()
        {
            //Main item index
            OptionsLst.SelectedIndex = Ini.ReadInt(Ini.ISction.Options, "tab", 0, 0, 2);

            //Performance Panel
            OpCmbBuffer.SelectedIndex = Ini.ReadInt(Ini.ISction.Options, "buffer", 0, 0, 9);
            OpCmbPriority.SelectedIndex = Ini.ReadInt(Ini.ISction.Options, "priority", 2, 0, 4);

            OpChkShell.Checked = Ini.ReadBool(Ini.ISction.Options, "shell", true);
            OpChkTurnOff.Checked = Ini.ReadBool(Ini.ISction.Options, "pc", false);
            OpChkSfv.Checked = Ini.ReadBool(Ini.ISction.Options, "sfv", false);

            //Splitter Panel
            OpChkDeleteOriginal.Checked = Ini.ReadBool(Ini.ISction.Options, "doriginal", false);

            //Joiner Panel
            OpChkDeleteFiles.Checked = Ini.ReadBool(Ini.ISction.Options, "dfiles", false);
            OpChkLog.Checked = Ini.ReadBool(Ini.ISction.Options, "log", false);
        }

        private void EnableShell()
        {
            RegistryKey mKey = Registry.ClassesRoot;
            RegistryKey buffer;

            //Create Split Integration
            buffer = mKey.CreateSubKey(@"*\shell\Split");
            buffer = buffer.CreateSubKey("command");
            buffer.SetValue("", Application.ExecutablePath + " \"/split\" \"%L\"");

            //Create Join Integration
            buffer = mKey.CreateSubKey(@".qsx");
            buffer.SetValue("", "MMH Split File.");
            buffer = buffer.CreateSubKey("DefaultIcon");
            buffer.SetValue("", Api.GetShortPathName(Application.ExecutablePath) + ",1");

            buffer = mKey.CreateSubKey(@".qsx\shell");
            buffer = buffer.CreateSubKey("Join");
            buffer = buffer.CreateSubKey("command");
            buffer.SetValue("", Application.ExecutablePath + " \"/join\" \"%L\"");
        }

        private void DisableShell()
        {
            try
            {
                RegistryKey mKey = Registry.ClassesRoot;
                mKey.DeleteSubKeyTree(@"*\shell\Split");
                mKey.DeleteSubKeyTree(@".qsx");
            }
            catch (Exception)
            {
            }
        }

        private void OpChkShell_CheckedChanged(object sender, EventArgs e)
        {
            if (startupFlag)
                return;

            if (OpChkShell.Checked)
                EnableShell();
            else
                DisableShell();
        }

        private void CheckShutDown()
        {
            if (OpChkTurnOff.Checked)
            {
                RunPanel.Visible = false;
                this.Refresh();
                ShowPanel(ShutDownPanel, BtnCancel);
                TmrShutDown.Enabled = true;
            }
        }

        private void TmrShutDown_Tick(object sender, EventArgs e)
        {
            if (shutDownCnt <= 0)
            {
                LShutDownMsg.Text = "Shuting Down.";
                BtnCancel.Enabled = false;
                this.Refresh();
                Windows.Api.Shutdown();
                TmrShutDown.Enabled = false;
            }
            else
            {
                string txt = shutDownCnt > 1 ? " Seconds." : " Second.";
                LShutDownMsg.Text = "Shuting Down Windows after " + shutDownCnt.ToString() + txt;

                --shutDownCnt;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TmrShutDown.Enabled = false;
            shutDownCnt = 45;
            BackLayer.Hide();
        }

        private void CheckFilesDelete()
        {
            if (OpChkDeleteFiles.Checked)
            {
                MsgPanel.Visible = false;
                BackLayer.Hide();
                this.Refresh();
                LHint.Text = "Deleting Files...";
                ShowPanel(HintPanel, BtnCancel);
                this.Refresh();
                Thread.Sleep(500);

                for (int i = 0; i < fJoin.FileNames.Length; i++)
                {
                    File.Delete(fJoin.FileNames[i]);
                }

                Thread.Sleep(700);
                HintPanel.Visible = false;
                BackLayer.Hide();
            }
        }

        private void CheckDeleteOriginal()
        {
            if (OpChkDeleteOriginal.Checked)
                File.Delete(fSplit.FileName);
        }

        private void CheckSfvGeneration()
        {
            if (!OpChkSfv.Checked)
                return;

            RunPanel.Visible = false;
            MsgPanel.Visible = false;
            this.Refresh();
            LHint.Text = "Generating Sfv Fle...";
            ShowPanel(HintPanel, BtnCancel);
            this.Refresh();

            SfvGenerator.Files = fSplit.Files;
            SfvGenerator.OutPutFile = Path.Combine(fSplit.OutputFolder, Path.GetFileName(fSplit.FileName));
            SfvGenerator.GenerateSfv();
            TmrSfv.Enabled = true;
        }

        #endregion

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            ResetVars();

            if (!JCore.CheckJoiner(this))
                return;

            if (!JCore.LoadParts(this))
                return;

            if (JnChkCrc.Checked && JnChkCrc.Enabled && !crcCheckStatus)
            {
                CheckSfvCrc();
                crcCheckStatus = true;
                return;
            }

            if (crcCheckStatus)
                crcCheckStatus = false;

            long pSize = 0;
            currentProc = 1;

            ProgBar.Value = 0;
            fJoin.OutputFileName = JCore.OutFileName;
            fJoin.OutputFolder = JnTxtFolder.Text;
            fJoin.Priority = GetPriority();
            if (OpCmbBuffer.SelectedIndex == 0)
            {
                pSize = JCore.GetPartSize();
                if (pSize > 0)
                    fJoin.BufferSize = GetBestBuffer(pSize);
            }
            else
            {
                fJoin.BufferSize = GetSelectedBuffer();
            }

            ShowRunPanel();
            //Calc time
            StartTime();
            
            fJoin.Join();
            Tmr.Enabled = true;
        }

        private void TmrSfv_Tick(object sender, EventArgs e)
        {
            if (!SfvGenerator.IsRunning)
            {
                HintPanel.Visible = false;
                BackLayer.Hide();

                if (OpChkTurnOff.Checked)
                    CheckShutDown();
                else
                    ShowMessage("Successfully done.", MsgType.Normal);

                TmrSfv.Enabled = false;
            }
            else
            {
                float val = (float)SfvGenerator.Finished / SfvGenerator.Files.Count;
                int ret = (int)(val * 100);
                LHint.Text = "Generating Sfv File - " + ret.ToString() + "% Completed.";
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            stoped = true;
            BtnPause.Enabled = false;
            BtnStop.Enabled = false;

            this.Refresh();
            if (currentProc == 0)
                fSplit.Stop();
            else
                fJoin.Stop();

            pauseState = false;
            BtnPause.Text = "Pause";
            BackLayer.Hide();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            pauseState = !pauseState;
            if (pauseState)
            {
                PicStatus.Image = Resources.pause;
                PauseTime();
                BtnPause.Text = "Resume";
                if (currentProc == 0)
                    fSplit.Pause();
                else
                    fJoin.Pause();
            }
            else
            {
                PicStatus.Image = Resources.work;
                ResumeTime();
                BtnPause.Text = "Pause";
                if (currentProc == 0)
                    fSplit.Resume();
                else
                    fJoin.Resume();
            }

        }

        private void CheckSfvCrc()
        {
            RunPanel.Visible = false;
            MsgPanel.Visible = false;
            this.Refresh();
            LHint.Text = "Checking Sfv Fle...";
            ShowPanel(HintPanel, BtnCancel);

            SfvChecker.CreateLog = OpChkLog.Checked;
            SfvChecker.FileName = JCore.SfvFile;
            SfvChecker.CheckSfv();
            TmrCrc.Enabled = true;
        }

        private void TmrCrc_Tick(object sender, EventArgs e)
        {
            if (!SfvChecker.IsRunning)
            {
                HintPanel.Visible = false;
                BackLayer.Hide();

                if (!SfvChecker.HasError)
                    BtnJoin_Click(sender, e);
                else
                {
                    ShowMessage("Sfv check error.", MsgType.Error);
                    crcCheckStatus = false;
                }

                TmrCrc.Enabled = false;
            }
            else
            {
                float val = (float)SfvChecker.Finished / SfvChecker.FilesCount;
                int ret = (int)(val * 100);
                LHint.Text = "Checking Sfv File - " + ret.ToString() + "% Completed.";
            }
        }

        private void LnkSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/m-elbably");
            }
            catch (Exception)
            {
            }

        }

        private void BackLayer_PaneHide(object sender, EventArgs e)
        {
            MainTab.Enabled = true;
        }

        private void BackLayer_PaneShow(object sender, EventArgs e)
        {
            MainTab.Enabled = false;
        }

        private void PauseTime()
        {
            CalcTime();
        }

        private void ResumeTime()
        {
            oTick = Environment.TickCount;
        }

        private void StartTime()
        {
            eTime = 0;
            ResumeTime();
        }

        private void CalcTime()
        {
            eTime += Environment.TickCount - oTick;
        }

        private void LnkContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/m-elbably");
            }
            catch (Exception)
            {
            }
        }

        private void ChkSfx_CheckedChanged(object sender, EventArgs e)
        {
            SfxOptions.Enabled = ChkSfx.Checked;
        }

        private void SfxOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            FrmSfxOptions frm = new FrmSfxOptions();
            frm.ShowDialog(this);
            frm.Dispose();
        }


        private int GetCaptionHeight()
        {
            int SM_CYCAPTION = 4;
            return Api.GetSystemMetrics(SM_CYCAPTION);
        }

    }
}
