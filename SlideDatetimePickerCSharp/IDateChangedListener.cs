using System;

namespace SlideDatetimePickerCSharp
{
	public interface IDateChangedListener
	{
		void OnDateChanged(int year,int month,int day);
	}
}

