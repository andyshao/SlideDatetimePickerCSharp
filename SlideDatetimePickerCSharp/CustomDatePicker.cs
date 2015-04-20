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

namespace SlideDatetimePickerCSharp
{
    public class CustomDatePicker : DatePicker
    {
        private const String Tag = "CustomDatePicker";

        public CustomDatePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            try
            {
                var idClass = Java.Lang.Class.ForName("com.android.internal.R$id");
                var monthField = idClass.GetField("month");
                var dayField = idClass.GetField("day");
                var yearField = idClass.GetField("year");

                var monthNumberPicker = FindViewById<NumberPicker>(monthField.GetInt(null));
                var dayNumberPicker = FindViewById<NumberPicker>(dayField.GetInt(null));
                var yearNumberPicker = FindViewById<NumberPicker>(yearField.GetInt(null));

                var numberPickerClass = Java.Lang.Class.ForName("android.widget.NumberPicker");

                var selectionDividerField = numberPickerClass.GetDeclaredField("mSelectionDivider");
                selectionDividerField.Accessible = true;
                selectionDividerField.Set(monthNumberPicker, Resources.GetDrawable(Resource.Drawable.selection_divider));
                selectionDividerField.Set(dayNumberPicker, Resources.GetDrawable(Resource.Drawable.selection_divider));
                selectionDividerField.Set(yearNumberPicker, Resources.GetDrawable(Resource.Drawable.selection_divider));
            }
            catch (Java.Lang.ClassNotFoundException e)
            {
                Log.Error(Tag, e, "ClassNotFoundException in CustomDatePicker");
            }
            catch (Java.Lang.NoSuchFieldException e)
            {
                Log.Error(Tag, e, "NoSuchFieldException in CustomDatePicker");
            }
            catch (Java.Lang.IllegalAccessException e)
            {
                Log.Error(Tag, e, "IllegalAccessException in CustomDatePicker");
            }
            catch (Java.Lang.IllegalArgumentException e)
            {
                Log.Error(Tag, e, "IllegalArgumentException in CustomDatePicker");
            }
        }
    }
}