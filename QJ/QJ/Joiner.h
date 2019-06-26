#pragma once

#include <vector>
#include "MemoryBlock.h"
#include "ProgressBar.h"
#include "StaticText.h"
#include "NextPart.h"

class Joiner
{
	static const int _bufferSize = 524288;

	string _currentPart;
	HINSTANCE _hInstance;
	HWND _parent;
	ProgressBar* _progressBar;
	StaticText* _progressText;

	ofstream* _fileHandle;
	volatile long _endThread;
	long _progress;
	bool _isActive;
	bool _stop;
	bool _finished;
	string _outputFile;
	double _outputSize;

	long GetHeaderLen();
	string GetNextFile() const;
public:
	Joiner(HINSTANCE hInstance, HWND parent, ProgressBar* progressBar, StaticText* progressText, string currentPart);
	vector<string> Parts;
	vector<long> Sizes;
	string getOutputFile() const;
	void setOutputFile(string);
	void setOutputSize(double);
	void Join();
	int GetProgress() const;
	bool IsActive() const;
	void Reset();
	void Stop();
	bool HasFinished() const;
};
