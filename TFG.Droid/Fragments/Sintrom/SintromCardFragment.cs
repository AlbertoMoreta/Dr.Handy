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
using TFG.DataBase;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromCardFragment : Fragment, IHealthFragment  {
        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState); 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_card, container, false);

            var moduleName = view.FindViewById<CustomTextView>(Resource.Id.module_name);
            var sintromImage = view.FindViewById<ImageView>(Resource.Id.sintrom_image);
            var sintromDate = view.FindViewById<CustomTextView>(Resource.Id.sintrom_date);

            moduleName.Text = HealthModuleType.Sintrom.HealthModuleName();

            var sintromItems = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now);

            if (sintromItems.Count > 0) {
                sintromImage.SetImageDrawable(ContextCompat.GetDrawable(Activity,
                                Activity.Resources.GetIdentifier(sintromItems[0].ImageName,
                                    "drawable", Activity.PackageName)));

                sintromDate.Text = sintromItems[0].Date.ToString("dd - MMM - yyyy");
            }


            return view;
        }
    }
}