using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System;

namespace XmlLib
{
    public class XmlSettings
    {
        private XmlDocument xmlDoc = null;
        private string fileName;
        private string root = "Settings";
        private bool gError;

        public XmlSettings(string fileName)
        {
            CreateXmlFile(fileName);
        }

        public XmlSettings(string root, string fileName)
        {
            this.root = root;
            CreateXmlFile(fileName);
        }

        /// <summary>
        /// Read Only
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// Read Only
        /// </summary>
        public string Root
        {
            get { return root; }
        }

        #region Xml File Creation...

        private void CreateXmlFile(string fileName)
        {
            xmlDoc = new XmlDocument();
            this.fileName = fileName;

            try
            {
                if (!File.Exists(fileName))
                    CreateDoc();
                else
                    xmlDoc.Load(fileName);
            }
            catch
            {
                CreateDoc();
            }
        }

        private void CreateDoc()
        {
            try
            {
                XmlNode dtd = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmlDoc.AppendChild(dtd);
                XmlElement docNode = xmlDoc.CreateElement(root);
                xmlDoc.AppendChild(docNode);

                xmlDoc.Save(fileName);
            }
            catch (Exception ex)
            {
                gError = true;
                MessageBox.Show("Invalid xml settings file\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SaveDoc()
        {
            XmlTextWriter xWr = new XmlTextWriter(fileName, Encoding.UTF8);
            xWr.Formatting = Formatting.Indented;
            xmlDoc.WriteTo(xWr);
            xWr.Close();
        }

        public void Refresh()
        {
            try
            {
                xmlDoc.Load(fileName);
            }
            catch
            {
                gError = true;
                return;
            }
        }

        #endregion

        #region Write Values...

        public bool WriteValue(string section, string key, string value)
        {
            try
            {
                XmlElement rootElement = xmlDoc.DocumentElement;
                XmlNode xmlSectionNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(section));
                XmlAttribute nameAtt = null;
                XmlAttribute valueAtt = null;

                if (xmlSectionNode == null)
                {
                    //xmlSectionNode = xmlDoc.DocumentElement;
                    XmlNode nNode = xmlDoc.CreateElement("section");
                    nameAtt = CreateAttribute("name", section);
                    nNode.Attributes.Append(nameAtt);
                    xmlSectionNode = rootElement.AppendChild(nNode);
                }

                XmlNode xmlKeyNode = xmlSectionNode.SelectSingleNode(GetKeyPath(key));

                if (xmlKeyNode == null)
                {
                    XmlNode nNode = xmlDoc.CreateElement("key");
                    nameAtt = CreateAttribute("name", key);
                    valueAtt = CreateAttribute("value", value);
                    nNode.Attributes.Append(nameAtt);
                    nNode.Attributes.Append(valueAtt);
                    xmlSectionNode.AppendChild(nNode);
                }
                else
                {
                    nameAtt = CreateAttribute("name", key);
                    valueAtt = CreateAttribute("value", value);
                    xmlKeyNode.Attributes.Append(nameAtt);
                    xmlKeyNode.Attributes.Append(valueAtt);
                    xmlSectionNode.AppendChild(xmlKeyNode);
                }

                SaveDoc();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private XmlAttribute CreateAttribute(string attName, string value)
        {
            XmlAttribute att = xmlDoc.CreateAttribute(attName);
            att.Value = value;

            return att;
        }

        private string GetRootPath()
        {
            return "/" + root + "/";
        }

        private string GetSectionPath(string section)
        {
            return "section[@name=\"" + section + "\"]";
        }

        private string GetKeyPath(string key)
        {
            return "key[@name=\"" + key + "\"]";
        }

        public bool WriteValue(string section, string key, int value)
        {
            return WriteValue(section, key, value.ToString());
        }

        public bool WriteValue(string section, string key, float value)
        {
            return WriteValue(section, key, value.ToString());
        }

        public bool WriteValue(string section, string key, double value)
        {
            return WriteValue(section, key, value.ToString());
        }

        public bool WriteValue(string section, string key, object value)
        {
            return WriteValue(section, key, value.ToString());
        }

        #endregion

        #region Read Values...

        public string ReadValue(string section, string key, string defaultValue)
        {
            if (gError)
                return defaultValue;
            try
            {
                XmlElement rootElement = xmlDoc.DocumentElement;
                XmlNode xmlKeyNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(section) + "/" + GetKeyPath(key));
                if (xmlKeyNode != null)
                {
                    if (xmlKeyNode.Attributes["value"] != null)
                        return xmlKeyNode.Attributes["value"].Value;
                }
            }
            catch { }

            return defaultValue;
        }

        public int ReadInt(string section, string key, int defaultValue)
        {
            int value = defaultValue;
            int.TryParse(ReadValue(section, key, defaultValue.ToString()), out value);

            return value;
        }

        public int ReadInt(string section, string key, int defaultValue, int min, int max)
        {
            int value = defaultValue;
            int.TryParse(ReadValue(section, key, defaultValue.ToString()), out value);
            if (value < min || value > max)
                value = defaultValue;

            return value;
        }

        public long ReadLong(string section, string key, long defaultValue)
        {
            long value = defaultValue;
            long.TryParse(ReadValue(section, key, defaultValue.ToString()), out value);

            return value;
        }

        public long ReadLong(string section, string key, long defaultValue, long min, long max)
        {
            long value = defaultValue;
            long.TryParse(ReadValue(section, key, defaultValue.ToString()), out value);
            if (value < min || value > max)
                value = defaultValue;

            return value;
        }

        #endregion


        public bool KeyExists(string section, string key)
        {
            XmlElement rootElement = xmlDoc.DocumentElement;
            try
            {
                XmlNode xmlKeyNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(section) + "/" + GetKeyPath(key));
                return (xmlKeyNode != null);
            }
            catch
            {
                return false;
            }
        }

        public bool SectionExists(string section)
        {
            XmlElement rootElement = xmlDoc.DocumentElement;
            try
            {
                XmlNode xmlsectionNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(section));
                return (xmlsectionNode != null);
            }
            catch
            {
                return false;
            }
        }

        public void DeleteKey(string section, string key)
        {
            XmlElement rootElement = xmlDoc.DocumentElement;
            XmlNode xmlSectionNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(section));

            if (xmlSectionNode != null)
            {
                try
                {
                    XmlNode xmlKeyNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(section) + "/" + GetKeyPath(key));
                    if (xmlKeyNode != null)
                    {
                        xmlSectionNode.RemoveChild(xmlKeyNode);
                        SaveDoc();
                    }
                }
                catch
                {
                    return;
                }

            }

        }

