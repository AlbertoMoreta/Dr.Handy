using System;
using Android.Content;
using Android.Graphics.Drawables;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils;
using TFG.Model;

namespace TFG.Model {
    public partial class HealthModule {
        

        public HealthModuleUtils GetUtilsClass() {
            var utilsClass = "TFG.Droid.Utils." + UtilsClass;
            Type t = Type.GetType(utilsClass);
            if (t == null) {
                throw new Exception("Class " + utilsClass + " not found.");
            }
            return (HealthModuleUtils)Activator.CreateInstance(t); 
        }

        public Drawable GetIcon (Context context) {   
            return GetUtilsClass().GetHealthModuleIcon(context);  
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
            return GetUtilsClass().GetHealthCardFragment(Name);
        }
    }
}
