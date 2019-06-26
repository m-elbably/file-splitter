#include "MemoryBlock.h"

MemoryBlock::MemoryBlock(): hLength(0), filesNum(0)
{
}

MemoryBlock::MemoryBlock(const string& fPath): hLength(0), filesNum(0)
{
	filePath = fPath;
	file.open(fPath.c_str(), ios_base::in);
}

string MemoryBlock::getDataToChar(const int stPos, const int max, const char c)
{
	char ch = 0;
	const int cnt = 0;
	string buffer;

	if (file.is_open())
	{
		file.seekg(stPos);
		while (ch != c && cnt < max && !file.eof())
		{
			if (ch != 0)
				buffer += ch;

			ch = file.get();
		}
	}

	return buffer;
}

string MemoryBlock::getData(const int stPos, const int len)
{
	char ch = 0;
	string buffer;

	if (file.is_open())
	{
		file.seekg(stPos);

		for (int i = 0; i < len; i++)
		{
			buffer += file.get();
		}
	}

	return buffer;
}

int MemoryBlock::getLenToChar(const int stPos, const int max, const char c)
{
	char ch = 0;
	int cnt = 0;
	int buffer = 0;

	if (file.is_open())
	{
		file.seekg(stPos);
		while (ch != c && cnt < max && !file.eof())
		{
			if (ch != 0)
				buffer++;

			ch = file.get();
			cnt++;
		}
	}

	return buffer;
}

//the same as above func but work with strings not files
int MemoryBlock::getLenToChar(const string& str, const unsigned int stPos, const string& c)
{
	unsigned int buffer = 0;

	if (str.length() <= 0 || stPos >= str.length())
		return 0;

	for (unsigned int i = stPos; i < str.length(); i++)
	{
		if (str.substr(i, 1) == c)
		{
			buffer += stPos + 1;
			break;
		}
		else
			buffer++;
	}

	return buffer;
}

bool MemoryBlock::LoadData()
{
	int pos = 0;

	//File Verification
	if (getLenToChar(0, 4, '?') == 3)
	{
		if (getDataToChar(0, 4, '?') != "QSH")
		{
			return false;
		}
	}
	else
	{
		return false;
	}

	pos = getLenToChar(0, 16, ':');

	if (pos <= 0)
		return false;

	string buffer = getDataToChar(pos + 1, 16, '.');
	pos += buffer.length();
	const int bSize = StringUtils::cInt(buffer);
	buffer.clear();

	if (bSize <= 0)
		return false;

	hLength = bSize + pos + 2;
	buffer = StringUtils::EncDec(&getData(pos + 2, bSize));

	int bPos = 0;
	int splitPos = 0;
	const int bLen = buffer.length();

	while (bPos < bLen)
	{
		if (buffer.substr(bPos, 1) == "|" || bPos == bLen - 1)
		{
			if (splitPos == 0) //this is the files number
			{
				filesNum = StringUtils::cInt(buffer.substr(0, bPos));
			}
			else
			{
				if (bPos == bLen - 1)
					bPos++;

				Rows.push_back(buffer.substr(splitPos, bPos - splitPos));
			}

			splitPos = bPos + 1;
		}

		bPos++;
	}

	return !Rows.empty();
}

void MemoryBlock::ParseData(const string& data)
{
	int pos = 0;
	string buffer;

	pos = getLenToChar(data, 0, ":");
	pos = getLenToChar(data, pos, ".");

	if (pos <= 0)
		return;

	buffer = data.substr(pos, data.length() - pos);
	buffer = StringUtils::EncDec(&buffer);

	int bPos = 0;
	int splitPos = 0;
	const int bLen = buffer.length();

	while (bPos < bLen)
	{
		if (buffer.substr(bPos, 1) == "|" || bPos == bLen - 1)
		{
			if (bPos == bLen - 1)
				Rows.push_back(buffer.substr(splitPos, bPos - (splitPos - 1)));
			else
				Rows.push_back(buffer.substr(splitPos, bPos - splitPos));

			splitPos = bPos + 1;
		}

		bPos++;
	}
}

int MemoryBlock::GetLength() const
{
	return hLength;
}

int MemoryBlock::GetFilesNumber() const
{
	return filesNum;
}

MemoryBlock::~MemoryBlock()
{
	try
	{
		file.close();
		Rows.clear();
	}
	catch (...)
	{
	}
}
