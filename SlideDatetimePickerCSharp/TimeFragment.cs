using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Text.Format;

namespace SlideDatetimePickerCSharp
{
	public class TimeFragment : Fragment,TimePicker.IOnTimeChangedListener,NumberPicker.IOnValueChangeListener
	{
		private ITimeChangedListener mCallBack;
		private TimePicker mTimePicker;

		public TimeFragment(){
		}

		public override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			try
			{
				mCallBack = (ITimeChangedListener)TargetFragment;
			}
			catch(Java.Lang.ClassCastException) {
				throw new Java.Lang.ClassCastException ("Calling fragment must implement ");
			}
		}

		public static TimeFragment NewInstance(int theme,int hour,int minute,
			bool isClientSpecified24HourTime,bool is24HourTime)
		{
			TimeFragment t = new TimeFragment ();
			Bundle b = new Bundle ();
			b.PutInt ("theme", theme);
			b.PutInt ("hour", hour);
			b.PutInt ("minute", minute);
			b.PutBoolean ("isClientSpecified24HourTime", isClientSpecified24HourTime);
			b.PutBoolean ("is24HourTime", is24HourTime);
			t.Arguments = b;

			return t;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			int theme = Arguments.GetInt ("theme");
			int initialHour = Arguments.GetInt ("hour");
			int initialMinute = Arguments.GetInt ("minute");
			bool isClientSpecified24HourTime = Arguments.GetBoolean ("isClientSpecified24HourTime");
			bool is24HourTime = Arguments.GetBoolean ("is24HourTime");

			Context contextThemeWrapper = new ContextThemeWrapper (
				                              Activity,
				                              theme == SlideDateTimePicker.HOLO_DARK ? Android.Resource.Style.ThemeHolo : Android.Resource.Style.ThemeHoloLight);

			LayoutInflater localInflater = inflater.CloneInContext (contextThemeWrapper);

			View v = localInflater.Inflate (Resource.Layout.Fragment_Time, container, false);

			mTimePicker = v.FindViewById<TimePicker> (Resource.Id.timePicker);
			mTimePicker.DescendantFocusability = DescendantFocusability.BlockDescendants;
			mTimePicker.SetOnTimeChangedListener (this);

			if (isClientSpecified24HourTime) {
				mTimePicker.Is24HourView = is24HourTime;
			} else {
				mTimePicker.Is24HourView = DateFormat.Is24HourFormat (TargetFragment.Activity);
			}

			mTimePicker.CurrentHour = initialHour;
			mTimePicker.CurrentMinute = initialMinute;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich
			   && Build.VERSION.SdkInt <= BuildVersionCodes.IceCreamSandwichMr1) {
				FixTimePickerBug18982 ();
			}

			return v;
		}

		public void OnTimeChanged (TimePicker view, int hourOfDay, int minute)
		{
			mCallBack.OnTimeChanged (hourOfDay, minute);
		}

		private void FixTimePickerBug18982()
		{
			View amPmView = ((ViewGroup)mTimePicker.GetChildAt (0)).GetChildAt (3);

			if (amPmView is NumberPicker) {
				((NumberPicker)amPmView).SetOnValueChangedListener (this);
			}
		}

		public void OnValueChange (NumberPicker picker, int oldVal, int newVal)
		{
			if (picker.Value == 1) {
				if (mTimePicker.CurrentHour < 12) {
					mTimePicker.CurrentHour = mTimePicker.CurrentHour + 12;
				}
			} else {
				if (mTimePicker.CurrentHour >= 12) {
					mTimePicker.CurrentHour = mTimePicker.CurrentHour - 12;
				}
			}

			mCallBack.OnTimeChanged (mTimePicker.CurrentHour, mTimePicker.CurrentMinute);
		}
	}
}

