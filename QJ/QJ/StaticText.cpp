#include "StaticText.h"

void StaticText::Create(int x, int y, int width, int height, HWND wnd, HINSTANCE hInst, LPCSTR text)
{
	handle = CreateWindowEx(0, WC_STATIC, NULL,
	                        WS_CHILD | WS_VISIBLE | WS_TABSTOP,
	                        x, y, width, height,
	                        wnd, NULL, hInst, NULL);

	SetBkColor(GetWindowDC(handle),TRANSPARENT);
	SetText(text);
}

void StaticText::SetText(LPCSTR text) const
{
	SendMessage(handle, (UINT)WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT),
	            (LPARAM)MAKELPARAM(FALSE, 0));

	SendMessage(handle,WM_SETTEXT, 0, (LPARAM)text);
}

void StaticText::Enable(bool state) const
{
	EnableWindow(handle, state);
}

void StaticText::Visible(bool state) const
{
	if (state)
		ShowWindow(handle, SW_SHOW);
	else
		ShowWindow(handle, SW_HIDE);
}

HWND StaticText::Handle() const
{
	return handle;
}
