#pragma once

#ifndef __THREAD_H__
#define __THREAD_H__

#include <windows.h>
#include <string>
#include <process.h>

class ThreadException
{
public:
	ThreadException(DWORD lastError)
	{
		HLOCAL msgBuffer;
		::FormatMessage(
			FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			nullptr, lastError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			LPSTR(&msgBuffer), 0, NULL);
		Message = LPSTR(msgBuffer);
		::LocalFree(msgBuffer);
	}

	ThreadException(const std::string& msg) { Message = msg; }
	std::string Message;
};

class Thread
{
public:
	Thread(unsigned (__stdcall *t)(void*));
	void Start();
	void SetPriority(int) const;
	int GetPriority() const;
	void Stop() const;
	void Suspend() const;
	void Resume() const;
	void Join(unsigned long timeOut = INFINITE) const;
	bool IsAlive() const;
	HANDLE GetHandle() const;
	~Thread();

protected:
	bool _started;
	HANDLE _threadHandle;
	unsigned (__stdcall *_t)(void*);

	void checkThreadHandle() const
	{
		if (_threadHandle == 0)
			throw ThreadException("Thread not yet created, call the start() method.");
	}

	void checkAlive() const
	{
		if (!IsAlive())
			throw ThreadException("No Thread alive.");
	}
};

#endif // __THREAD_H__
