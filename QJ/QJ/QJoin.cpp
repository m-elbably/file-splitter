#define WIN32_LEAN_AND_MEAN

#include "CDialogs.h"
#include "StaticText.h"
#include "TextBox.h"
#include "Button.h"
#include "ProgressBar.h"
#include "Threading.h"
#include "Joiner.h"
#include "shellapi.h"

HINSTANCE _mHInstance;
HWND _mHandle;

string _currentPart;
ofstream* _fileHandle = nullptr;
volatile long endThread = 0;
volatile long joinThreadIsActive = 0;

CDialogs win32Dialogs;
MemoryBlock* memoryBlock = new MemoryBlock();

// int oldProgress = 0;
Thread* mThread = NULL;
Thread* joinThread = NULL;

//Injected Data
int fNum = 0; //Files number (to be joined)
int cOutPathIndex = 0; //Output path index
int cIndex = 0; //Comment line index in memory block rows
string oFileName;
string mOutPath; //output path data
string mComment; //Comment line data (will be shown before main window display
string mFilter; //Output file type
string mDefaultExt;

// Main Dialog:
HINSTANCE hInst;
ATOM QRegisterClass(HINSTANCE hInstance);
BOOL InitInstance(HINSTANCE, int);
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

//About Dialog
INT_PTR CALLBACK About(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK Comment(HWND, UINT, WPARAM, LPARAM);

StaticText lblSource;
TextBox txtSource;
StaticText lblDestination;
TextBox txtDestination;
Button btnBrowse;

Button btnAbout;
Button btnClose;
Button btnJoin;

StaticText lblProgressMessage;
StaticText lblProgressPercentage;
ProgressBar progressBar;

//Main functions:
string GetBinaryStub(); //Get injected binary data
bool ParseBinaryStub(); //Parse the binary data to memory block 	
void SetControls(bool); //Set gui controls state while run/idle modes
bool CheckOutputFile(); //Check for main output file
void FillData(Joiner*); //Fill the join class with data required to start it's thread
void DisableControls();
void DoEvents();
void UpdateUi(int);
void Clean();
void RunTread();

int WINAPI WinMain(HINSTANCE hInstance,
                   HINSTANCE hPrevInstance,
                   LPSTR lpCmdLine,
                   int nCmdShow)
{
	InitCommonControls();
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	MSG msg;
	QRegisterClass(hInstance);

	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	while (GetMessage(&msg, NULL, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return int(msg.wParam);
}

ATOM QRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;
	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_ICON));
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = HBRUSH(COLOR_3DFACE + 1);
	wcex.lpszMenuName = NULL;
	wcex.lpszClassName = "QJoin";
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_ICON));

	return RegisterClassEx(&wcex);
}

