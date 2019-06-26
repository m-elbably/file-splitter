using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QSplit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            QProcess pType = QProcess.None;

            if (args.Length > 1)
            {


                App.Args = args;
                App.Operation = args[0];

                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] != "")
                        App.CurrentFile += args[i];

                    if (i < args.Length - 1)
                        App.CurrentFile += " ";

                    MessageBox.Show("\"" + args[i] + "\"");
                }

                
                if (args[0] == "/split")
                    pType = QProcess.Split;
                else if (args[0] == "/join")
                    pType = QProcess.Join;
            }

            Application.Run(new FrmMain(pType));

        }
    }
}