        public void DeleteSection(string sectionName)
        {
            XmlElement rootElement = xmlDoc.DocumentElement;
            XmlNode xmlSectionNode = rootElement.SelectSingleNode(GetRootPath() + GetSectionPath(sectionName));

            if (xmlSectionNode != null)
            {
                rootElement.RemoveChild(xmlSectionNode);
                SaveDoc();
            }

        }

        public string[] GetSections()
        {
            XmlDocument doc = xmlDoc;
            if (doc == null)
                return null;

            XmlElement root = doc.DocumentElement;
            if (root == null)
                return null;

            XmlNodeList sectionNodes = root.SelectNodes("section[@name]");
            if (sectionNodes == null)
                return null;

            string[] sections = new string[sectionNodes.Count];
            int i = 0;

            foreach (XmlNode node in sectionNodes)
                sections[i++] = node.Attributes["name"].Value;

            return sections;
        }

        public string[] GetKeys(string section)
        {
            XmlDocument doc = xmlDoc;
            if (doc == null)
                return null;

            XmlElement root = doc.DocumentElement;
            if (root == null)
                return null;

            XmlNode sectionNode = root.SelectSingleNode(GetSectionPath(section));
            if (sectionNode == null)
                return null;

            XmlNodeList keyNodes = sectionNode.SelectNodes("key[@name]");
            if (keyNodes == null)
                return null;

            string[] keys = new string[keyNodes.Count];
            int i = 0;

            foreach (XmlNode node in keyNodes)
                keys[i++] = node.Attributes["name"].Value;

            return keys;
        }
    }
}
