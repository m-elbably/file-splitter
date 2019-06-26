#pragma once
#include <windows.h>
#include <commctrl.h>

class ProgressBar
{
	HWND handle;

public:
	void Create(int,int,int,int,HWND,HINSTANCE,int,int);
	int GetValue() const;
	void SetValue(int) const;
	void SetMax(int) const;
	void Enable(bool) const;
	void Visible(bool) const;
	HWND Handle() const;
};