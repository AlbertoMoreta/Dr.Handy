﻿using System;
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
using DrHandy.Droid.Interfaces;
using DrHandy.Model;
using Android.Preferences;

namespace DrHandy.Droid.Utils {
    
    /*
     * HealthModuleUtils - Utils class to retrieve information for an specific health module 
     */ 
    public abstract class HealthModuleUtils { 
        public abstract void InitModuleDB();

        public abstract void DeviceBootUp(Context context, string shortName); 

        public abstract IHealthFragment GetHeaderFragment();
        public abstract IHealthFragment GetBodyFragment();
        public abstract IHealthFragment GetHealthCardFragment(string shortName);
        public abstract NotificationItem GetNotificationItem(Context context, HealthModule healthModule);

        public static string GetCurrentUserId(Context context) {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            return prefs.GetString("Id", null);
        } 

        public Drawable GetHealthModuleIcon(Context context, string shortName) {
            return GetDrawableFromResources(context, shortName + "_icon");  
        }

        public Drawable GetHealthModuleBackground(Context context, string color) {
            return GetDrawableFromResources(context, "background_" + color);
        } 

        public Drawable GetHealthModuleHeader(Context context, string color) { 
            return GetDrawableFromResources(context, "header_" + color);
        }

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
