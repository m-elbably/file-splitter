using System;
using System.Collections.Generic;
using System.Text;

namespace QSplit.Tools
{
    static class Sfx
    {
        private static bool customFolder;
        private static string outPath = "";
        private static string comment = "";

        public static string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public static string OutPath
        {
            get { return outPath; }
            set { outPath = value; }
        }

        public static bool CustomFolder
        {
            get { return customFolder; }
            set { customFolder = value; }
        }
    }
}
