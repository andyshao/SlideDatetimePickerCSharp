using System;

namespace SlideDatetimePickerCSharp
{
	public interface ITimeChangedListener
	{
		void OnTimeChanged(int hour,int minute);
	}
}

