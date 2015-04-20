using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SlideDatetimePickerCSharp;
using Android.Support.V4.App;

namespace SampleActivity
{
    [Activity(Label = "SampleActivity", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FragmentActivity, SlideDateTimeListener
	{
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
				new SlideDateTimePicker.Builder(SupportFragmentManager)
					.SetListener(this)
					.SetIs24HourTime(true)
					.SetInitialDate(DateTime.Now)
					.Build()
					.Show();
            };
        }

		#region SlideDateTimeListener implementation

		public void OnDateTimeSet (DateTime date)
		{
			Toast.MakeText (this, date.ToString (), ToastLength.Long).Show ();
		}

		public void OnDateTimeCancel ()
		{
			Toast.MakeText (this, "Canceled", ToastLength.Long).Show ();
		}

		#endregion
    }
}

