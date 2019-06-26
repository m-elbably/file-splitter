#include "StringUtils.h"

int StringUtils::cInt(const string tstr)
{
	const int ret = atoi(tstr.c_str());
	return ret;
}

long StringUtils::cLong(const string tstr)
{
	const long ret = atol(tstr.c_str());
	return ret;
}

double StringUtils::cDbl(const string tstr)
{
	double ret = strtod(tstr.c_str(), nullptr);
	return ret;
}

string StringUtils::cStr(const int value)
{
	stringstream tstr;
	tstr.flags(ios::fixed);
	if (tstr << value)
	{
		return tstr.str();
	}
	else
	{
		return "";
	}
}

string StringUtils::cStr(const unsigned long value)
{
	stringstream tstr;
	tstr.flags(ios::fixed);
	if (tstr << value)
	{
		return tstr.str();
	}
	else
	{
		return "";
	}
}

string StringUtils::cStr(const float value)
{
	stringstream tstr;
	tstr.flags(ios::dec);
	tstr.precision(3);
	if (tstr << value)
	{
		return tstr.str();
	}
	else
	{
		return "";
	}
}

string StringUtils::Concat(string a, string b)
{
	string buffer(a);
	buffer.append(b);
	return buffer;
}

string StringUtils::EncDec(string* str)
{
	std::string buffer;

	char c;
	for (int i = 0; i < (int)str->length(); i++)
	{
		c = (char)str->at(i);
		c ^= 64;
		buffer += c;
	}

	return buffer;
}
