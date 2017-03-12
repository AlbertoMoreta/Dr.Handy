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
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils;
using TFG.Model;

namespace TFG.Droid.Fragments.Sintrom {
    /// <summary>
    /// Header fragment for the Sintrom health module
    /// </summary>
    public class SintromHeaderFragment : Fragment, IHealthFragment
    {

        private CustomTextView _medicine;
        private ImageView _icon;
        private CustomTextView _fraction;
        private LayoutInflater _inflater;
        private ViewGroup _container;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
             
            //Set alarm at 12 pm
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var time = DateTime.UtcNow.Date.ToLocalTime()
                    .AddHours(12)
                    .AddMinutes(0)
                    .AddSeconds(0);
            var offset = System.TimeZone.CurrentTimeZone.GetUtcOffset(time);
            var diff = time.ToUniversalTime() - origin - offset; 
            NotificationsUtils.ScheduleNotification(Activity, HealthModuleType.Sintrom.HealthModuleId(), (long) diff.TotalMilliseconds);
        }
 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_header, container, false);
            _inflater = inflater;
            _container = container;

            _medicine = view.FindViewById<CustomTextView>(Resource.Id.medicine);
            _icon = view.FindViewById<ImageView>(Resource.Id.icon);
            _fraction = view.FindViewById<CustomTextView>(Resource.Id.fraction);  

           
            return view;
        }

        public override void OnStart() {
            base.OnStart();
            RefreshHeader();
        }

        private void RefreshHeader() {
             //Get Sintrom treatment for today
            var items =
                    DBHelper.Instance.GetSintromItemFromDate(DateTime.Now);

            var inrItems = DBHelper.Instance.GetSintromINRItemFromDate(DateTime.Now);

            if (items.Count > 0) {
                //Toolbar title with today's date 
                (Activity as BaseActivity).ToolbarTitle.Text = DateTime.Now.ToString("dd / MM / yyyy");
                View.Visibility = ViewStates.Visible;

                var item = items.ElementAt(0);
                if (inrItems.Count > 0 && inrItems[0].Control) {
                    var layout = (ViewGroup) View;
                    layout.RemoveAllViews();
                    var controlDayView = _inflater.Inflate(Resource.Layout.fragment_sintrom_header_control, _container, false);
                    layout.AddView(controlDayView);
                    var inputINR = controlDayView.FindViewById<EditText>(Resource.Id.input_inr);
                    inputINR.Text = inrItems[0].INR.ToString();
                    inputINR.TextChanged += SintromUtils.INRTextChanged;
                    inputINR.AfterTextChanged += (s, e) => {
                        DBHelper.Instance.InsertSintromINRItem(new SintromINRItem(DateTime.Now, true,
                            ((EditText) s).Text.Equals("") ? 0 : double.Parse(((EditText) s).Text)));
                    };

                }else { 
                    _medicine.Text = item.Medicine;
                    _icon.SetImageDrawable(item.ImageName.Equals("")
                            ? null
                            : ContextCompat.GetDrawable(Activity,
                                Activity.Resources.GetIdentifier(item.ImageName,
                                    "drawable", Activity.PackageName)));
                    _fraction.Text = item.Fraction;
                }
            } else  {
                //If there is no treatment, set the toolbar title as the name of the health module
                (Activity as BaseActivity).ToolbarTitle.Text = HealthModuleType.Sintrom.HealthModuleName();
                View.Visibility = ViewStates.Gone;
            }

        }

    }
}