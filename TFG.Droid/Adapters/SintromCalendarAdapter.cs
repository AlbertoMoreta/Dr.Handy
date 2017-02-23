using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TFG.Droid.Custom_Views;
using TFG.Droid.Listeners;

namespace TFG.Droid.Adapters {
    class SintromCalendarAdapter : RecyclerView.Adapter {

        //ViewHolder for the calendar cells
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
        private DateTime _date;
        private int _offset;
        private int _totalDays;

        public SintromCalendarAdapter(Context context, DateTime date) {
            _context = context;
            _date = date;
            _offset = - (int) date.DayOfWeek;
            var daysInMonth = DateTime.DaysInMonth(_date.Year, _date.Month);
            _totalDays = daysInMonth + date.DayOfWeek + 6 - new DateTime(_date.Year, _date.Month, daysInMonth).DayOfWeek;
        }

        public override int ItemCount {
            get { return _totalDays; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
            var itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.sintrom_calendar_item, parent, false);  
            return new ViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            ViewHolder viewHolder = holder as ViewHolder;
            var date = new DateTime(_date.Year, _date.Month, 1).AddDays(_offset); 
            _offset++;
            var item = DBHelper.Instance.GetSintromItemFromDate(date);
            viewHolder.Date.Text = date.ToString("dd/MM/yyyy");

            if (item != null) {
                viewHolder.Icon.SetImageDrawable(ContextCompat.GetDrawable(_context,
                                                _context.Resources.GetIdentifier(item[0].ImageName,
                                                "drawable", _context.PackageName))); 
                viewHolder.Fraction.Text = item[0].Fraction;
            }

            viewHolder.ItemView.Click += delegate { };
        }
         
       

    }
}