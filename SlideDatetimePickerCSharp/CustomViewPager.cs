using System;
using Android.Support.V4.View;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;

namespace SlideDatetimePickerCSharp
{
    public class CustomViewPager : ViewPager
    {
        private DatePicker mDatePicker;
        private TimePicker mTimePicker;
        private float x1, y1, x2, y2;
        private float mTouchSlop;

        public CustomViewPager(Context context)
			: this(context,null){}

		public CustomViewPager(Context context,IAttributeSet attrs)
			:base(context,attrs)
		{
			Init (context);
		}

        private void Init(Context context)
        {
            mTouchSlop = ViewConfiguration.Get(context).ScaledPagingTouchSlop;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            if (ChildCount > 0)
            {
                View childView = GetChildAt(0);

                if (childView != null)
                {
                    childView.Measure(widthMeasureSpec, heightMeasureSpec);
                    int h = childView.MeasuredHeight;
                    SetMeasuredDimension(MeasuredWidth, h);
                    LayoutParameters.Height = h;
                }
            }

            mDatePicker = FindViewById<DatePicker>(Resource.Id.datePicker);
            mTimePicker = FindViewById<TimePicker>(Resource.Id.timePicker);
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    {
                        x1 = e.RawX;
                        y1 = e.RawY;
                    }
                    break;
                case MotionEventActions.Move:
                    {
                        x2 = e.RawX;
                        y2 = e.RawY;

                        if (IsScrollingHorizontal(x1, y2, x2, y2))
                        {
                            return base.DispatchTouchEvent(e);
                        }
                    }
                    break;
            }

            switch (CurrentItem)
            {
                case 0:
                    {
                        if (mDatePicker != null)
                            mDatePicker.DispatchTouchEvent(e);
                    }
                    break;
                case 1:
                    {
                        if (mTimePicker != null)
                            mTimePicker.DispatchTouchEvent(e);
                    }
                    break;
            }

            return base.DispatchTouchEvent(e);
        }

        private bool IsScrollingHorizontal(float x1, float y1, float x2, float y2)
        {
            float deltaX = x2 - x1;
            float deltaY = y2 - y1;

            if (Math.Abs(deltaX) > mTouchSlop && Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                return true;
            }
            return false;
        }
    }
}

