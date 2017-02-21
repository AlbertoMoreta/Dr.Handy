using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using TFG.Droid.Custom_Views;
using Java.Util;
using TFG.Droid.Listeners;
using TFG.Model;

namespace TFG.Droid.Adapters {
    class SintromTreatmentListAdapter : RecyclerView.Adapter {

        //ViewHolder for the treatment days
        public class ViewHolder : RecyclerView.ViewHolder { 
            
            public CustomTextView Date { get; set; }
            public ImageView Icon { get; set; }
            public CustomTextView Fraction { get; set; }

            public ViewHolder(View itemView) : base(itemView) {
                Date = itemView.FindViewById<CustomTextView>(Resource.Id.date);
                Icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
                Fraction = itemView.FindViewById<CustomTextView>(Resource.Id.fraction);
            }
        }

        private Context _context;
        private List<SintromTreatmentItem> _items = new List<SintromTreatmentItem>(); 


        public SintromTreatmentListAdapter(Context context, List<SintromTreatmentItem> items) {
            _context = context;
            _items = items;
        }

        public override int ItemCount {
            get {
                return _items.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
            var itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.sintrom_treatment_row, parent, false);
            return new ViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            var viewHolder = holder as ViewHolder;
            var item = _items.ElementAt(position);
            viewHolder.Date.Text = item.Date.ToString();
            viewHolder.Icon.SetImageDrawable(ContextCompat.GetDrawable(_context,
                                                _context.Resources.GetIdentifier(item.ImageName,
                                                "drawable", _context.PackageName))); 
        }  

    }
}
