#pragma once
#include <windows.h>
#include <Commdlg.h>
#pragma comment(lib,"comctl32.lib")
#define NULL_TERMINATE

class CDialogs
{
	OPENFILENAME ofn;
	TCHAR _fileName[MAX_PATH];
	DWORD _dFlags;

public:
	HWND Owner;
	LPCSTR Filter;
	LPCSTR InitialDir;
	LPCSTR DefaultExt;
	DWORD DlgFlags;

	enum eOpenFileFlags
	{
		E_ALLOWMULTISELECT = 0x200,
		E_CREATEPROMPT = 0x2000,
		E_EXPLORER = 0x80000,
		E_EXTENSIONDIFFERENT = 0x400,
		E_FILEMUSTEXIST = 0x1000,
		E_HIDEREADONLY = 0x4,
		E_NOCHANGEDIR = 0x8,
		E_NODEREFERENCELINKS = 0x100000,
		E_NONETWORKBUTTON = 0x20000,
		E_NOREADONLYRETURN = 0x8000,
		E_NOTESTFILECREATE = 0x10000,
		E_NOVALIDATE = 0x100,
		E_OVERWRITEPROMPT = 0x2,
		E_PATHMUSTEXIST = 0x800,
		E_READONLY = 0x1,
		E_SHOWHELP = 0x10
	};

	CDialogs();
	bool OpenFile();
	bool SaveFile();
	LPCSTR GetFileName() const;
	~CDialogs();
};
