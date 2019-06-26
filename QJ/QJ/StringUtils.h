#pragma once

#include <string>
#include <iostream>
#include <sstream>

using namespace std;

class StringUtils
{
public:
	static int cInt(string);
	static long cLong(string);
	static double cDbl(string);
	static string cStr(int);
	static string cStr(unsigned long);
	static string cStr(float);
	static string Concat(string, string);
	static string EncDec(string*);
};
