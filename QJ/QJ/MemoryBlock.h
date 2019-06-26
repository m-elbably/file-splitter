#pragma once

#include <fstream>
#include <vector>
#include "StringUtils.h"

class MemoryBlock
{
	string filePath;
	int hLength;
	ifstream file;
	int filesNum;

	string getDataToChar(int, int, char);
	string getData(int, int);
	int getLenToChar(int, int, char);
	static int getLenToChar(const string&, unsigned int, const string&);

public:
	MemoryBlock();
	MemoryBlock(const string&);
	vector<string> Rows;
	bool LoadData();
	void ParseData(const string&);
	int GetLength() const;
	int GetFilesNumber() const;
	~MemoryBlock();
};
