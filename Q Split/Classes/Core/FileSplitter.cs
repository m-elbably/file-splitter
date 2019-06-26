using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using QSplit.Tools;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Windows.Forms;

namespace QSplit
{
    public class SplitterInfo
    {
        private long fileSize;
        private long segmentSize;
        private int segments;

        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public long SegmentSize
        {
            get { return segmentSize; }
            set { segmentSize = value; }
        }

        public int Segments
        {
            get { return segments; }
            set { segments = value; }
        }

    }

    public class FileSplitter
    {
        private int bufferSize;
        private int progress;
        private int subProgress;

        private string fileName;
        private string outputFolder;
        private List<string> files = new List<string>();
        private List<long> pSizes = new List<long>();
        private bool includeSfx;

        private long segmentSize;

        private Thread currentThread;
        private long fileSize;
        private string splitMessage;

        private ThreadPriority priority;

        ManualResetEvent pauseEvent;
        ManualResetEvent stopEvent;
        bool iBreak = false;

        public ThreadPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        //Info
        private SplitterInfo info;

        private bool isRunning;

        private string currentFile;
        private int currentPart;


        public FileSplitter()
        {
            info = new SplitterInfo();
        }

        #region Properties

        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                GetSplitterInfo();
            }
        }

        public string OutputFolder
        {
            get { return outputFolder; }
            set { outputFolder = value; }
        }

        public bool IncludeSfx
        {
            get { return includeSfx; }
            set { includeSfx = value; }
        }

        public long SegmentSize
        {
            get { return segmentSize; }
            set
            {
                segmentSize = value;
                UpdateInfo();
            }
        }

        //Properties to return information
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

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        public string CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; }
        }

        public int CurrentPart
        {
            get { return currentPart; }
            set { currentPart = value; }
        }

        public string ErrorMessage
        {
            get { return splitMessage; }
            set { splitMessage = value; }
        }

        public SplitterInfo Info
        {
            get { return info; }
            set { info = value; }
        }

        /// <summary>
        /// Generated Files
        /// </summary>
        public List<string> Files
        {
            get { return files; }
            set { files = value; }
        }
        #endregion

        private void GetSplitterInfo()
        {
            info.FileSize = IoTools.GetFileSize(fileName);

            if (segmentSize == 0)
                return;

            info.SegmentSize = segmentSize;
            info.Segments = (int)(info.FileSize / segmentSize + 0.5f);

            UpdateSegments();
        }

        private void UpdateInfo()
        {
            float buffer = 0f;
            info.SegmentSize = segmentSize;
            buffer = (float)info.FileSize / segmentSize;
            info.Segments = (int)Math.Round(buffer + 0.5f);

            UpdateSegments();
        }

        private void UpdateSegments()
        {
            if (segmentSize == 0)
                return;

            long HdrLength = 0;
            for (int i = 0; i < info.Segments; i++)
            {
                HdrLength += GetFileName(i + 1).Length + 1;
            }

            HdrLength += info.Segments.ToString().Length + HdrLength.ToString().Length + Header.Signature.Length + 1;

            if (HdrLength >= segmentSize)
            {
                info.Segments++;
            }
            else
            {
                double buffer = 0f;
                HdrLength += GetFileName(info.Segments + 1).Length;
                buffer = ((double)(info.FileSize + HdrLength) / segmentSize);
                info.Segments = (int)Math.Round(buffer + 0.5f);
            }
        }

        private string GetFileName(int index)
        {
            string buffer = Path.GetFileName(fileName) + "." + index.ToString() + ".qsx";
            return buffer;
        }

        public bool Split()
        {
            if (currentThread != null)
            {
                if (currentThread.IsAlive)
                {
                    return false;
                }
            }

            isRunning = true;
            splitMessage = null;

            stopEvent = new ManualResetEvent(false);
            pauseEvent = new ManualResetEvent(true);

            currentThread = new Thread(new ThreadStart(SpltProcess));
            currentThread.Priority = priority;
            currentThread.Start();

            return true;
        }

        private void SpltProcess()
        {
            //io main objects
            FileStream fsReader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bReader = new BinaryReader(fsReader);
            FileStream fsWriter = null;
            BinaryWriter bWriter = null;

            iBreak = false;
            Header hdr = new Header();

            int count = 0;
            long bSize = 0;
            string fileStr = "";
            files.Clear();
            long realSegmentSize;
            long segSize = 0;
            byte[] buffer = new byte[bufferSize];
            GCHandle gc = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            fileSize = fsReader.Length;

            if (segmentSize >= fileSize)
            {
                realSegmentSize = fileSize;
            }
            else
            {
                realSegmentSize = segmentSize;
            }

            InitValues();

            bool fBPass = false; //first buffer pass
            long hLen = 0; //Header block length
            hdr.Add(info.Segments.ToString());
            for (int i = 0; i < info.Segments; i++)
            {
                hdr.Add(GetFileName(i + 1));
            }

            hdr.Add(fileSize.ToString());

            hLen = hdr.Length;

            try
            {
                while (fsReader.Position != fileSize)
                {
                    if (iBreak)
                        break;

                    subProgress = 0;
                    currentFile = GetFileName(count + 1);
                    currentPart = count + 1;
                    fileStr = IoTools.FormatPath(outputFolder) + currentFile;
                    files.Add(fileStr);

                    fsWriter = new FileStream(fileStr, FileMode.Create);
                    bWriter = new BinaryWriter(fsWriter);


                    if (currentPart == 1)
                    {
                        hdr.Write(bWriter);
                        segSize = hLen;
                    }
                    else
                        segSize = 0;

                    //Just for the last part for accuracy
                    if (fileSize - fsReader.Position < segmentSize)
                    {
                        realSegmentSize = fileSize - fsReader.Position;
                    }

                    while (fsWriter.Position < segmentSize && fsReader.Position != fsReader.Length)
                    {
                        pauseEvent.WaitOne(Timeout.Infinite, false);

                        if (stopEvent.WaitOne(0, true))
                        {
                            iBreak = true;
                            break;
                        }

                        if (bufferSize > segmentSize)
                        {
                            if (fsReader.Length - fsReader.Position < segmentSize)
                                bSize = (int)(fsReader.Length - fsReader.Position);
                            else
                                bSize = segmentSize;

                        }
                        else
                        {
                            if (fsReader.Length - fsReader.Position < bufferSize)
                            {
                                if (fsReader.Length - fsReader.Position < segmentSize - fsWriter.Position)
                                    bSize = (int)(fsReader.Length - fsReader.Position);
                                else
                                    bSize = segmentSize - fsWriter.Position;
                            }
                            else
                            {
                                if (fsWriter.Position + bufferSize < segmentSize)
                                    bSize = bufferSize;
                                else
                                    bSize = segmentSize - ((int)fsWriter.Position);
                            }
                        }

                        if (currentPart == 1 && !fBPass)
                        {
                            if (bSize > hLen)
                                bSize -= hLen;

                            fBPass = true;
                        }

                        bReader.Read(buffer, 0, (int)bSize);
                        bWriter.Write(buffer, 0, (int)bSize);

                        subProgress = GetPecent(fsWriter.Position, realSegmentSize);
                        progress = GetPecent(fsReader.Position, fileSize);

                        segSize += bSize;
                    }

                    pSizes.Add(segSize);
                    subProgress = 100;

                    fsWriter.Close();
                    bWriter.Close();

                    count++;
                }

                subProgress = 0;
                progress = 0;

                buffer = null;
                gc.Free();
                GC.Collect();

                fsReader.Close();
                bReader.Close();

                GenerateSfx();

                isRunning = false;
            }
            catch (IOException ex)
            {
                buffer = null;
                //gc.Free();
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

                splitMessage = ex.Message;

                isRunning = false;
            }
        }

        private int GetPecent(long value, long max)
        {
            double result = 0;
            result = ((double)value / (double)max) * 100;
            return (int)result;
        }

        //0- Original File Name
        //1- Original File Size
        //2- First Part Name
        //3- Parts Number
        //4- {
        //   P% Name
        //   P% Size
        //   }
        private void GenerateSfx()
        {
            if (!includeSfx)
                return;

            Header hdr = new Header();

            //Self Joiner Signature
            hdr.Add(Path.GetFileName(fileName));
            hdr.Add(fileSize.ToString());
            hdr.Add(Path.GetFileName(files[0]));
            hdr.Add(files.Count.ToString());

            for (int i = 0; i < files.Count; i++)
            {
                hdr.Add(Path.GetFileName(files[i]));
                hdr.Add(pSizes[i].ToString());
            }

            if (Sfx.OutPath.Length > 0)
                hdr.Add(Sfx.OutPath);
            else
                hdr.Add("\0");

            if(Sfx.Comment.Length > 0)
                hdr.Add(Sfx.Comment);
            else
                hdr.Add("\0");

            string sfxPath = Path.Combine(outputFolder, "Q Join.exe");
            File.Copy(Path.Combine(Application.StartupPath,"qj.bin"), sfxPath,true);
            long sfxSize = IoTools.GetFileSize(sfxPath);

            FileStream fs = new FileStream(sfxPath, FileMode.Append);
            BinaryWriter br = new BinaryWriter(fs);

            br.Seek(0, SeekOrigin.End);
            hdr.Write(br);

            fs.Close();
            br.Close();

            fs.Dispose();
        }

        private void InitValues()
        {
            progress = 0;
            subProgress = 0;
        }

        public bool Finished
        {
            get
            {
                if (splitMessage == null && !isRunning)
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

            currentThread.Join();
        }

        public void Dispose()
        {
            if (currentThread != null)
            {
                if (currentThread.IsAlive)
                {
                    currentThread.Abort();
                }
            }

        }
    }


}
