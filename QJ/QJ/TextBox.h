#pragma once
#include <windows.h>
#include <commctrl.h>

class TextBox
{
	HWND handle;
	bool readOnly;

public:
	void Create(int, int, int, int, HWND, HINSTANCE, LPCSTR);
	void SetMaxLength(int) const;
	void SetText(LPCSTR) const;
	LPCSTR GetText() const;
	void ReadOnly(bool);
	void Enable(bool) const;
	void Visible(bool) const;
};
