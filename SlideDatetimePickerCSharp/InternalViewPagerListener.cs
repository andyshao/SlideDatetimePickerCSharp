using Android.Support.V4.View;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlideDatetimePickerCSharp
{
    public class InternalViewPagerListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
    {
        private int mScrollState;
        private SlidingTabLayout mTabLayout;

        public InternalViewPagerListener(SlidingTabLayout tablayout)
        {
            this.mTabLayout = tablayout;
        }

        #region IOnPageChangeListener

        public void OnPageScrollStateChanged(int state)
        {
            mScrollState = state;

            if (mTabLayout.ViewPagerPageChangeListener != null)
            {
                mTabLayout.ViewPagerPageChangeListener.OnPageScrollStateChanged(state);
            }
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            int tabStripChildCount = mTabLayout.TabStrip.ChildCount;
            if ((tabStripChildCount == 0) || (position < 0) || (position >= tabStripChildCount))
            {
                return;
            }

            mTabLayout.TabStrip.OnViewPagerPageChanged(position, positionOffset);

            View selectedTitle = mTabLayout.TabStrip.GetChildAt(position);
            int extraOffset = (selectedTitle != null) ? (int)(positionOffset * selectedTitle.Width) : 0;
            mTabLayout.ScrollToTab(position, extraOffset);

            if (mTabLayout.ViewPagerPageChangeListener != null)
            {
                mTabLayout.ViewPagerPageChangeListener.OnPageScrolled(position, positionOffset,
                    positionOffsetPixels);
            }
        }

        public void OnPageSelected(int position)
        {
            if (mScrollState == ViewPager.ScrollStateIdle)
            {
                mTabLayout.TabStrip.OnViewPagerPageChanged(position, 0f);
                mTabLayout.ScrollToTab(position, 0);
            }

            if (mTabLayout.ViewPagerPageChangeListener != null)
            {
                mTabLayout.ViewPagerPageChangeListener.OnPageSelected(position);
            }
        }

        #endregion
    }
}
