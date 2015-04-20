using System;
using Android.Widget;
using Android.Content;
using Android.Util;

namespace SlideDatetimePickerCSharp
{
    public class CustomTimePicker : TimePicker
    {
        private const String TAG = "CustomTimePicker";

        public CustomTimePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            try
            {
                var idClass = Java.Lang.Class.ForName("com.android.internal.R$id");

                var hourField = idClass.GetField("hour");
                var minuteField = idClass.GetField("minute");
                var amPmField = idClass.GetField("amPm");

                var hourNumberPicker = FindViewById<NumberPicker>(hourField.GetInt(null));
                var minuteNumberPicker = FindViewById<NumberPicker>(minuteField.GetInt(null));
                var amPmNumberPicker = FindViewById<NumberPicker>(amPmField.GetInt(null));

                var numberPickerClass = Java.Lang.Class.ForName("android.widget.NumberPicker");

                var selectionDividerField = numberPickerClass.GetDeclaredField("mSelectionDivider");
                selectionDividerField.Accessible = true;
                selectionDividerField.Set(hourNumberPicker, Resources.GetDrawable(Resource.Drawable.selection_divider));
                selectionDividerField.Set(minuteNumberPicker, Resources.GetDrawable(Resource.Drawable.selection_divider));
                selectionDividerField.Set(amPmNumberPicker, Resources.GetDrawable(Resource.Drawable.selection_divider));
            }
            catch (Java.Lang.ClassNotFoundException e)
            {
				Log.Error(TAG, e, "ClassNotFoundException in CustomTimePicker");
            }
            catch (Java.Lang.NoSuchFieldException e)
            {
				Log.Error(TAG, e, "NoSuchFieldException in CustomTimePicker");
            }
            catch (Java.Lang.IllegalAccessException e)
            {
				Log.Error(TAG, e, "IllegalAccessException in CustomTimePicker");
            }
            catch (Java.Lang.IllegalArgumentException e)
            {
				Log.Error(TAG, e, "IllegalArgumentException in CustomTimePicker");
            }
        }
    }
}

