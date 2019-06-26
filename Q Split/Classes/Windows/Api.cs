using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace QSplit.Windows
{
    static class Api
    {
        // Importing Windows API library
        [DllImport("user32.dll")]
        public static extern void LockWorkStation();
        [DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)]
        string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder
        pszShortPath, uint cchBuffer);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        // Lock workstation
        public static void Lock()
        {
            LockWorkStation();
        }

        // Log Off
        public static void LogOff()
        {
            ExitWindowsEx(0, 0);
        }

        // Reboot
        public static void Reboot()
        {
            ExitWindowsEx(2, 0);
        }

        // Shutdown
        public static void Shutdown()
        {
            try
            {
                ExitWindowsEx(1, 0);
                Process.Start("ShutDown", "/f /s");
            }
            catch
            {
            }
        }

        // Force LogOff
        public static void ForceLogOff()
        {
            ExitWindowsEx(4, 0);
        }

        // Hibernate
        public static void Hibernate()
        {
            Application.SetSuspendState(PowerState.Hibernate, true, true);
        }

        // Stand By
        public static void Standby()
        {
            Application.SetSuspendState(PowerState.Suspend, true, true);
        }

        public static string GetShortPathName(string longName)
        {
            StringBuilder shortNameBuffer = new StringBuilder(longName.Length);
            uint bufferSize = Convert.ToUInt16(shortNameBuffer.Capacity);

            GetShortPathName(longName, shortNameBuffer, bufferSize);

            return shortNameBuffer.ToString();
        }

    }
}
