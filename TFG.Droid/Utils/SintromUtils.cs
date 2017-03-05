using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace TFG.Droid.Utils {
    class SintromUtils {

        public static void INRTextChanged(object s, TextChangedEventArgs e) {
            var editText = (EditText) s;
            if (e.Text.Count() == 2 && !e.Text.ElementAt(0).Equals('.') && !e.Text.ElementAt(1).Equals('.')) {
                editText.Text = e.Text.ElementAt(0) + "." + e.Text.ElementAt(1);
                editText.SetSelection(editText.Text.Length);
            } else if (e.Text.Count() == 1 && e.Text.ElementAt(0).Equals('.')) {
                editText.Text = "";
            }
        }
    }
}