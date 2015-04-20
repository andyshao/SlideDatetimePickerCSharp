using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Util;

namespace SlideDatetimePickerCSharp
{
    public class SlidingTabLayout : HorizontalScrollView
    {
        private const int TITLE_OFFSET_DIPS = 24;
        private const int TAB_VIEW_PADDING_DIPS = 16;
        private const int TAB_VIEW_TEXT_SIZE_SP = 12;

        private int mTitleOffset;

        private int mTabViewLayoutId;
        private int mTabViewTextViewId;

        private Dictionary<int, TextView> mTabTitleViews = new Dictionary<int, TextView>();

        private ViewPager mViewPager;
        private ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;

        private SlidingTabStrip mTabStrip;

        public SlidingTabLayout(Context context)
            : this(context, null) { }

        public SlidingTabLayout(Context context, IAttributeSet attrs)
            : this(context, attrs, 0) { }

        public SlidingTabLayout(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            HorizontalFadingEdgeEnabled = false;
            FillViewport = true;

            mTitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);

            mTabStrip = new SlidingTabStrip(context);
            AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.WrapContent);
        }
    }
}