BOOL InitInstance(const HINSTANCE hInstance, const int nCmdShow)
{
	int hFactor = WndUtils::GetTitleHeight() - 20;
	hInst = hInstance;

	const DWORD WS_QWindow = WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | DS_CENTER;
	hFactor = hFactor > 0 ? hFactor : 0;
	const HWND hWnd = CreateWindow("QJoin",
	                               "Q Join",
	                               WS_QWindow,
	                               WndUtils::GetCenterX(400),
	                               WndUtils::GetCenterY(200),
	                               400,
	                               202 + hFactor,
	                               NULL,
	                               NULL,
	                               hInstance,
	                               NULL);


	if (!hWnd)
		return FALSE;

	_mHandle = hWnd;
	_mHInstance = hInstance;

	ParseBinaryStub();
	if (mComment.length() > 0)
	{
		if (mComment[0] != 0)
		{
			DialogBox(hInst, MAKEINTRESOURCE(DLGCOMMENT), hWnd, Comment);
		}
	}

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_CREATE:
		lblSource.Create(12, 12, 140, 15, hWnd, hInst, "Source File:");
		txtSource.Create(12, 28, 367, 20, hWnd, hInst, "");
		txtSource.ReadOnly(true);
		lblDestination.Create(12, 56, 140, 15, hWnd, hInst, "Destination File:");
		txtDestination.Create(12, 72, 337, 20, hWnd, hInst, "");
		txtDestination.SetMaxLength(260);
		btnBrowse.Create(355, 70, 25, 23, hWnd, hInst, 2, "...");

		btnJoin.Create(230, 142, 73, 23, hWnd, hInst, 7, "Join");
		btnClose.Create(310, 142, 73, 23, hWnd, hInst, 8, "Close");
		btnAbout.Create(12, 142, 73, 23, hWnd, hInst, 6, "About");

		lblProgressMessage.Create(12, 125, 160, 15, hWnd, hInst, "Joining Files:");
		lblProgressPercentage.Create(185, 125, 50, 15, hWnd, hInst, "");
		progressBar.Create(12, 144, 200, 19, hWnd, hInst, 100, 0);

		lblProgressMessage.Visible(false);
		lblProgressPercentage.Visible(false);
		progressBar.Visible(false);

		break;
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);

		if (wmEvent == BN_CLICKED)
		{
			switch (wmId)
			{
			case 2:
				win32Dialogs.Owner = _mHandle;
				win32Dialogs.DefaultExt = mDefaultExt.c_str();
				win32Dialogs.Filter = mFilter.c_str();

				if (win32Dialogs.SaveFile())
					txtDestination.SetText(win32Dialogs.GetFileName());

				break;
			case 6: //About
				DialogBox(hInst, MAKEINTRESOURCE(DLGABOUT), hWnd, About);
				break;
			case 7: //Join Button
				if (CheckOutputFile())
				{
					lblProgressPercentage.SetText("0%");
					progressBar.SetValue(0);
					DoEvents();

					joinThreadIsActive = 1;
					RunTread();
				}

				break;
			case 8:
				Clean();
				PostQuitMessage(0);
				break;
			default:
				break;
			}
		}

		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		Clean();
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}

	return 0;
}

void Clean()
{
	btnClose.Enable(false);
	UpdateWindow(_mHandle);
	InterlockedIncrement(&endThread);
	if (joinThreadIsActive != 0)
	{
		if (_fileHandle != NULL)
			_fileHandle->close();

		//if(joinThread != NULL)
		//	joinThread->Stop();

		if (IoUtils::FileExists(oFileName))
		{
			DeleteFile(oFileName.c_str());
		}
	}
}

void SetControls(bool runMode)
{
	const bool value = !runMode;
	btnAbout.Visible(value);
	btnJoin.Enable(value);
	lblSource.Enable(value);
	lblDestination.Enable(value);
	txtSource.Enable(value);
	txtDestination.Enable(value);
	btnBrowse.Enable(value);
	lblProgressMessage.Visible(!value);
	lblProgressPercentage.Visible(!value);
	progressBar.Visible(!value);
}

void DisableControls()
{
	btnJoin.Enable(false);
	lblSource.Enable(false);
	lblDestination.Enable(false);
	txtSource.Enable(false);
	txtDestination.Enable(false);
	btnBrowse.Enable(false);
}

void UpdateUi(int value)
{
	progressBar.SetValue(value);
	lblProgressPercentage.SetText(StringUtils::Concat(StringUtils::cStr(value), " %").c_str());
}

