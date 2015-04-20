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
using Android.Util;
using Android.Graphics;

namespace SlideDatetimePickerCSharp
{
    public class SlidingTabStrip : LinearLayout
    {
        private class SimpleTabColorizer : ITabColorizer
        {
            private int[] mIndicatorColors;
            private int[] mDividerColors;

            public int GetIndicatorColor(int position)
            {
                return mIndicatorColors[position % mIndicatorColors.Length];
            }

            public int GetDividerColor(int position)
            {
                return mDividerColors[position % mDividerColors.Length];
            }

            public void SetIndicatorColors(params int[] colors)
            {
                mIndicatorColors = colors;
            }

            public void SetDividerColors(params int[] colors)
            {
                mDividerColors = colors;
            }
        }

        private const int DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS = 2;
        private const byte DEFAULT_BOTTOM_BORDER_COLOR_ALPHA = 0x26;
        private const int SELECTED_INDICATOR_THICKNESS_DIPS = 6;
        private const uint DEFAULT_SELECTED_INDICATOR_COLOR = 0xff33b5e5;

        private const int DEFAULT_DIVIDER_THICKNESS_DIPS = 1;
        private const byte DEFAULT_DIVIDER_COLOR_ALPHA = 0x20;
        private const float DEFAULT_DIVIDER_HEIGHT = 0.5f;

        private int mBottomBorderThickness;
        private Paint mBottomBorderPaint;

        private int mSelectedIndicatorThickness;
        private Paint mSelectedIndicatorPaint;

        private int mDefaultBottomBorderColor;

        private Paint mDividerPaint;
        private float mDividerHeight;

        private int mSelectedPosition;
        private float mSelectionOffset;

        private ITabColorizer mCustomTabColorizer;
        private SimpleTabColorizer mDefaultTabColorizer;

        public SlidingTabStrip(Context context)
            : this(context, null) { }

        public SlidingTabStrip(Context context, IAttributeSet attrs)
            : this(context, attrs, 0) { }

        public SlidingTabStrip(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            SetWillNotDraw(false);

            float density = Resources.DisplayMetrics.Density;

            TypedValue outValue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorForeground, outValue, true);
            int themeForegroundColor = outValue.Data;

            mDefaultBottomBorderColor = SetColorAlpha(themeForegroundColor, DEFAULT_BOTTOM_BORDER_COLOR_ALPHA);
        }

        public void SetCustomTabColorizer(ITabColorizer customTabColorizer)
        {
            mCustomTabColorizer = customTabColorizer;
            Invalidate();
        }

        public void SetSelectedIndicatorColors(params int[] colors)
        {
            mCustomTabColorizer = null;
            mDefaultTabColorizer.SetIndicatorColors(colors);
            Invalidate();
        }

        public void SetDividerColors(params int[] colors)
        {
            mCustomTabColorizer = null;
            mDefaultTabColorizer.SetDividerColors(colors);
            Invalidate();
        }

        public void OnViewPagerPageChanged(int position, float positionOffset)
        {
            mSelectedPosition = position;
            mSelectionOffset = positionOffset;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            int height = Height;
            int childCount = ChildCount;
            int dividerHeightPx = (int)(Math.Min(Math.Max(0f, mDividerHeight), 1f) * height);
            ITabColorizer tabColorizer = mCustomTabColorizer != null ? mCustomTabColorizer : mDefaultTabColorizer;

            if (childCount > 0)
            {
                View selectedTitle = GetChildAt(mSelectedPosition);
                int left = selectedTitle.Left;
                int right = selectedTitle.Right;
                int color = tabColorizer.GetIndicatorColor(mSelectedPosition);

                if (mSelectionOffset > 0f && mSelectedPosition < (childCount - 1))
                {
                    int nextColor = tabColorizer.GetIndicatorColor(mSelectedPosition + 1);
                    if (color != nextColor)
                    {
                        color = BlendColors(nextColor, color, mSelectionOffset);
                    }

                    View nextTitle = GetChildAt(mSelectedPosition + 1);
                    left = (int)(mSelectionOffset * nextTitle.Left + (1.0f - mSelectionOffset) * left);
                    right = (int)(mSelectionOffset * nextTitle.Right + (1.0f - mSelectionOffset) * right);
                }

                mSelectedIndicatorPaint.Color = new Color(color);

                canvas.DrawRect(left, height - mSelectedIndicatorThickness, right, height, mSelectedIndicatorPaint);
            }

            canvas.DrawRect(0, height - mBottomBorderThickness, Width, height, mBottomBorderPaint);

            int separatorTop = (height - dividerHeightPx) / 2;
            for (int i = 0; i < childCount - 1; i++)
            {
                View child = GetChildAt(i);
                mDividerPaint.Color = new Color(tabColorizer.GetDividerColor(i));
                canvas.DrawLine(child.Right, separatorTop, child.Right,
                    separatorTop + dividerHeightPx, mDividerPaint);
            }
        }

        private int SetColorAlpha(int color, byte alpha)
        {
            return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private int BlendColors(int color1, int color2, float ratio)
        {
            float inverseRation = 1f - ratio;
            float r = (Color.GetRedComponent(color1) * ratio) + (Color.GetRedComponent(color2) * inverseRation);
            float g = (Color.GetGreenComponent(color1) * ratio) + (Color.GetGreenComponent(color2) * inverseRation);
            float b = (Color.GetBlueComponent(color1) * ratio) + (Color.GetBlueComponent(color2) * inverseRation);
            return Color.Rgb((int)r, (int)g, (int)b);
        }
    }
}