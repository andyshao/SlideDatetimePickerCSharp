using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using Android.Text.Format;
using Android.Graphics;
using Android.Support.V4.App;
using Android.App;

namespace SlideDatetimePickerCSharp
{
    public class SlideDateTimeDialogFragment : Android.Support.V4.App.DialogFragment, IDateChangedListener, ITimeChangedListener
    {
        public const String TAG_SLIDE_DATE_TIME_DIALOG_FRAGMENT = "tagSlideDateTimeDialogFragment";

        private static SlideDateTimeListener mListener;

        private Context mContext;
        private CustomViewPager mViewPager;
        private ViewPagerAdapter mViewPagerAdapter;
        private SlidingTabLayout mSlidingTabLayout;
        private View mButtonHorizontalDivider;
        private View mButtonVerticalDivider;
        private Button mOkButton;
        private Button mCancelButton;
        private DateTime mInitialDate;
        private int mTheme;
        private int mIndicatorColor;
        private DateTime mMinDate;
        private DateTime mMaxDate;
        private bool mIsClientSpecified24HourTime;
        private bool mIs24HourTime;
        private FormatStyleFlags mDateFlags = FormatStyleFlags.ShowWeekday | FormatStyleFlags.ShowDate | FormatStyleFlags.AbbrevAll;

        public bool IsClientSpecified24HourTime
        {
            get
            {
                return this.mIsClientSpecified24HourTime;
            }
        }

        public bool Is24HourTime
        {
            get
            {
                return this.mIs24HourTime;
            }
        }

        public DateTime MinDate
        {
            get
            {
                return mMinDate;
            }
        }

        public DateTime MaxDate
        {
            get
            {
                return mMaxDate;
            }
        }

        public DateTime Calendar
        {
            get
            {
                return mInitialDate;
            }
        }

        public int MyTheme
        {
            get
            {
                return mTheme;
            }
        }

        public SlideDateTimeDialogFragment() { }

        public static SlideDateTimeDialogFragment NewInstance(SlideDateTimeListener listener,
            DateTime initialDate, DateTime minDate, DateTime maxDate, bool isClientSpecified24HourTime,
            bool is24HourTime, int theme, int indicatorColor)
        {
            mListener = listener;

            SlideDateTimeDialogFragment dialogFragment = new SlideDateTimeDialogFragment();

            Bundle bundle = new Bundle();
            bundle.PutLong("initialDate", initialDate.Ticks);
            bundle.PutLong("minDate", minDate.Ticks);
            bundle.PutLong("maxDate", maxDate.Ticks);
            bundle.PutBoolean("isClientSpecified24HourTime", isClientSpecified24HourTime);
            bundle.PutBoolean("is24HourTime", is24HourTime);
            bundle.PutInt("theme", theme);
            bundle.PutInt("indicatorColor", indicatorColor);
            dialogFragment.Arguments = bundle;

            return dialogFragment;
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            mContext = activity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
            UnPackBundle();

            switch (mTheme)
            {
                case SlideDateTimePicker.HOLO_DARK:
                    {
                        SetStyle((int)DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeHoloDialogNoActionBar);
                    }
                    break;
                case SlideDateTimePicker.HOLO_LIGHT:
                    {
                        SetStyle((int)DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeHoloLightDialogNoActionBar);
                    }
                    break;
                default:
                    {
                        SetStyle((int)DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeHoloLightDialogNoActionBar);
                    }
                    break;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Slide_Date_Time_Picker, container);

            SetUpViews(view);
            CustomizeViews();
            InitViewPager();
            InitTabs();
            InitButtons();

            return view;
        }

        public override void OnDestroyView()
        {
            if (Dialog != null && RetainInstance)
            {
                Dialog.SetDismissMessage(null);
            }
            base.OnDestroyView();
        }

        private void UnPackBundle()
        {
            Bundle args = Arguments;

            mInitialDate = new DateTime(args.GetLong("initialDate"));
            mMinDate = new DateTime(args.GetLong("minDate"));
            mMaxDate = new DateTime(args.GetLong("maxDate"));
            mIsClientSpecified24HourTime = args.GetBoolean("isClientSpecified24HourTime");
            mIs24HourTime = args.GetBoolean("is24HourTime");
            mTheme = args.GetInt("theme");
            mIndicatorColor = args.GetInt("indicatorColor");
        }

