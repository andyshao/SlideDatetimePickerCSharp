using System;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Support.V4.App;

namespace SlideDatetimePickerCSharp
{
    public class DateFragment : Fragment, CustomDatePicker.IOnDateChangedListener
    {
        private IDateChangedListener mCallback;
        private CustomDatePicker mDatePicker;

        public DateFragment() { }

        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                mCallback = (IDateChangedListener)TargetFragment;
            }
            catch (Java.Lang.ClassCastException)
            {
                throw new Java.Lang.ClassCastException("Calling fragment must implement " +
                "IDateChangedListener interface");
            }
        }

        public static DateFragment NewInstance(int theme, int year, int month,
            int day, DateTime minDate, DateTime maxDate)
        {
            DateFragment t = new DateFragment();

            Bundle b = new Bundle();
            b.PutInt("theme", theme);
            b.PutInt("year", year);
            b.PutInt("month", month);
            b.PutInt("day", day);
            b.PutLong("minDate", minDate.Ticks);
            b.PutLong("maxDate", maxDate.Ticks);
            t.Arguments = b;

            return t;
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            int theme = Arguments.GetInt("theme");
            int initialYear = Arguments.GetInt("year");
            int initialMonth = Arguments.GetInt("month");
            int initialDay = Arguments.GetInt("day");
			DateTime? minDate = null;
			DateTime? maxDate = null;

			if (Arguments.GetLong ("minDate", 0) > 0) {
				minDate = new DateTime (Arguments.GetLong ("minDate"));
			}
			if (Arguments.GetLong ("maxDate", 0) > 0) {
				maxDate = new DateTime (Arguments.GetLong ("maxDate"));
			}

            //获取指定主题样式的上下文
            Context contextThemeWrapper = new ContextThemeWrapper(
                                              Activity,
                                              theme == SlideDateTimePicker.HOLO_DARK ? Android.Resource.Style.ThemeHolo : Android.Resource.Style.ThemeHoloLight);

            LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

            View v = localInflater.Inflate(Resource.Layout.Fragment_Date, container, false);

            mDatePicker = v.FindViewById<CustomDatePicker>(Resource.Id.datePicker);
            mDatePicker.Init(initialYear, initialMonth, initialDay, this);
			mDatePicker.DescendantFocusability = DescendantFocusability.BlockDescendants;

            if (minDate != null)
            {
				mDatePicker.MinDate = ConvertDateTimeLong(minDate.Value);
            }

            if (maxDate != null)
            {
				mDatePicker.MaxDate = ConvertDateTimeLong(maxDate.Value);
            }

            return v;
        }

        private long ConvertDateTimeLong(DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 7));
            return timeStamp;
        }

        private DateTime ConvertLongDateTime(long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long iTime = long.Parse(d + "0000000");
            TimeSpan toNow = new TimeSpan(iTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        public void OnDateChanged(Android.Widget.DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            mCallback.OnDateChanged(year, monthOfYear, dayOfMonth);
        }
    }
}

