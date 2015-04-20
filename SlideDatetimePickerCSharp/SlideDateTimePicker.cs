using Android.Support.V4.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlideDatetimePickerCSharp
{
    public class SlideDateTimePicker
    {
        public const int HOLO_DARK = 1;
        public const int HOLO_LIGHT = 2;

        private FragmentManager mFragmentManager;
        private SlideDateTimeListener mListener;
        private int mTheme;

        public bool Is24HourTime { get; set; }
        public int IndicatorColor { get; set; }
        public bool IsClientSpecified24HourTime { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public SlideDateTimePicker(FragmentManager fm)
        {
            FragmentTransaction ft = fm.BeginTransaction();
            Fragment prev = fm.FindFragmentByTag(SlideDateTimeDialogFragment.TAG_SLIDE_DATE_TIME_DIALOG_FRAGMENT);

            if (prev != null)
            {
                ft.Remove(prev);
                ft.Commit();
            }

            mFragmentManager = fm;
        }

        public void SetListener(SlideDateTimeListener listener)
        {
            mListener = listener;
        }

        public void SetTheme(int theme)
        {
            this.mTheme = theme;
        }

        public void Show()
        {
            if (mListener == null)
            {
                throw new ArgumentNullException("Attempting to bind null listener to SlideDateTimePicker");
            }

            if (InitialDate == null)
            {
                InitialDate = DateTime.Now;
            }

            SlideDateTimeDialogFragment dialogFragmen =
                SlideDateTimeDialogFragment.NewInstance(mListener,
                InitialDate, MinDate, MaxDate,
                IsClientSpecified24HourTime,
                Is24HourTime, mTheme, IndicatorColor);

            dialogFragmen.Show(mFragmentManager,
                SlideDateTimeDialogFragment.TAG_SLIDE_DATE_TIME_DIALOG_FRAGMENT);
        }

        public class Builder
        {
            private FragmentManager fm;
            private SlideDateTimeListener listener;

            private DateTime initialDate;
            private DateTime minDate;
            private DateTime maxDate;
            private bool isClientSpecified24HourTime;
            private bool is24HourTime;
            private int theme;
            private int indicatorColor;

            public Builder(FragmentManager fm)
            {
                this.fm = fm;
            }

            public Builder SetListener(SlideDateTimeListener listener)
            {
                this.listener = listener;
                return this;
            }

            public Builder SetInitialDate(DateTime initialDate)
            {
                this.initialDate = initialDate;
                return this;
            }

            public Builder SetMinDate(DateTime minDate)
            {
                this.minDate = minDate;
                return this;
            }

            public Builder SetMaxDate(DateTime maxDate)
            {
                this.maxDate = maxDate;
                return this;
            }

            public Builder SetIs24HourTime(bool is24HourTime)
            {
                this.isClientSpecified24HourTime = is24HourTime;
                this.is24HourTime = is24HourTime;
                return this;
            }

            public Builder SetTheme(int theme)
            {
                this.theme = theme;
                return this;
            }

            public Builder SetIndicatorColor(int indicatorColor)
            {
                this.indicatorColor = indicatorColor;
                return this;
            }

            public SlideDateTimePicker Build()
            {
                SlideDateTimePicker picker = new SlideDateTimePicker(fm);
                picker.SetListener(listener);
                picker.InitialDate = initialDate;
                picker.MinDate = minDate;
                picker.MaxDate = maxDate;
                picker.IsClientSpecified24HourTime = isClientSpecified24HourTime;
                picker.Is24HourTime = is24HourTime;
                picker.SetTheme(theme);
                picker.IndicatorColor = indicatorColor;
                return picker;
            }
        }
    }
}
