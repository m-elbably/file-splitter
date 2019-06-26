#pragma once
#include <windows.h>

class WndUtils
{
public:
	static int ScreeWidth();
	static int ScreenHeight();
	static int GetCenterX(int);
	static int GetCenterY(int);
	static int GetTitleHeight();
	static void CenterForm(HWND);
};
