#include "IoUtils.h"
#include <utility>

bool IoUtils::IsFile(string szPath)
{
	const DWORD dwAttr = GetFileAttributes(szPath.c_str());
	return (dwAttr & FILE_ATTRIBUTE_DIRECTORY) == 0;
}

bool IoUtils::IsFolder(const string szPath)
{
	const DWORD dwAttr = GetFileAttributes(szPath.c_str());
	return (dwAttr & FILE_ATTRIBUTE_DIRECTORY) != 0;
}

bool IoUtils::FileExists(string szPath)
{
	const DWORD dwAttr = GetFileAttributes(szPath.c_str());
	return dwAttr != 0xffffffff;
}

bool IoUtils::FolderExists(string szPath)
{
	const DWORD dwAttr = GetFileAttributes(szPath.c_str());
	return dwAttr != 0xffffffff;
}

bool IoUtils::IsAccessible(string szPath)
{
	const DWORD dwAttr = GetFileAttributes(szPath.c_str());
	return dwAttr != 0xffffffff;
}

string IoUtils::CurrentPath()
{
	char buffer[MAX_PATH] = {0};
	GetModuleFileName(GetModuleHandle(0), LPCH(buffer), 255);
	return IoUtils::GetPath(buffer);
}

string IoUtils::Combine(const string a, const string b)
{
	string buffer;
	if (a.substr(a.length() - 1, 1) == "\\")
		buffer.append(a);
	else
		buffer.append(a + "\\");

	buffer.append(b);
	return buffer;
}

string IoUtils::GetFileName(const string szPath)
{
	unsigned int index = 0;
	string buffer;

	index = szPath.find_last_of('\\');

	if (index > 0 && index < szPath.length())
		buffer = szPath.substr(index + 1, szPath.length() - index);

	return buffer;
}

string IoUtils::GetFolderName(string szPath)
{
	return GetFileName(std::move(szPath));
}

string IoUtils::GetPath(const string& szPath)
{
	int index = 0;
	string buffer;

	index = szPath.find_last_of('\\');

	if (index > 0)
		buffer = szPath.substr(0, index);

	return buffer;
}

long IoUtils::GetFileLen(const string path)
{
	long ln = 0;
	ifstream file;
	file.open(path.c_str());

	file.seekg(0, ios::end);
	ln = file.tellg();
	file.seekg(0, ios::beg);

	file.close();
	return ln;
}

string IoUtils::GetFileExt(const const string& szPath)
{
	unsigned int index = 0;
	string buffer;

	index = szPath.find_last_of('.');


	if (index > 0 && index < szPath.length())
		buffer = szPath.substr(index + 1, szPath.length() - index);

	return buffer;
}

string IoUtils::GetFileNameNoExt(const string& szPath)
{
	const string ext = GetFileExt(szPath);
	if (ext.length() > 0)
		return szPath.substr(0, szPath.length() - (ext.length() + 1));
	else
		return "";
}

string IoUtils::GetDrive(const string& path)
{
	if (path.length() < 3)
		return "";

	const string drv = path.substr(0, 3);
	if (drv.substr(0, 1) != "\\")
		return path.substr(0, 3);
	else
		return "\\\\";
}

bool IoUtils::CheckFileName(const string& path)
{
	for (unsigned int i = 0; i < path.length(); i++)
	{
		string ch = path.substr(i, 1);
		if (ch == "/" ||
			ch == "\\" ||
			ch == ":" ||
			ch == "*" ||
			ch == "?" ||
			ch == "\"" ||
			ch == "<" ||
			ch == ">" ||
			ch == "|")
		{
			return false;
		}
	}

	return true;
}
