#pragma once

#include <windows.h>
#include <commctrl.h>

class StaticText
{
	HWND handle;

public:
	void Create(int, int, int, int, HWND, HINSTANCE, LPCSTR);
	void SetText(LPCSTR) const;
	void Enable(bool) const;
	void Visible(bool) const;
	HWND Handle() const;
};
