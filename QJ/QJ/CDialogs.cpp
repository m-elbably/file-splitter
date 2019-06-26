#include "CDialogs.h"

CDialogs::CDialogs()
{
	_dFlags = OFN_EXPLORER |
		OFN_FILEMUSTEXIST |
		OFN_OVERWRITEPROMPT;

	DlgFlags = 0;
	Filter = NULL;
	InitialDir = NULL;
	DefaultExt = NULL;
}

LPCSTR CDialogs::GetFileName() const
{
	return _fileName;
}

bool CDialogs::OpenFile()
{
	ZeroMemory(&ofn, sizeof(ofn));

#ifndef	NULL_TERMINATE
	{
		for (int i = 0; i < MAX_PATH; i++)
			_fileName[i] = ' ';
	}
#else
	_fileName[0] = '\0';
#endif

	if (DlgFlags == 0)
		DlgFlags = _dFlags;

	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = Owner;
	ofn.nFileOffset = 1;
	ofn.lpstrFilter = Filter;
	ofn.lpstrFile = _fileName;
	ofn.nMaxFile = MAX_PATH;
	ofn.lpstrInitialDir = InitialDir;
	ofn.Flags = DlgFlags;
	ofn.lpstrDefExt = DefaultExt;

	return GetOpenFileName(&ofn) != 0;
}

bool CDialogs::SaveFile()
{
	ZeroMemory(&ofn, sizeof(ofn));

#ifndef	NULL_TERMINATE
	{
		for (int i = 0; i < MAX_PATH; i++)
			_fileName[i] = ' ';
	}
#else
	_fileName[0] = '\0';
#endif

	if (DlgFlags == 0)
		DlgFlags = _dFlags;

	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = Owner;
	ofn.nFileOffset = 1;
	ofn.lpstrFilter = Filter;
	ofn.lpstrFile = _fileName;
	ofn.nMaxFile = MAX_PATH;
	ofn.lpstrInitialDir = InitialDir;
	ofn.Flags = DlgFlags;
	ofn.lpstrDefExt = DefaultExt;

	return GetSaveFileName(&ofn) != 0;
}

CDialogs::~CDialogs()
{
	//delete[] &_fileName;
	//delete &ofn;
}
