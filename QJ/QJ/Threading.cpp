#include "Threading.h"

Thread::Thread(unsigned (__stdcall *t)(void*))
{
	_t = t;
	_started = false;
	_threadHandle = 0;
}

Thread::~Thread()
{
	if (_threadHandle != 0)
		::CloseHandle(_threadHandle);
}

void Thread::Start()
{
	if (_started)
		throw ThreadException("Thread already started.");

	if (!_started && _threadHandle != 0)
		::CloseHandle(_threadHandle);

	if (_t == 0)
		throw ThreadException("An object implementing the IRunnable interface required.");


	unsigned int threadID = 0;
	_threadHandle = (HANDLE)_beginthreadex(NULL, 0, _t,NULL, 0, &threadID);
	_started = true;
	if (_threadHandle == 0)
		throw ThreadException(::GetLastError());
}

void Thread::SetPriority(int priority) const
{
	SetThreadPriority(_threadHandle, priority);
}

int Thread::GetPriority() const
{
	return GetThreadPriority(_threadHandle);
}


void Thread::Stop() const
{
	checkAlive();
	_endthreadex(0);
}

void Thread::Suspend() const
{
	checkAlive();
	checkThreadHandle();
	if (::SuspendThread(_threadHandle) == -1)
		throw ThreadException(::GetLastError());
}

void Thread::Resume() const
{
	checkAlive();
	checkThreadHandle();
	if (::ResumeThread(_threadHandle) == -1)
		throw ThreadException(::GetLastError());
}

void Thread::Join(const unsigned long timeOut) const
{
	checkThreadHandle();
	if (IsAlive())
	{
		const DWORD waitResult = ::WaitForSingleObject(_threadHandle, timeOut);
		if (waitResult == WAIT_FAILED)
			throw ThreadException(::GetLastError());
	}
}

bool Thread::IsAlive() const
{
	return _started;
}

HANDLE Thread::GetHandle() const
{
	return _threadHandle;
}
