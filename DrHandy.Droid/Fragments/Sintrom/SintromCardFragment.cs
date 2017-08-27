using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using DrHandy.DataBase;
using DrHandy.Droid.Custom_Views;
using DrHandy.Droid.Interfaces;
using DrHandy.Model;
using DrHandy.Droid.Utils;

namespace DrHandy.Droid.Fragments.Sintrom {
    public class SintromCardFragment : Fragment, IHealthFragment  {

        public string ShortName { get; set; }

        public SintromCardFragment(string shortName) {
            ShortName = shortName;
        }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState); 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_card, container, false);

            var moduleName = view.FindViewById<CustomTextView>(Resource.Id.module_name);
            var controlDayText = view.FindViewById<CustomTextView>(Resource.Id.control_day);
            var sintromImage = view.FindViewById<ImageView>(Resource.Id.sintrom_image);
            var sintromDate = view.FindViewById<CustomTextView>(Resource.Id.sintrom_date);

            var module = DBHelper.Instance.GetHealthModuleByShortName(ShortName);

            moduleName.Text = module.Name;

            var userId = HealthModuleUtils.GetCurrentUserId(Activity);
            var inrItems = DBHelper.Instance.GetSintromINRItemFromDate(DateTime.Now, userId);
            var sintromItems = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now, userId);

            if (inrItems.Count > 0) {
                controlDayText.Visibility = ViewStates.Visible;
                sintromImage.Visibility = ViewStates.Gone;
                sintromDate.Visibility = ViewStates.Visible;

                sintromDate.Text = inrItems[0].Date.ToString("dd - MMM - yyyy");
            } else if (sintromItems.Count > 0) {
                controlDayText.Visibility = ViewStates.Gone;
                sintromImage.Visibility = ViewStates.Visible;
                sintromDate.Visibility = ViewStates.Visible;

                sintromImage.SetImageDrawable(ContextCompat.GetDrawable(Activity,
                    Activity.Resources.GetIdentifier(sintromItems[0].ImageName,
                        "drawable", Activity.PackageName)));

                sintromDate.Text = sintromItems[0].Date.ToString("dd - MMM - yyyy");
            } else {
                sintromImage.SetImageDrawable(module.GetIcon(Activity)); 

                controlDayText.Visibility = ViewStates.Gone;
                sintromDate.Visibility = ViewStates.Gone;
            }


            return view;
        }
    }
}