        private void SetUpViews(View v)
        {
            mViewPager = v.FindViewById<CustomViewPager>(Resource.Id.viewPager);
            mSlidingTabLayout = v.FindViewById<SlidingTabLayout>(Resource.Id.slidingTabLayout);
            mButtonHorizontalDivider = v.FindViewById(Resource.Id.buttonHorizontalDivider);
            mButtonVerticalDivider = v.FindViewById(Resource.Id.buttonVerticalDivider);
            mOkButton = v.FindViewById<Button>(Resource.Id.okButton);
            mCancelButton = v.FindViewById<Button>(Resource.Id.cancelButton);
        }

        private void CustomizeViews()
        {
            int lineColor = mTheme == SlideDateTimePicker.HOLO_DARK ? Resources.GetColor(Resource.Color.gray_holo_dark) : Resources.GetColor(Resource.Color.gray_holo_light);

            switch (mTheme)
            {
                case SlideDateTimePicker.HOLO_LIGHT:
                case SlideDateTimePicker.HOLO_DARK:
                    {
                        mButtonHorizontalDivider.SetBackgroundColor(new Color(lineColor));
                        mButtonVerticalDivider.SetBackgroundColor(new Color(lineColor));
                    }
                    break;
                default:
                    {
                        mButtonHorizontalDivider.SetBackgroundColor(Resources.GetColor(Resource.Color.gray_holo_light));
                        mButtonVerticalDivider.SetBackgroundColor(Resources.GetColor(Resource.Color.gray_holo_light));
                    }
                    break;
            }

            if (mIndicatorColor != 0)
            {
                mSlidingTabLayout.SetSelectedIndicatorColor(mIndicatorColor);
            }
        }

        private void InitViewPager()
        {
			mViewPagerAdapter = new ViewPagerAdapter (ChildFragmentManager, this);
			mViewPager.Adapter = mViewPagerAdapter;

			mSlidingTabLayout.SetCustomTabView (Resource.Layout.Custom_Tab, Resource.Id.tabText);
			mSlidingTabLayout.SetViewPager (mViewPager);
        }

        private void InitButtons()
        {
            mOkButton.Click += (e, s) =>
                {
                    if (mListener == null)
                    {
                        throw new ArgumentNullException("Listener no longer exists for mOkButton");
                    }
                    mListener.OnDateTimeSet(mInitialDate);
                    Dismiss();
                };

            mCancelButton.Click += (e, s) =>
                {
                    if (mListener == null)
                    {
                        throw new ArgumentNullException("Listener no longer exists for mCancelButton");
                    }
                    mListener.OnDateTimeCancel();
                    Dismiss();
                };
        }

        private void UpdateTimeTab()
        {
            if (mIsClientSpecified24HourTime)
            {
                if (mIs24HourTime)
                {
					mSlidingTabLayout.SetTabText (1, mInitialDate.ToString ("HH:mm"));
                }
                else
                {
					mSlidingTabLayout.SetTabText (1, mInitialDate.ToString ("h:mm tt"));
                }
            }
            else
            {
				mSlidingTabLayout.SetTabText (1, mInitialDate.ToString ("h:mm tt"));
            }
        }

        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);

            if (mListener == null)
            {
                throw new ArgumentNullException("Listener no longer exists in onCalcel");
            }
            mListener.OnDateTimeCancel();
        }

        private void UpdateDateTab()
        {
            mSlidingTabLayout.SetTabText(0, mInitialDate.ToString("yyyyƒÍM‘¬d»’"));
        }

        private void InitTabs()
        {
            UpdateDateTab();
            UpdateTimeTab();
        }

        #region IDateChangeListener

        public void OnDateChanged(int year, int month, int day)
        {
			mInitialDate = new DateTime(year, month + 1, day, mInitialDate.Hour, mInitialDate.Minute, mInitialDate.Second);
            UpdateDateTab();
        }

        #endregion

        #region ITimeChangedListener

        public void OnTimeChanged(int hour, int minute)
        {
			mInitialDate = new DateTime(mInitialDate.Year, mInitialDate.Month, mInitialDate.Day, hour, minute, mInitialDate.Second);
            UpdateTimeTab();
        }

        #endregion
    }
}