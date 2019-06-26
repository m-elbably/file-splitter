using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class MemoryBlock
{
    private List<string> rows;

    public MemoryBlock()
    {
        rows = new List<string>();
    }

    public List<string> Rows
    {
        get { return rows; }
        set { rows = value; }
    }

    public void ReadBlock(string block)
    {
        string buffer = "";
        rows.Clear();

        buffer = EncryptDecrypt(block);
        string[] items = buffer.Split('|');

        for (int i = 0; i < items.Length; i++)
        {
            rows.Add(items[i]);
        }
    }

    public void ReadBlock(byte[] block)
    {
        string buffer = "";

        buffer = Encoding.UTF8.GetString(block);
        ReadBlock(buffer);
    }

    public string GetBlockString
    {
        get
        {
            string buffer = "";
            for (int i = 0; i < rows.Count; i++)
            {
                if (i < rows.Count - 1)
                    buffer += rows[i] + "|";
                else
                    buffer += rows[i];
            }


            buffer = EncryptDecrypt(buffer);
            buffer = Encoding.UTF8.GetByteCount(buffer).ToString() + "." + buffer;
            return buffer;
        }
    }

    public byte[] GetBlockBytes
    {
        get
        {
            string buffer = "";
            for (int i = 0; i < rows.Count; i++)
            {
                if (i < rows.Count - 1)
                    buffer += rows[i] + "|";
                else
                    buffer += rows[i];
            }

            buffer = EncryptDecrypt(buffer);
            buffer = Encoding.UTF8.GetByteCount(buffer).ToString() + "." + buffer;
            return Encoding.UTF8.GetBytes(buffer);
        }
    }

    public int BlockLength
    {
        get { return GetBlockString.Length; }
    }

    private string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);

        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ 64);
            outSb.Append(c);
        }

        return outSb.ToString();
    }
}

public class Header
{
    private MemoryBlock blk = new MemoryBlock();
    private static string sgn = "QSH?2.0:";

    public MemoryBlock HeaderBlock
    {
        get { return blk; }
        set { blk = value; }
    }


    public void Add(string text)
    {
        blk.Rows.Add(text);
    }

    public void Write(BinaryWriter bW)
    {
        bW.Write(GetBytes(sgn));
        bW.Write(blk.GetBlockBytes);
    }

    /// <summary>
    /// Get header length from file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public int GetHeaderLength(string path, FileStream fs)
    {
        string fSign = "";  //file signature
        byte[] buffer = new byte[3];
        //FileStream fsReader = new FileStream(path, FileMode.Open);
        fs.Read(buffer, 0, buffer.Length);

        if (Encoding.UTF8.GetString(buffer, 0, 3) != "QSH")
        {
            return 0;
        }

        fSign = GetData(fs, 0, ':');

        int hLen = 0;
        string strLen = GetData(fs, fSign.Length + 1, '.');
        hLen = int.Parse(strLen);

        int offset = fSign.Length + strLen.Length + 2;

        fs.Position = hLen + offset;

        return (hLen + offset);
    }

    public bool ReadHeader(string path)
    {
        string fSign = "";  //file signature
        byte[] buffer = new byte[3];

        FileStream fsReader = new FileStream(path, FileMode.Open);
        fsReader.Read(buffer, 0, buffer.Length);

        if (Encoding.UTF8.GetString(buffer, 0, 3) != "QSH")
        {
            fsReader.Close();
            fsReader.Dispose();

            return false;
        }

        fSign = GetData(fsReader, 0, ':');

        int hLen = 0;
        string strLen = GetData(fsReader, fSign.Length + 1, '.');
        hLen = int.Parse(strLen);

        int offset = fSign.Length + strLen.Length + 2;
        buffer = new byte[hLen];

        fsReader.Position = offset;
        fsReader.Read(buffer, 0, hLen);
        blk.ReadBlock(buffer);

        fsReader.Close();
        fsReader.Dispose();

        return true;
    }

    private string GetData(FileStream fs, int start, char end)
    {
        int cnt = 0;
        fs.Position = start;
        int bt = 0;
        byte[] buffer = new byte[128];

        while ((char)bt != end)
        {
            if (bt != 0)
                buffer[cnt++] = (byte)bt;

            bt = fs.ReadByte();
        }

        return Encoding.UTF8.GetString(buffer, 0, cnt);
    }

    /// <summary>
    /// used for splitter to get memory block data length
    /// </summary>
    public long Length
    {
        get
        {
            return (blk.BlockLength + sgn.Length);
        }
    }

    public static string Signature
    {
        get { return sgn; }
        set { sgn = value; }
    }

    /// <summary>
    /// ch = the delimeter char
    /// charArray = the array to extract from it
    /// 
    /// used to extract chars until finding given delimeter
    /// </summary>
    private int GetLength(char ch, FileStream stream)
    {
        string StrLen = "";
        int finalLen = 0;
        char c;
        while (stream.Position != stream.Length)
        {
            c = Convert.ToChar(stream.ReadByte());
            if (c == ch)
                break;
            else
                StrLen += c;

        }

        int.TryParse(StrLen, out finalLen);

        return finalLen;
    }

    private byte[] GetBytes(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        return buffer;
    }


}