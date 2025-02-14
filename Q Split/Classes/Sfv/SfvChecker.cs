﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace QSplit.Tools
{
    static class SfvChecker
    {
        private static bool isRunning;
        private static int finished;
        private static string fileName;
        private static int filesCount;
        private static bool createLog;
        private static List<string> files = new List<string>();
        private static List<string> filesCrc = new List<string>();
        private static List<string> msg = new List<string>();

        private static bool hasError = false;

        public static bool CreateLog
        {
            set { createLog = value; }
        }
 
        public static bool HasError
        {
            get { return hasError; }
        }

        public static int Finished
        {
            get { return finished; }
        }

        public static int FilesCount
        {
            get { return filesCount; }
        }

        public static List<string> Files
        {
            get { return files; }
            set { files = value; }
        }

        public static bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        public static string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public static void CheckSfv()
        {
            isRunning = true;
            hasError = false;
            finished = 0;
            Thread th = new Thread(new ThreadStart(CheckHash));
            th.Priority = ThreadPriority.Highest;
            th.Start();
        }

        private static void CheckHash()
        {
            if (fileName == null)
                return;

            if (!File.Exists(fileName))
                return;

            string pDir = Path.GetDirectoryName(fileName);
            CRC32 crc = new CRC32();
            StreamReader sR = new StreamReader(fileName);
            string fCrc32 = "";
            string sLine = "";
            int sIndex = 0;

            while (!sR.EndOfStream)
            {
                sLine = sR.ReadLine();

                if (sLine.TrimStart(' ').StartsWith(";"))
                    continue;

                sIndex = sLine.LastIndexOf(' ');

                if (sIndex > 0)
                {
                    files.Add(sLine.Substring(0, sIndex));
                    filesCrc.Add(sLine.Substring(sIndex + 1, sLine.Length - sIndex - 1));
                }
            }

            filesCount = files.Count;

            if (filesCount <= 0)
            {
                return;
            }

            for (int i = 0; i < files.Count; i++)
            {
                string filePath = Path.Combine(pDir, files[i]);
                if (File.Exists(filePath))
                {
                    FileStream f = new FileStream(filePath, FileMode.Open,
                    FileAccess.Read, FileShare.Read, 8192);

                    fCrc32 = crc.GetCrc32(f).ToString("X");

                    if (fCrc32 == filesCrc[i])
                    {
                        msg.Add(files[i] + "        Correct.");
                    }
                    else
                    {
                        hasError = true;
                        msg.Add(files[i] + "        Does not match.");
                    }

                    f.Close();
                    f.Dispose();
                }
                else
                {
                    hasError = true;
                    msg.Add(files[i] + "        File does not exist.");
                }

                Interlocked.Increment(ref finished);
            }

            if (hasError && createLog)
            {
                string logPath = fileName + ".log";

                try
                {
                    if (File.Exists(logPath))
                        File.Delete(logPath);

                    StreamWriter sW = new StreamWriter(fileName + ".log");
                    sW.WriteLine("Generated by MMH Split on " + DateTime.Now.ToString());
                    sW.WriteLine("========================");
                    sW.WriteLine("File" + Space(files[0].Length) + "    Crc Check Result");
                    sW.WriteLine("=====" + Space(files[0].Length) + "   ================");
                    sW.WriteLine("");

                    for (int i = 0; i < msg.Count; i++)
                    {
                        sW.WriteLine(msg[i]);
                    }

                    sW.Close();
                    sW.Dispose();
                }
                catch (IOException)
                {
                    //Nothing
                }
            }

            sR.Close();
            sR.Dispose();
            isRunning = false;
        }

        private static string Space(int n)
        {
            StringBuilder s = new StringBuilder(n);
            for (int i = 0; i < n; i++)
            {
                s.Append(' ');
            }

            return s.ToString();
        }
    }
}
