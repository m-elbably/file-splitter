#include "ProgressBar.h"

void ProgressBar::Create(int x,int y,int width,int height,HWND wnd,HINSTANCE hInst,int max,int value)
{
   int style = WS_CHILD | WS_VISIBLE | PBS_SMOOTH;
   handle = CreateWindowEx(0, PROGRESS_CLASS, NULL,
	               style ,
		      x, y, width, height,
		      wnd, NULL, hInst, NULL);

   SetMax(max);
   SetValue(value);
}

void ProgressBar::SetMax(int value) const
{
	SendMessage(handle,PBM_SETRANGE,0,MAKELPARAM(0,value));
}

void ProgressBar::SetValue(int value) const
{
	SendMessage(handle,PBM_SETPOS,value,0);
}

int ProgressBar::GetValue() const
{
    return SendMessage(handle,PBM_SETPOS,0,0);
}

void ProgressBar::Enable(bool state) const
{
	EnableWindow(handle , state );
}

void ProgressBar::Visible(bool state) const
{
	if(state)
		ShowWindow(handle , SW_SHOW );
	else
		ShowWindow(handle , SW_HIDE );
}

HWND ProgressBar::Handle() const
{
	return handle;
}
