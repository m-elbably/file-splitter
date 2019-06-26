using XmlLib;
using System.Windows.Forms;
using System.IO;
namespace QSplit.Core
{
    public static class Ini
    {
        private static XmlSettings xs = new XmlSettings(Path.Combine(Application.StartupPath, "config.xml"));

        public enum ISction
        {
            General,
            Options,
            Splitter,
            Joiner
        }

        //Writing Properties
        public static void Write(ISction section, string key, string value)
        {
            xs.WriteValue(section.ToString().ToLower(), key, value);
        }

        public static void Write(ISction section, string key, int value)
        {
            xs.WriteValue(section.ToString().ToLower(), key, value);
        }

        public static void Write(ISction section, string key, float value)
        {
            xs.WriteValue(section.ToString().ToLower(), key, value);
        }

        public static void Write(ISction section, string key, double value)
        {
            xs.WriteValue(section.ToString().ToLower(), key, value);
        }

        public static void Write(ISction section, string key, bool value)
        {
            int dv = 0;
            if (value)
                dv = 1;

            xs.WriteValue(section.ToString().ToLower(), key, dv);
        }

        public static void Write(ISction section, string key, object value)
        {
            xs.WriteValue(section.ToString().ToLower(), key, value);
        }

        //Reading Properties
        public static string Read(ISction section, string key, string defalutValue)
        {
            return xs.ReadValue(section.ToString().ToLower(), key, defalutValue);
        }

        public static string Read(string section, string key, string defalutValue)
        {
            return xs.ReadValue(section, key, defalutValue);
        }

        public static int ReadInt(ISction section, string key, int defalutValue)
        {
            return xs.ReadInt(section.ToString().ToLower(), key, defalutValue);
        }

        public static int ReadInt(string section, string key, int defalutValue)
        {
            return xs.ReadInt(section, key, defalutValue);
        }

        public static int ReadInt(ISction section, string key, int defalutValue, int min, int max)
        {
            return xs.ReadInt(section.ToString().ToLower(), key, defalutValue, min, max);
        }

        public static bool ReadBool(ISction section, string key, bool defalutValue)
        {
            int dv = 0;
            if (defalutValue)
                dv = 1;

            return (xs.ReadInt(section.ToString().ToLower(), key, dv, 0, 1) != 0);
        }

        public static int ReadInt(string section, string key, int defalutValue, int min, int max)
        {
            return xs.ReadInt(section, key, defalutValue, min, max);
        }

        public static long ReadLong(ISction section, string key, int defalutValue)
        {
            return xs.ReadLong(section.ToString().ToLower(), key, defalutValue);
        }

        public static long ReadLong(string section, string key, int defalutValue)
        {
            return xs.ReadLong(section, key, defalutValue);
        }

        public static long ReadLong(ISction section, string key, int defalutValue, int min, int max)
        {
            return xs.ReadLong(section.ToString().ToLower(), key, defalutValue, min, max);
        }

        public static long ReadLong(string section, string key, int defalutValue, int min, int max)
        {
            return xs.ReadLong(section, key, defalutValue, min, max);
        }

        public static string[] GetKeys(string section)
        {
            return xs.GetKeys(section);
        }

        public static void Refresh()
        {
            xs.Refresh();
        }
    }
}
