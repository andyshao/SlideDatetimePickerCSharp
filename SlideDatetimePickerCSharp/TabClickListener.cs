using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlideDatetimePickerCSharp
{
    public class TabClickListener : Java.Lang.Object, Android.Views.View.IOnClickListener
    {
        private SlidingTabLayout mTabLayout;

        public TabClickListener(SlidingTabLayout tabLayout)
        {
            this.mTabLayout = tabLayout;
        }

        public void OnClick(Android.Views.View v)
        {
            for (int i = 0; i < mTabLayout.TabStrip.ChildCount; i++)
            {
                if (v == mTabLayout.TabStrip.GetChildAt(i))
                {
                    mTabLayout.ViewPager.CurrentItem = i;
                    return;
                }
            }
        }
    }
}
