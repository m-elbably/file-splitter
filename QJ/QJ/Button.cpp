#include "Button.h"

void Button::Create(int x, int y, int width, int height, HWND wnd, HINSTANCE hInst, int btnID, LPCSTR text)
{
	int style = WS_CHILD | WS_VISIBLE | WS_TABSTOP;
	handle = CreateWindowEx(0, WC_BUTTON,NULL,
	                        style,
	                        x, y, width, height,
	                        wnd, (HMENU)btnID, hInst, NULL);

	SetText(text);
}

void Button::SetText(LPCSTR text) const
{
	SendMessage(handle, (UINT)WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT),
	            (LPARAM)MAKELPARAM(FALSE, 0));

	SendMessage(handle,WM_SETTEXT, 0, (LPARAM)text);
}

void Button::Enable(bool state) const
{
	EnableWindow(handle, state);
}

void Button::Visible(bool state) const
{
	if (state)
		ShowWindow(handle, SW_SHOW);
	else
		ShowWindow(handle, SW_HIDE);
}

int Button::Handle() const
{
	return (int)handle;
}
