using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid.Utils {
    public abstract class HealthModuleUtils {

        public abstract Drawable GetHealthModuleIcon(Context context);
        public abstract Drawable GetHealthModuleBackground(Context context);
        public abstract Drawable GetHealthModuleHeader(Context context);

        public abstract IHealthFragment GetHeaderFragment();
        public abstract IHealthFragment GetBodyFragment();
        public abstract IHealthFragment GetHealthCardFragment();


        public int GetHealthModuleTheme(Context context, string color) {
            var resName = "AppTheme_" + color;
            return GetStyleFromResources(context, resName);
        }


        public static Drawable GetDrawableFromResources(Context context, string resName) {
            try{
                return ContextCompat.GetDrawable(context,
                                                   context.Resources.GetIdentifier(resName,
                                                   "drawable", context.PackageName));
            } catch (Resources.NotFoundException) {
                return null;
            }

        }

        public static int GetStyleFromResources(Context context, string resName) {
            try { 
                return context.Resources.GetIdentifier(resName, "style", context.PackageName);
            } catch (Resources.NotFoundException) {
                return -1;
            }
        }



    }
}