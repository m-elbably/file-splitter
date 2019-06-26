using System;
using System.Collections.Generic;
using System.Text;
using QSplit.Tools;
using XmlLib;
using QSplit.Core;

namespace QSplit.Types
{
    public struct Profile
    {
        private string name;
        private long size;
        private int sizeIndex;
        private string description;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        public int SizeIndex
        {
            get { return sizeIndex; }
            set { sizeIndex = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Return size as string
        /// </summary>
        /// <returns></returns>
        public string GetSize()
        {
            return IoTools.ParseSize(size);
        }

        public void ReadProfile(string section)
        {
            Ini.Refresh();

            this.name = Ini.Read(section, "name", "");
            this.size = Ini.ReadLong(section, "size", 0);
            this.sizeIndex = Ini.ReadInt(section, "index", 0);
            this.description = Ini.Read(section, "description", "");
        }

    }
}
