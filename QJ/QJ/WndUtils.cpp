#include "WndUtills.h"

int WndUtils::ScreeWidth()
{
	return GetSystemMetrics(SM_CXSCREEN);
}

int WndUtils::ScreenHeight()
{
	return GetSystemMetrics(SM_CYSCREEN);
}

int WndUtils::GetCenterX(int width)
{
	return (ScreeWidth() - width) / 2;
}

int WndUtils::GetCenterY(int height)
{
	return (ScreenHeight() - height) / 2;
}

int WndUtils::GetTitleHeight()
{
	return GetSystemMetrics(SM_CYCAPTION);
}

void WndUtils::CenterForm(const HWND hwnd)
{
	RECT rc;
	const HWND hParent = GetParent(hwnd);
	GetClientRect(hParent, &rc);

	if (rc.left == 0 && rc.right == 0 && rc.top == 0 && rc.bottom == 0)
	{
		ShowWindow(hParent, SW_SHOWNORMAL);
		GetClientRect(hParent, &rc);
	}

	POINT center;
	center.x = rc.left + (rc.right - rc.left) / 2;
	center.y = rc.top + (rc.bottom - rc.top) / 2;
	ClientToScreen(hParent, &center);

	RECT _rc;
	GetWindowRect(hwnd, &_rc);
	const int x = center.x - (_rc.right - _rc.left) / 2;
	const int y = center.y - (_rc.bottom - _rc.top) / 2;

	SetWindowPos(hwnd, HWND_TOP, x, y, _rc.right - _rc.left, _rc.bottom - _rc.top, SWP_SHOWWINDOW);
};
