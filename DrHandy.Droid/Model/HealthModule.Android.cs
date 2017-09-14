using System;
using Android.Content;
using Android.Graphics.Drawables;
using DrHandy.Droid.Interfaces;
using DrHandy.Droid.Utils;
using DrHandy.Model;

namespace DrHandy.Model {
    /*
     * HealthModule.Android - Partial class of shared HealthModule with Android specific methods
     */
    public partial class HealthModule { 

        public HealthModuleUtils GetUtilsClass() {
            var utilsClass = "DrHandy.Droid.Utils." + UtilsClass;
            Type t = Type.GetType(utilsClass);
            if (t == null) {
                throw new Exception("Class " + utilsClass + " not found.");
            }
            return (HealthModuleUtils)Activator.CreateInstance(t); 
        }

        public Drawable GetIcon (Context context, string shortName) {   
            return GetUtilsClass().GetHealthModuleIcon(context, shortName);  
        }

        public Drawable GetBackground(Context context) { 
            return GetUtilsClass().GetHealthModuleBackground(context, Color);
        }

        public Drawable GetHeader(Context context) { 
            return GetUtilsClass().GetHealthModuleHeader(context, Color);
        }

        public int GetTheme(Context context)  {
            return GetUtilsClass().GetHealthModuleTheme(context, Color);
        }

        public IHealthFragment GetHeaderFragment() {
            return GetUtilsClass().GetHeaderFragment();
        }
        public IHealthFragment GetBodyFragment() {
            return GetUtilsClass().GetBodyFragment();
        }
        public IHealthFragment GetHealthCardFragment() {
            return GetUtilsClass().GetHealthCardFragment(ShortName);
        }
        public NotificationItem GetNotificationItem(Context context, HealthModule healthModule) {
            return GetUtilsClass().GetNotificationItem(context, healthModule);
        }
    }
}
