using System;
using System.Collections.Generic;
using System.Text;

namespace QSplit
{
    class App
    {
        private static string[] args;
        private static string currentFile;
        private static string operation;

        static App()
        {
            currentFile = "";
            operation = "";
        }

        public static string[] Args
        {
            get { return args; }
            set { args = value; }
        }

        public static string CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; }
        }

        public static string Operation
        {
            get { return operation; }
            set { operation = value; }
        }


    }
}
