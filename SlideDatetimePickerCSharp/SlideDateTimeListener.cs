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

namespace SlideDatetimePickerCSharp
{
    public abstract class SlideDateTimeListener
    {
        public abstract void OnDateTimeSet(DateTime date);
        public virtual void OnDateTimeCancel() { }
    }
}