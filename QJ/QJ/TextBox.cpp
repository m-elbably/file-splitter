#include "TextBox.h"

void TextBox::Create(int x, int y, int width, int height, HWND wnd, HINSTANCE hInst, LPCSTR text)
{
	int style;
	if (readOnly)
		style = WS_CHILD | WS_VISIBLE | WS_TABSTOP | ES_AUTOHSCROLL | WS_TABSTOP | ES_READONLY;
	else
		style = WS_CHILD | WS_VISIBLE | WS_TABSTOP | ES_AUTOHSCROLL | WS_TABSTOP;

	handle = CreateWindowEx(WS_EX_CLIENTEDGE, WC_EDIT, NULL,
	                        style,
	                        x, y, width, height,
	                        wnd, NULL, hInst, NULL);

	SetText(text);
}

void TextBox::SetText(LPCSTR text) const
{
	SendMessage(handle, (UINT)WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT),
	            (LPARAM)MAKELPARAM(FALSE, 0));

	SendMessage(handle,WM_SETTEXT, 0, (LPARAM)text);
}

LPCSTR TextBox::GetText() const
{
	WORD lnLen = 0;
	lnLen = (WORD)SendMessage(handle,EM_LINELENGTH, (WPARAM)0, (LPARAM)0);
	lnLen++; //Max number + terminating null character
	TCHAR* text = new TCHAR[lnLen];
	SendMessage(handle,WM_GETTEXT, (WPARAM)lnLen, (LPARAM)text);
	text[lnLen - 1] = '\0';

	return (LPCSTR)text;
}

void TextBox::Enable(bool state) const
{
	EnableWindow(handle, state);
}

void TextBox::Visible(bool state) const
{
	if (state)
		ShowWindow(handle, SW_SHOW);
	else
		ShowWindow(handle, SW_HIDE);
}

void TextBox::SetMaxLength(int max) const
{
	SendMessage(handle,EM_SETLIMITTEXT, max, 0);
}

void TextBox::ReadOnly(bool r)
{
	readOnly = r;
	SendMessage(handle,EM_SETREADONLY, r, 0);
}
