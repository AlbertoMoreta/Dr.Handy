
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using TFG.Droid.Fragments.ColorBlindnessTest;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid{
    public static class HealthModulesInfoExtension {

        public static IHealthFragment GetHealthCardFragmentFromHealthModuleName(string moduleName) {
            return null;
        }

        public static IHealthFragment GetHeaderFragmentFromHealthModuleName(string moduleName) { 
	    if (HealthModuleType.ColorBlindnessTest.HealthModuleName().Equals(moduleName)) { return new CBTHeaderFragment(); }
            return null;
        }

        public static IHealthFragment GetBodyFragmentFromHealthModuleName(string moduleName) {
            if (HealthModuleType.ColorBlindnessTest.HealthModuleName().Equals(moduleName)) { return new CBTBodyFragment(); }
            return null;
        } 

        public static Drawable GetHealthModuleIconFromHealthModuleType(this HealthModuleType type, Context context) { 
            try {
                var resName = type.ToString().ToLower() + "_icon";
                return ContextCompat.GetDrawable(context,
                    context.Resources.GetIdentifier(resName,
                        "drawable", context.PackageName));
            } catch (Resources.NotFoundException) {
                return null;
            }
        }

        public static Drawable GetHealthModuleBackgroundFromHealthModuleName(Context context, string moduleName) {
            try {
                var resName = "background_" + HealthModulesInfo.GetHealthModuleColorFromHealthModuleName(moduleName);
                return ContextCompat.GetDrawable(context,
                    context.Resources.GetIdentifier(resName,
                        "drawable", context.PackageName));
            } catch (Resources.NotFoundException) {
                return null;
            }
        }

        public static Drawable GetHealthModuleHeaderFromHealthModuleName(Context context, string moduleName) {
            try {
                var resName = "header_" + HealthModulesInfo.GetHealthModuleColorFromHealthModuleName(moduleName);
                return ContextCompat.GetDrawable(context,
                                                context.Resources.GetIdentifier(resName,
                                                "drawable", context.PackageName));
            } catch (Resources.NotFoundException) {
                return null;
            }
        }

        public static int GetHealthModuleThemeFromHealthModuleName(Context context, string moduleName) {
            try {
                var resName = "AppTheme_" + HealthModulesInfo.GetHealthModuleColorFromHealthModuleName(moduleName);
                return context.Resources.GetIdentifier(resName, "style", context.PackageName);
            } catch (Resources.NotFoundException) {
                return -1;
            }
        }

    }
}
