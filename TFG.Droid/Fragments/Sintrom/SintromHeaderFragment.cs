using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using TFG.DataBase;
using TFG.Droid.Activities;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils;
using TFG.Model;

namespace TFG.Droid.Fragments.Sintrom {
    /// <summary>
    /// Header fragment for the Sintrom health module
    /// </summary>
    public class SintromHeaderFragment : Fragment, IHealthFragment { 

        private LayoutInflater _inflater;
        private ViewGroup _container;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
             
            //Set alarm at 12 pm
            int dayOffset = DateTime.UtcNow.ToLocalTime().Hour < 12 ? 0 : 1;  
            var calendar = Calendar.Instance;

            calendar.Set(CalendarField.Date, calendar.Get(CalendarField.Date) + dayOffset);
            calendar.Set(CalendarField.HourOfDay, 12);
            calendar.Set(CalendarField.Minute, 0);
            calendar.Set(CalendarField.Second, 0);

            var moduleShortName = ((ModuleDetailActivity)Activity).CurrentHealthModule.ShortName;
            NotificationsUtils.ScheduleNotification(Activity, moduleShortName, calendar.TimeInMillis);
        }
 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_header, container, false);

            _inflater = inflater;
            _container = container; 
           
            return view;
        }

        public override void OnResume() {
            base.OnResume();
             
            //Toolbar title with today's date 
            (Activity as BaseActivity).ToolbarTitle.Text = DateTime.Now.ToString("D");
            RefreshHeader();
        }
         

        private void RefreshHeader() {
            var userId = HealthModuleUtils.GetCurrentUserId(Context);

            //Get Sintrom treatment for today
            var items = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now, userId);

            var inrItems = DBHelper.Instance.GetSintromINRItemFromDate(DateTime.Now, userId);
    

            if(inrItems.Count > 0 && inrItems[0].Control) {
                View.Visibility = ViewStates.Visible;
                RefreshLayout(true);

                var inputINR = View.FindViewById<EditText>(Resource.Id.input_inr);
                inputINR.Text = inrItems[0].INR.ToString();
                inputINR.TextChanged += SintromUtils.INRTextChanged;
                inputINR.AfterTextChanged += (s, e) => {
                    DBHelper.Instance.InsertSintromINRItem(new SintromINRItem(userId, DateTime.Now, true,
                        ((EditText)s).Text.Equals("") ? 0 : double.Parse(((EditText)s).Text)));
                }; 
            }else if (items.Count > 0) {

                View.Visibility = ViewStates.Visible;
                RefreshLayout(false);

                var medicine = View.FindViewById<CustomTextView>(Resource.Id.medicine);
                var icon = View.FindViewById<ImageView>(Resource.Id.icon);
                var fraction = View.FindViewById<CustomTextView>(Resource.Id.fraction);

                var item = items.ElementAt(0);
                medicine.Text = item.Medicine;
                icon.SetImageDrawable(item.ImageName.Equals("")
                        ? null
                        : ContextCompat.GetDrawable(Activity,
                            Activity.Resources.GetIdentifier(item.ImageName,
                                "drawable", Activity.PackageName)));
                fraction.Text = item.Fraction;

            } else  {
                //If there is no treatment, set the toolbar title as the name of the health module
                (Activity as BaseActivity).ToolbarTitle.Text = ((ModuleDetailActivity) Activity).CurrentHealthModule.Name;
                View.Visibility = ViewStates.Gone;
            }

        }

        private void RefreshLayout(bool control) {
            var layout = (ViewGroup)View;
            layout.RemoveAllViews();
            var resource = control ? Resource.Layout.fragment_sintrom_header_control : Resource.Layout.fragment_sintrom_header;
            var newLayout = _inflater.Inflate(resource, _container, false);
            layout.AddView(newLayout); 
        }

    }
}