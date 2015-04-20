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

        public SlidingTabStrip(Context context)
            : this(context, null) { }

        public SlidingTabStrip(Context context, IAttributeSet attrs)
            : this(context, attrs, 0) { }

        public SlidingTabStrip(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {

        }
    }
}