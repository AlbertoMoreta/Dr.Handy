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
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid.Fragments.Sintrom {
    /// <summary>
    /// Header fragment for the Sintrom health module
    /// </summary>
    public class SintromHeaderFragment : Fragment, IHealthFragment { 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_header, container, false);

            var medicine = view.FindViewById<CustomTextView>(Resource.Id.medicine);
            var icon = view.FindViewById<ImageView>(Resource.Id.icon);
            var fraction = view.FindViewById<CustomTextView>(Resource.Id.fraction);  

            //Get Sintrom treatment for today
            var items =
                    DBHelper.Instance.GetSintromItemFromDate(DateTime.Now); 

            if (items.Count > 0) {
                //Toolbar title with today's date 
                (Activity as BaseActivity).ToolbarTitle.Text = DateTime.Now.ToString("dd / MM / yyyy");

                var item = items.ElementAt(0); 
                medicine.Text = item.Medicine; 
                icon.SetImageDrawable(ContextCompat.GetDrawable(Activity,
                    Activity.Resources.GetIdentifier(item.ImageName,
                        "drawable", Activity.PackageName))); 
                fraction.Text = item.Fraction;

            } else  {
                //If there is no treatment, set the toolbar title as the name of the health module
                (Activity as BaseActivity).ToolbarTitle.Text = HealthModuleType.Sintrom.HealthModuleName();
            }

            return view;
        }
    }
}