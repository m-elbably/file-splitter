using System;
using System.IO;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.Security;

namespace QSplit.Tools
{
    enum IoSize
    {
        Bit,
        Byte,
        KB, //kilo byte = 1024^1 byte
        MB, //mega byte = 1024^2 byte
        GB, //giga byte = 1024^3 byte
        TB, //tera byte = 1024^4 byte
        PB, //peta byte byte = 1024^5 byte
        EB, //exa byte = 1024^6 byte
        ZB, //zetta byte = 1024^7 byte
        YB  //yotta byte = 1024^8 byte
    }

    class IoTools
    {
        /// <summary>
        /// Convert long size (in bytes) to sutable string
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static string ParseSize(long sz)
        {
            int level = 0;
            double cSize = 0;
            string buffer = "";

            if (sz < 1024)
                level = 1;
            else if (sz >= 1024 && sz < 1048576)
                level = 2;
            else if (sz >= 1048576 && sz < 1073741824)
                level = 3;
            else if (sz >= 1073741824 && sz < 1099511627776)
                level = 4;
            else if (sz >= 1099511627776 && sz < 1125899906842624)
                level = 5;
            else if (sz >= 1125899906842624 && sz < 1152921504606846976)
                level = 6;
            else if (sz >= 1152921504606846976 && sz <= Math.Pow(1024, 7))
                level = 7;

            cSize = sz / Math.Pow(1024, level - 1);
            buffer = Math.Round(cSize, 2).ToString() + " " + (IoSize)level;

            return buffer;
        }

        public static string ParseFileSize(string filePath)
        {
            return ParseSize(GetFileSize(filePath));
        }

        public static long GetFileSize(string filePath)
        {
            long fileLen = 0;
            try
            {
                FileStream fs = File.OpenRead(filePath);
                fileLen = fs.Length;
                fs.Close();
            }
            catch (IOException)
            {
                return 0;
            }

            return fileLen;
        }

        public static string FormatPath(string path)
        {
            string buffer = path;
            if (!path.EndsWith("\\"))
                buffer += "\\";

            return buffer;
        }

        public static bool FileOpened(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                fs.Dispose();
            }
            catch (IOException)
            {
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return true;
            }

            return false;
        }

        public static string GetFileShortName(string str, int len)
        {
            if (str == null)
                return "";

            if (str.Length > len)
                return str.Substring(0, len) + "...";
            else
                return str;
        }
    }


}
