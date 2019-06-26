#pragma once

#include <windows.h>
#include <fstream>
#include <string>

using namespace std;

class IoUtils
{
public:
	static bool IsFile(string);
	static bool IsFolder(string);
	static bool IsAccessible(string);
	static bool FileExists(string);
	static bool FolderExists(string);
	static bool CheckFileName(const string&);
	static string CurrentPath();
	static string Combine(string,string);
	static string GetFileName(string);
	static string GetFolderName(string);
	static string GetPath(const string&);
	static string GetDrive(const string&);
	static long GetFileLen(string);
	static string GetFileExt(const string&);
	static string GetFileNameNoExt(const string&);
};
