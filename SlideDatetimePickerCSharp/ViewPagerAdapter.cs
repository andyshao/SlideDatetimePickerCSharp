using Android.Support.V4.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlideDatetimePickerCSharp
{
    public class ViewPagerAdapter : FragmentPagerAdapter
    {
        private SlideDateTimeDialogFragment mDialogFragment;

        public ViewPagerAdapter(FragmentManager fm, SlideDateTimeDialogFragment dialogFragment)
            : base(fm)
        {
            this.mDialogFragment = dialogFragment;
        }

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    {
                        DateFragment dateFragment = DateFragment.NewInstance(
                            mDialogFragment.MyTheme,
                            mDialogFragment.Calendar.Year,
                            mDialogFragment.Calendar.Month -1,
                            mDialogFragment.Calendar.Day,
                            mDialogFragment.MinDate,
                            mDialogFragment.MaxDate);
					dateFragment.SetTargetFragment (mDialogFragment, 200);
                        return dateFragment;
                    }
                case 1:
                    {
                        TimeFragment timeFragment = TimeFragment.NewInstance(
                            mDialogFragment.MyTheme,
                            mDialogFragment.Calendar.Hour,
                            mDialogFragment.Calendar.Minute,
                            mDialogFragment.IsClientSpecified24HourTime,
                            mDialogFragment.Is24HourTime);
                        timeFragment.SetTargetFragment(mDialogFragment, 200);
                        return timeFragment;
                    }
                default:
                    return null;
            }
        }

        public override int Count
        {
            get { return 2; }
        }
    }
}
