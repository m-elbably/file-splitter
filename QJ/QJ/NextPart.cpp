#include "NextPart.h"
#include <utility>

static HINSTANCE _nHInstance;
static HWND _nParent;
static string _currentPart;
static CDialogs _nCDialogs;
static string _nSelectedPart;
static string _nTip;

string NextPart::ShowDialog(HINSTANCE hInstance, HWND parent, string currentPart)
{
	_nHInstance = hInstance;
	_nParent = parent;

	_nSelectedPart = "";
	_currentPart = std::move(currentPart);
	_nTip = _currentPart; //+ "\nis required to continue.";

	DialogBox(_nHInstance, MAKEINTRESOURCE(DLGPART), _nParent, NextPart::DialogProc);
	return _nSelectedPart;
}

BOOL CALLBACK NextPart::DialogProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	int wmId;
	int wmEvent;
	const string filter(_currentPart + '\0' + _currentPart + '\0');

	switch (message)
	{
	case WM_INITDIALOG:
		WndUtils::CenterForm(hDlg);
		SetDlgItemText(hDlg,DLGPART_TIP, _nTip.c_str());
		return static_cast<INT_PTR>(TRUE);
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);

		switch (wmId)
		{
		case DLGPART_BROWSE:
			_nCDialogs.Owner = hDlg;
			_nCDialogs.DefaultExt = _currentPart.c_str();
			_nCDialogs.Filter = filter.c_str();

			if (_nCDialogs.OpenFile())
			{
				SetDlgItemText(hDlg,DLGPART_TXT, _nCDialogs.GetFileName());
				_nSelectedPart = _nCDialogs.GetFileName();
			}

			break;
		case IDOK:
			if (!IoUtils::FileExists(_nSelectedPart))
			{
				MessageBox(hDlg, "Selected file does not exists.", "Error",MB_ICONERROR);
			}
			else
			{
				EndDialog(hDlg, LOWORD(wParam));
			}

			return static_cast<INT_PTR>(TRUE);
		case IDCANCEL:
			_nSelectedPart = "";
			EndDialog(hDlg, LOWORD(wParam));
			return static_cast<INT_PTR>(TRUE);
		default: ;
		}

		break;
	default: ;
	}

	return static_cast<INT_PTR>(FALSE);
}
