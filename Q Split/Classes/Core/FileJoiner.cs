using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

namespace QSplit
{
    public class JoinerInfo
    {
        private long segmentSize;
        private int parts;

        public long SegmentSize
        {
            get { return segmentSize; }
            set { segmentSize = value; }
        }

        public int Parts
        {
            get { return parts; }
            set { parts = value; }
        }

    }

    public class FileJoiner
    {
        private int bufferSize;
        private int progress;
        private int subProgress;
        private string[] fileNames;
        private string outputFileName;
        private long outputSize;

        private string outputFolder;
        private int segmentSize;

        private int currentPart;
        private string currentFile;

        private Thread mth;
        private long fileSize;
        private string joinMessage;
        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }
        private ThreadPriority priority;
        ManualResetEvent pauseEvent;
        ManualResetEvent stopEvent;
        bool iBreak = false;

        public ThreadPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        JoinerInfo info;

        public FileJoiner()
        {
            info = new JoinerInfo();
        }

        #region Properties
        public string OutputFileName
        {
            get { return outputFileName; }
            set { outputFileName = value; }
        }

        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        public int Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        public int SubProgress
        {
            get { return subProgress; }
            set { subProgress = value; }
        }

        public string[] FileNames
        {
            get { return fileNames; }
            set
            {
                fileNames = value;
                ReadInfo();
            }
        }

        public string OutputFolder
        {
            get { return outputFolder; }
            set { outputFolder = value; }
        }

        public long OutputSize
        {
            get { return outputSize; }
            set { outputSize = value; }
        }

        public int SegmentSize
        {
            get { return segmentSize; }
            set
            {
                segmentSize = value;
                UpdateInfo();
            }
        }

        public int CurrentPart
        {
            get { return currentPart; }
            set { currentPart = value; }
        }

        public string CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; }
        }

        public string ErrorMessage
        {
            get { return joinMessage; }
            set { joinMessage = value; }
        }

        public JoinerInfo Info
        {
            get { return info; }
            set { info = value; }
        }
        #endregion

        private void ReadInfo()
        {
            //FileStream fsr = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //info.FileSize = fsr.Length;

            //if (segmentSize == 0)
            //    return;

            //info.SegmentSize = segmentSize;
            //info.Parts = (int)(fsr.Length / segmentSize + 0.5f);
        }

        private void UpdateInfo()
        {
            //float buffer = 0f;
            //info.SegmentSize = segmentSize;
            //buffer = (float)info.FileSize / segmentSize;
            //info.Segments = (int)Math.Round(buffer + 0.5f);
        }

        public bool Join()
        {
            if (mth != null)
            {
                if (mth.IsAlive)
                {
                    return false;
                }
            }

            isRunning = true;
            joinMessage = null;

            stopEvent = new ManualResetEvent(false);
            pauseEvent = new ManualResetEvent(true);

            mth = new Thread(new ThreadStart(JoinProcess));
            mth.Priority = priority;
            mth.Start();

            return true;
        }

        private void JoinProcess()
        {
            FileStream fsReader = null;
            BinaryReader bReader = null;

            FileStream fsWriter = null;
            BinaryWriter bWriter = null;


            iBreak = false;
            Header hdr = new Header();

            long fSize = 0;
            long totalLen = 0;
            byte[] buffer = new byte[bufferSize];
            GCHandle gc = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            int cnt = 0;
            int bSize = 0;


            try
            {
                string outFile = Path.Combine(outputFolder, Path.GetFileName(outputFileName));
                fsWriter = new FileStream(outFile, FileMode.Create);
                bWriter = new BinaryWriter(fsWriter);

                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (iBreak)
                        break;

                    if (!File.Exists(fileNames[i]))
                    {
                        joinMessage = "Source file: '" + fileNames[i] + "' does not exists,\nJoin process aborted.";
                        return;
                    }

                    fsReader = new FileStream(fileNames[i], FileMode.Open, FileAccess.Read);
                    bReader = new BinaryReader(fsReader);

                    subProgress = 0;
                    currentFile = Path.GetFileName(fileNames[i]);
                    currentPart = i + 1;
                    fSize = fsReader.Length;

                    if (i == 0)
                    {
                        hdr.GetHeaderLength(fileNames[i], fsReader);
                    }

                    while (fsReader.Position != fSize)
                    {
                        pauseEvent.WaitOne(Timeout.Infinite, false);

                        if (stopEvent.WaitOne(0, true))
                        {
                            iBreak = true;
                            break;
                        }

                        if (bufferSize > fsReader.Length - fsReader.Position)
                        {
                            bSize = (int)(fsReader.Length - fsReader.Position);
                        }
                        else
                        {
                            bSize = bufferSize;
                        }

                        bReader.Read(buffer, 0, bSize);
                        bWriter.Write(buffer, 0, bSize);

                        subProgress = GetPecent(fsReader.Position, fSize);
                        totalLen += bSize;

                        if (outputSize > 0)
                            progress = GetPecent(totalLen, outputSize);
                        else
                            progress = GetPecent(cnt, fileNames.Length);
                    }

                    subProgress = 100;

                    cnt++;
                    //just for compatibility with version one that does not
                    //have main file length in the header

                    fsReader.Close();
                    bReader.Close();
                }

                buffer = null;
                gc.Free();
                GC.Collect();

                fsWriter.Close();
                bWriter.Close();

                fsReader.Dispose();
                fsWriter.Dispose();

                isRunning = false;
            }
            catch (IOException ex)
            {
                buffer = null;
                gc.Free();
                GC.Collect();

                if (fsWriter != null)
                {
                    fsWriter.Close();
                    fsWriter.Dispose();
                }

                if (bWriter != null)
                    bWriter.Close();

                if (fsReader != null)
                {
                    fsReader.Close();
                    fsReader.Dispose();
                }

                if (bReader != null)
                    bReader.Close();

                joinMessage = ex.Message;

                isRunning = false;
            }

            subProgress = 0;
            progress = 0;
        }

        private int GetPecent(long value, long max)
        {
            float result = 0f;
            result = ((float)value / (float)max) * 100;
            return (int)result;
        }

        private void ResetValues()
        {
            progress = 0;
            subProgress = 0;
        }

        public bool Finished
        {
            get
            {
                if (joinMessage == null && !isRunning)
                    return true;
                else
                    return false;
            }
        }

        public void Pause()
        {
            pauseEvent.Reset();
        }

        public void Resume()
        {
            pauseEvent.Set();
        }

        public void Stop()
        {
            isRunning = false;
            stopEvent.Set();
            pauseEvent.Set();

            mth.Join();
        }

        public void Dispose()
        {
            if (mth != null)
            {
                if (mth.IsAlive)
                {
                    mth.Abort();
                }
            }
        }

    }
}
