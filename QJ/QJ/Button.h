#pragma once
#include <windows.h>
#include <commctrl.h>

class Button
{
	HWND handle;

public:
	void Create(int, int, int, int, HWND, HINSTANCE, int, LPCSTR);
	void SetText(LPCSTR) const;
	int Handle() const;
	void Enable(bool) const;
	void Visible(bool) const;
};