bool CheckOutputFile()
{
	string msg;
	const string oFile = txtDestination.GetText();
	if (!IoUtils::CheckFileName(IoUtils::GetFileName(oFile)))
	{
		MessageBox(_mHandle, "A file name cannot contain any of the following characters:\n/\\:*?\"<>|", "Q Join",
		           MB_ICONINFORMATION);
		return false;
	}

	if (!IoUtils::FolderExists(IoUtils::GetPath(oFile)))
	{
		msg = "\"" + IoUtils::GetPath(oFile) + "\" does not exists.";
		MessageBox(_mHandle, msg.c_str(), "Q Join",MB_ICONERROR);
		return false;
	}

	if (IoUtils::FileExists(txtDestination.GetText()))
	{
		msg = "\"" + string(txtDestination.GetText()) + "\" is already exists.\nDo you want to overwrite it?";
		const int mID = MessageBox(_mHandle, msg.c_str(), "Q Join",MB_YESNO | MB_ICONEXCLAMATION);

		if (mID == IDNO)
		{
			return false;
		}

		if (!DeleteFile(txtDestination.GetText()))
		{
			msg = "\"" + string(txtDestination.GetText()) + "\" is Inaccessible, The file already opened.";
			MessageBox(_mHandle, msg.c_str(), "Error",MB_ICONERROR);
			return false;
		}
	}

	return true;
}

void DoEvents()
{
	MSG msg;
	while (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
	{
		if (msg.message == WM_QUIT)
		{
			return;
		}

		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
}

void FillData(Joiner* jn)
{
	int filesNum = 0;

	oFileName = txtDestination.GetText();
	jn->setOutputFile(txtDestination.GetText()); //memoryBlock->Rows[0];
	jn->setOutputSize(StringUtils::cDbl(memoryBlock->Rows[1]));
	filesNum = StringUtils::cInt(memoryBlock->Rows[3]);

	jn->Parts.clear();
	jn->Sizes.clear();
	for (int i = 4; i < (filesNum * 2) + 3; i += 2)
	{
		jn->Parts.push_back(IoUtils::Combine(IoUtils::CurrentPath(), memoryBlock->Rows[i]));
		jn->Sizes.push_back(StringUtils::cLong(memoryBlock->Rows[i + 1]));
	}
}

unsigned __stdcall threadfunc(void* param)
{
	Joiner* join = new Joiner(_mHInstance, _mHandle, &progressBar, &lblProgressPercentage, _currentPart);

	SetControls(true);
	FillData(join);
	join->Join();

	if (join->HasFinished())
	{
		lblProgressMessage.SetText("Files Successfully Joined.");
	}
	else
	{
		SetControls(false);
		UpdateWindow(_mHandle);
		lblProgressPercentage.SetText("");
		progressBar.SetValue(0);
		DeleteFile(join->getOutputFile().c_str());
	}

	InterlockedDecrement(&joinThreadIsActive);
	_endthreadex(0);
	return 0;
}

unsigned __stdcall monitorThread(void* param)
{
	//int pv = 0;
	//while(joinThreadIsActive)
	//{
	//	pv = join->GetProgress();
	//	progressBar.SetValue(pv);	
	//	if(pv != oldProgress)
	//	{
	//		lblProgressPercentage.SetText(StringUtils::Concat(StringUtils::cStr(pv)," %").c_str());
	//		oldProgress = pv;
	//	}
	//}

	//_endthreadex(0);
	return 0;
}

void RunTread()
{
	joinThread = new Thread(threadfunc);
	joinThread->Start();
	//joinThread->SetPriority(THREAD_PRIORITY_HIGHEST);
}

INT_PTR CALLBACK Comment(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	int wmId;
	int wmEvent;

	switch (message)
	{
	case WM_INITDIALOG:
		WndUtils::CenterForm(hDlg);
		SetDlgItemText(hDlg,DC_Edit, mComment.c_str());
		return INT_PTR(TRUE);
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);

		switch (wmId)
		{
		case IDOK:
			EndDialog(hDlg, LOWORD(wParam));
			return INT_PTR(TRUE);
		default: ;
		}

		break;
	}

	return INT_PTR(FALSE);
}

INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	int wmId;
	int wmEvent;

	StaticText lblName;
	StaticText lblCopyright;

	switch (message)
	{
	case WM_INITDIALOG:
		WndUtils::CenterForm(hDlg);
		lblName.Create(75, 15, 150, 15, hDlg, hInst, "Q Join v 1.0");
		lblCopyright.Create(75, 32, 200, 15, hDlg, hInst, "Copyright (c) 2009 Mohamed El-Bably");
		SetDlgItemText(hDlg,DLGABOUT_SYSLINK,
		               "<A HREF=\"https://github.com/m-elbably\">https://github.com/m-elbably</A>");

		return INT_PTR(TRUE);
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);

		switch (wmId)
		{
		case IDOK:
		case IDCANCEL:
			EndDialog(hDlg, LOWORD(wParam));
			return INT_PTR(TRUE);
		}

		break;
	case WM_NOTIFY:
		switch (LPNMHDR(lParam)->code)
		{
		case NM_CLICK:
		case NM_RETURN:
			ShellExecute(NULL, "open", "https://github.com/m-elbably", NULL, NULL, SW_SHOW);
			return INT_PTR(TRUE);
		default: ;
		}

		break;
	default: ;
	}

	return INT_PTR(FALSE);
}

