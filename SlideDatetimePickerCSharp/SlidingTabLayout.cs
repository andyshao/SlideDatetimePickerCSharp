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
using Android.Graphics;

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

        public ViewPager ViewPager
        {
            get
            {
                return mViewPager;
            }
        }

        public SlidingTabStrip TabStrip
        {
            get
            {
                return mTabStrip;
            }
        }

        public int TitleOffset
        {
            get
            {
                return mTitleOffset;
            }
        }

        public ViewPager.IOnPageChangeListener ViewPagerPageChangeListener
        {
            get
            {
                return this.mViewPagerPageChangeListener;
            }
        }

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

        public void SetCustomTabColorizer(ITabColorizer tabColorizer)
        {
            mTabStrip.SetCustomTabColorizer(tabColorizer);
        }

        public void SetSelectedIndicatorColor(params int[] colors)
        {
            mTabStrip.SetSelectedIndicatorColors(colors);
        }

        public void SetDividerColors(params int[] colors)
        {
            mTabStrip.SetDividerColors(colors);
        }

        public void SetOnPageChangeListener(ViewPager.IOnPageChangeListener listener)
        {
            mViewPagerPageChangeListener = listener;
        }

        public void SetCustomTabView(int layoutResId, int textViewId)
        {
            mTabViewLayoutId = layoutResId;
            mTabViewTextViewId = textViewId;
        }

        public void SetViewPager(ViewPager viewPager)
        {
            mTabStrip.RemoveAllViews();

            mViewPager = viewPager;
            if (viewPager != null)
            {
                viewPager.SetOnPageChangeListener(new InternalViewPagerListener(this));
                PopulateTabStrip();
            }
        }

        protected TextView CreateDefaultTabView(Context context)
        {
            TextView textView = new TextView(context);
            textView.Gravity = GravityFlags.Center;
            textView.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
            textView.Typeface = Typeface.DefaultBold;

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Honeycomb)
            {
                TypedValue outValue = new TypedValue();
                Context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground,
                    outValue, true);
                textView.SetBackgroundResource(outValue.ResourceId);
            }

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.IceCreamSandwich)
            {
                textView.SetAllCaps(true);
            }

            int padding = (int)(TAB_VIEW_PADDING_DIPS * Resources.DisplayMetrics.Density);
            textView.SetPadding(padding, padding, padding, padding);
            return textView;
        }

        private void PopulateTabStrip()
        {
            PagerAdapter adapter = mViewPager.Adapter;
            IOnClickListener tabClickListener = new TabClickListener(this);

            for (int i = 0; i < adapter.Count; i++)
            {
                View tabView = null;
                TextView tabTitleView = null;

                if (mTabViewLayoutId != 0)
                {
                    tabView = LayoutInflater.From(Context).Inflate(mTabViewLayoutId, mTabStrip, false);
                    tabTitleView = tabView.FindViewById<TextView>(mTabViewTextViewId);
                }

                if (tabView == null)
                {
                    tabView = CreateDefaultTabView(Context);
                }

                if (tabTitleView == null && tabView is TextView)
                {
                    tabTitleView = (TextView)tabView;
                }

                tabTitleView.Text = adapter.GetPageTitle(i);
                tabView.SetOnClickListener(tabClickListener);

                mTabTitleViews.Add(i, tabTitleView);
                mTabStrip.AddView(tabView);
            }
        }

        public void SetTabText(int index, String text)
        {
            TextView tv = null;
            mTabTitleViews.TryGetValue(index, out tv);

            if (tv != null)
            {
                tv.Text = text;
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (mViewPager != null)
            {
                ScrollToTab(mViewPager.CurrentItem, 0);
            }
        }

        public void ScrollToTab(int tabIndex, int positionOffset)
        {
            int tabStripChildCount = mTabStrip.ChildCount;
            if (tabStripChildCount == 0 || tabIndex < 0 || tabIndex >= tabStripChildCount)
            {
                return;
            }

            View selectedChild = mTabStrip.GetChildAt(tabIndex);
            if (selectedChild != null)
            {
                int targetScrollX = selectedChild.Left + positionOffset;

                if (tabIndex > 0 || positionOffset > 0)
                {
                    targetScrollX -= mTitleOffset;
                }
                ScrollTo(targetScrollX, 0);
            }
        }
    }
}