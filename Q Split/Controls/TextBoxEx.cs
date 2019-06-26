using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace QSplit
{
    public partial class TextBoxEx : TextBox
    {
        public enum TBoxType
        {
            Text,
            StartWithChar,
            Integer,
            Float
        }

        private TBoxType type;
        private bool allowNegative;

        int WM_PASTE = 0x0302;

        public TextBoxEx()
        {
            InitializeComponent();
        }

        public TBoxType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool AllowNegative
        {
            get { return allowNegative; }
            set { allowNegative = value; }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE)
            {
                IDataObject obj = Clipboard.GetDataObject();
                string input = (string)obj.GetData(typeof(string));

                if (!CheckData(input))
                {
                    m.Result = (IntPtr)0;
                    return;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            string newText = base.Text.Substring(0, base.SelectionStart)
                             + e.KeyChar.ToString()
                             + base.Text.Substring(base.SelectionStart + base.SelectionLength);

            if (!char.IsControl(e.KeyChar))
            {
                if (e.KeyChar == ' ' &&
                    type != TBoxType.Text &&
                    type != TBoxType.StartWithChar)
                {
                    e.Handled = true;
                }
                else
                {
                    if (e.KeyChar != '\b')
                        e.Handled = !CheckData(newText);
                }
            }

            base.OnKeyPress(e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            e.Cancel = !CheckData(base.Text);
        }

        private bool CheckData(string text)
        {
            switch (type)
            {
                case TBoxType.Text:
                    break;
                case TBoxType.StartWithChar:
                    if (text.Length > 0)
                    {
                        if (char.IsDigit(text[0]))
                            return false;
                    }

                    break;
                case TBoxType.Integer:
                    if (!IsInt(text, allowNegative))
                        return false;

                    break;
                case TBoxType.Float:
                    if (!IsDecimal(text, allowNegative))
                        return false;

                    break;
                default:
                    break;
            }

            return true;
        }

        private bool IsInt(string data, bool negative)
        {
            bool buffer = true;
            for (int i = 0; i < data.Length; i++)
            {
                if (!char.IsDigit(data[i]))
                {
                    if (!(data[i] == '-' && i == 0 && negative))
                    {
                        buffer = false;
                        break;
                    }
                }
            }

            return buffer;
        }

        private bool IsDecimal(string data, bool negative)
        {
            bool buffer = true;
            bool fPoint = false;
            for (int i = 0; i < data.Length; i++)
            {
                if (!char.IsDigit(data[i]))
                {
                    if (!(data[i] == '-' && i == 0 && negative))
                    {

                        if (data[i] == '.' && fPoint == false)
                        {
                            fPoint = true;
                        }
                        else
                        {
                            buffer = false;
                            break;
                        }
                    }
                }
            }

            return buffer;
        }


    }
}
