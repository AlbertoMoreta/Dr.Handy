using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using TFG.DataBase;
using TFG.Droid.Fragments.Sintrom;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Utils {
    class SintromUtils : HealthModuleUtils{

        public static void INRTextChanged(object s, TextChangedEventArgs e) {
            var editText = (EditText) s;
            if (e.Text.Count() == 2 && !e.Text.ElementAt(0).Equals('.') && !e.Text.ElementAt(1).Equals('.')) {
                editText.Text = e.Text.ElementAt(0) + "." + e.Text.ElementAt(1);
                editText.SetSelection(editText.Text.Length);
            } else if (e.Text.Count() == 1 && e.Text.ElementAt(0).Equals('.')) {
                editText.Text = "";
            }
        }

        public override void InitModuleDB() {
            var db = DBHelper.Instance;
            db.CreateSintromTable();
            db.CreateINRTable();
        }

        public override Drawable GetHealthModuleIcon(Context context) {
            return GetDrawableFromResources(context, "sintrom_icon");
        }

        public override IHealthFragment GetHeaderFragment() {
            return new SintromHeaderFragment();
        }

        public override IHealthFragment GetBodyFragment() {
            return new SintromBodyFragment();
        }

        public override IHealthFragment GetHealthCardFragment(string shortName) {
            return new SintromCardFragment(shortName);
        }
    }
}