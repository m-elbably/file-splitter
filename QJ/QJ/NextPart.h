#pragma once

#include <windows.h>
#include <commctrl.h>
#include "IoUtils.h"
#include "WndUtills.h"
#include "CDialogs.h"
#include "resource.h"

class NextPart
{
public:
	static BOOL CALLBACK DialogProc(HWND, UINT, WPARAM, LPARAM);
	static string ShowDialog(HINSTANCE _hInstance, HWND _parent, string _currentPart);
};
