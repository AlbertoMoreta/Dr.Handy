 
 
using Android.OS; 
using Android.Views; 
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromCalendarFragment : Fragment { 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.sintrom_calendar, container, false); 

            return view;
        }
    }
}