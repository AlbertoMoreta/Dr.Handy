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
using DrHandy.DataBase;
using DrHandy.Droid.Fragments.Sintrom;
using DrHandy.Droid.Interfaces;
using DrHandy.Model;
using Java.Util;

namespace DrHandy.Droid.Utils {
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

        public override void DeviceBootUp(Context context, string shortName) {
            ScheduleNotification(context, shortName);
        }

        public override void InitModuleDB() {
            var db = DBHelper.Instance;
            db.CreateSintromTable();
            db.CreateINRTable();
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

        public override NotificationItem GetNotificationItem(Context context, HealthModule healthModule) {
            NotificationItem notificationItem = null;

            var controlday = DBHelper.Instance.GetSintromINRItemFromDate(DateTime.Now, GetCurrentUserId(context));
            var title = healthModule.Name;
            //Control Day Notification
            if (controlday.Count > 0 && controlday[0].Control) { 
                var description = context.GetString(context.Resources.GetIdentifier("sintrom_notification_control_description",
                "string", context.PackageName));

                notificationItem = new NotificationItem(title, description, true);
            } else {

                //Treatment Day Notification
                var sintromItems = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now, GetCurrentUserId(context));
                if (sintromItems.Count > 0) {
                    var sintromItem = sintromItems[0];
                    var description = string.Format(context.GetString(context.Resources.GetIdentifier("sintrom_notification_description",
                    "string", context.PackageName)), sintromItem.Fraction, sintromItem.Medicine);

                    notificationItem = new NotificationItem(title, description, true);
                }
            }
            //No Notification
            return notificationItem;
        } 


        public static void ScheduleNotification(Context context, string shortName) {
            //Set alarm at 12 pm
            int dayOffset = DateTime.UtcNow.ToLocalTime().Hour < 12 ? 0 : 1;
            var calendar = Java.Util.Calendar.Instance;

            calendar.Set(CalendarField.Date, calendar.Get(CalendarField.Date) + dayOffset);
            calendar.Set(CalendarField.HourOfDay, 12);
            calendar.Set(CalendarField.Minute, 0);
            calendar.Set(CalendarField.Second, 0);
             
            NotificationsUtils.ScheduleNotification(context, shortName, calendar.TimeInMillis);
        }


    }
}