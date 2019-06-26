#include "Joiner.h"
#include <utility>

Joiner::Joiner(HINSTANCE hInstance, HWND parent, ProgressBar* progressBar, StaticText* progressText, string currentPart)
{
	_fileHandle = nullptr;
	_endThread = 0;
	_hInstance = hInstance;
	_parent = parent;

	_currentPart = std::move(currentPart);
	_progressBar = progressBar;
	_progressText = progressText;

	_progress = 0;
	_isActive = false;
	_stop = false;
	_finished = false;

	_outputFile = "";
	_outputSize = 0;
}

long Joiner::GetHeaderLen()
{
	MemoryBlock hdr(Parts[0]);

	if (!hdr.LoadData())
		return -1;

	return hdr.GetLength();
}

string Joiner::GetNextFile() const
{
	return NextPart::ShowDialog(_hInstance, _parent, _currentPart);
}

string Joiner::getOutputFile() const
{
	return _outputFile;
}

void Joiner::setOutputFile(string outputFile)
{
	_outputFile = std::move(outputFile);
}

void Joiner::setOutputSize(double outputSize)
{
	_outputSize = outputSize;
}

void Joiner::Join()
{
	int bSize = 0;
	unsigned long fLen = 0;
	double pos = 0;
	double mPos = 0;
	long stIndex = 0;

	bool bx = false;
	string cPath; //Current Part Path
	int cIndex = 0; //Current Part index

	string tmp;
	_isActive = true;
	char* buffer = new char[_bufferSize];
	ofstream pDest(_outputFile.c_str(), ofstream::binary);
	_fileHandle = &pDest;
	ifstream pBuffer;

	_progress = 0;

	_stop = false;
	_finished = false;

	if (!IoUtils::IsAccessible(_outputFile))
	{
		MessageBox(_parent, "Unable to write data, selected directory is inaccessible", "Error",MB_ICONERROR);
		pDest.close();
		_isActive = false;
		return;
	}

	while (cIndex < Parts.size())
	{
		if (_stop || _endThread)
		{
			pDest.close();
			break;
		}

		_currentPart = IoUtils::GetFileName(Parts.at(cIndex));
		if (!IoUtils::FileExists(Parts.at(cIndex)))
		{
			tmp = GetNextFile();
			if (IoUtils::FileExists(tmp))
				Parts[cIndex] = tmp;
			else
			{
				MessageBox(_parent, "Required part is missing, Operation failed", "Error",MB_ICONERROR);
				pDest.close();
				_isActive = false;
				return;
			}
		}

		cPath = Parts.at(cIndex);
		pBuffer.open(cPath.c_str(), ifstream::binary);

		if (cIndex == 0)
		{
			stIndex = GetHeaderLen();

			if (stIndex <= 0)
			{
				string msg = "Part \"" + _currentPart + "\" is invalid.";
				MessageBox(_parent, msg.c_str(), "Error",MB_ICONERROR);
				pDest.close();
				_isActive = false;
				return;
			}

			pBuffer.seekg(stIndex);
			pos = stIndex;
		}

		fLen = IoUtils::GetFileLen(cPath);

		while (pos < fLen)
		{
			if (_stop || _endThread)
			{
				pDest.close();
				break;
			}

			if (_bufferSize > fLen - pos)
			{
				bSize = int(fLen - pos);
			}
			else
			{
				bSize = _bufferSize;
			}

			pBuffer.read(buffer, bSize);
			pDest.write(buffer, bSize);

			pos += bSize;
			mPos += bSize;

			_progress = mPos / _outputSize * 100;
			_progressBar->SetValue(_progress);
			_progressText->SetText(StringUtils::Concat(StringUtils::cStr(_progress), " %").c_str());
		}

		pBuffer.close();

		cIndex++;
		pos = 0;
	}

	delete[] buffer;
	pDest.close();

	_isActive = false;
	_finished = true;
}

int Joiner::GetProgress() const
{
	return _progress;
}

bool Joiner::IsActive() const
{
	return _isActive;
}

void Joiner::Stop()
{
	_stop = true;
}

bool Joiner::HasFinished() const
{
	return _finished;
}


void Joiner::Reset()
{
	_progress = 0;
}