//Commands Footer
//1- Signature
//2- Original File Name
//3- Original File Size
//4- First Part Name
//5- Parts Number
//6- {
//   P% Name
//   P% Size
//   }

const int realSize = 193536;

string GetBinaryStub()
{
	const HANDLE buff = 0;
	HANDLE file = NULL;
	DWORD buffer = 0;
	char* namebuffer = new char[255];

	GetModuleFileName(GetModuleHandle(0), LPCH(namebuffer), 255);
	file = CreateFileA(namebuffer, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, buff);
	SetFilePointer(file, realSize, 0,FILE_BEGIN);

	unsigned int fSize = 0;
	unsigned int stubSize = 0;
	const bool fsResult = GetFileSizeEx(file, PLARGE_INTEGER(&fSize));
	if (fsResult && fSize > realSize)
	{
		stubSize = (fSize - realSize) + 1;
	}

	// string sSize = StringUtils::cStr(stubSize);
	// MessageBox(_mHandle, sSize.c_str(), "Q Join",MB_ICONINFORMATION);

	if (stubSize <= 0)
	{
		return "";
	}

	const LPVOID memory = VirtualAlloc(0, stubSize, MEM_COMMIT, PAGE_READWRITE);
	ReadFile(file, memory, stubSize, LPDWORD(&buffer),NULL);

	TCHAR* pStub = new TCHAR[stubSize];
	memcpy_s(pStub, stubSize, memory, stubSize);
	VirtualFree(memory, 0, MEM_RELEASE);
	CloseHandle(file);

	if (pStub != NULL)
	{
		std::ostringstream oss;
		oss << pStub;
		return oss.str();
	}
	else
	{
		return "";
	}
}

bool ParseBinaryStub()
{
	const string stubData = GetBinaryStub();
	if (stubData.empty() || stubData.length() < 4)
	{
		DisableControls();
		return false;
	}

	if (stubData.substr(0, 4) != "QSH?")
	{
		DisableControls();
		return false;
	}

	memoryBlock->ParseData(stubData);
	fNum = StringUtils::cInt(memoryBlock->Rows[3]);

	cOutPathIndex = (fNum * 2) + 4; //outpath Index
	cIndex = (fNum * 2) + 5; //Comment Index

	if (cOutPathIndex < memoryBlock->Rows.size())
		mOutPath = memoryBlock->Rows[cOutPathIndex];

	if (cIndex < memoryBlock->Rows.size())
		mComment = memoryBlock->Rows[cIndex];

	string mExt = IoUtils::GetFileExt(memoryBlock->Rows[0]);
	mDefaultExt = mExt + '\0';
	mFilter = mExt + " File" + '\0' + "*." + mExt + '\0';
	txtSource.SetText(IoUtils::Combine(IoUtils::CurrentPath(), memoryBlock->Rows[2]).c_str());

	if (mOutPath.length() > 3)
		txtDestination.SetText(IoUtils::Combine(mOutPath, memoryBlock->Rows[0]).c_str());
	else
		txtDestination.SetText(IoUtils::Combine(IoUtils::CurrentPath(), memoryBlock->Rows[0]).c_str());

	return true;
}
