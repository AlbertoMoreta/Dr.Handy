using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using TFG.Droid.Custom_Views;
using TFG.Droid.Listeners;
using TFG.Model;

namespace TFG.Droid.Adapters {
    class SintromCalendarAdapter : RecyclerView.Adapter {

        //ViewHolder for the calendar cells
        public class ViewHolder : RecyclerView.ViewHolder { 
            public View ItemView { get; private set; }
            public CustomTextView Date { get; set; }
            public ImageView Icon { get; set; }
            public CustomTextView Fraction { get; set; }

            public ViewHolder(View itemView) : base(itemView) {
                ItemView = itemView;
                Date = itemView.FindViewById<CustomTextView>(Resource.Id.date);
                Icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
                Fraction = itemView.FindViewById<CustomTextView>(Resource.Id.fraction);
            }
        }

        private Context _context; 
        private DateTime _date; 

        public DateTime Date {
            get { return _date; }
            set { _date = value; UpdateCalendarInfo(); }
        }
        private int _firstDayMonth;
        private int _totalDays;

        public SintromCalendarAdapter(Context context, DateTime date) {
            _context = context;
            Date = date; 
        }

        public override int ItemCount {
            get { return _totalDays; }
        }

        private void UpdateCalendarInfo() {
            var daysInMonth = DateTime.DaysInMonth(_date.Year, _date.Month);
            _firstDayMonth = (int) new DateTime(_date.Year, _date.Month, 1).DayOfWeek; 
            var lastDayMonth = new DateTime(_date.Year, _date.Month, daysInMonth).DayOfWeek;
            //Get total days to show counting the last days of the previous month and the first days of the next month 
            _totalDays = daysInMonth + _firstDayMonth + 6 - (int) lastDayMonth;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
            var itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.sintrom_calendar_item, parent, false);  
            return new ViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            ViewHolder viewHolder = holder as ViewHolder;
            var date = new DateTime(_date.Year, _date.Month, 1).AddDays(position - _firstDayMonth);  
            var items = DBHelper.Instance.GetSintromItemFromDate(date);

            viewHolder.Date.Text = date.ToString("dd/MM/yyyy");

            if (date.Month == _date.Month) {
                if (items.Count != 0) {
                    viewHolder.Icon.SetImageDrawable(ContextCompat.GetDrawable(_context,
                        _context.Resources.GetIdentifier(items[0].ImageName,
                            "drawable", _context.PackageName)));
                    viewHolder.Fraction.Text = items[0].Fraction;
                }
                
                viewHolder.ItemView.SetBackgroundColor(Color.White);
                viewHolder.ItemView.Click += delegate { CreateConfigurationDialog(date); };
            } else {
                viewHolder.ItemView.SetBackgroundColor(Color.LightGray);
                viewHolder.Icon.SetImageDrawable(null);
                viewHolder.Fraction.Text = null;
            }
        }

        private void CreateConfigurationDialog(DateTime date) {
            var builder = new AlertDialog.Builder(_context);

            var v = ((Activity) _context).LayoutInflater.Inflate(Resource.Layout.sintrom_configuration_dialog, null);

            var currentDate = v.FindViewById<CustomTextView>(Resource.Id.current_date);
            currentDate.Text = date.ToString("dd MMMM yyyy");

            var control = v.FindViewById<SwitchCompat>(Resource.Id.control);

            var sintrom1 = v.FindViewById<ImageView>(Resource.Id.sintrom1);
            sintrom1.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1));
            sintrom1.Click += PillClicked;
            var sintrom3_4 = v.FindViewById<ImageView>(Resource.Id.sintrom3_4);
            sintrom3_4.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_3_4));
            var sintrom1_2 = v.FindViewById<ImageView>(Resource.Id.sintrom1_2);
            sintrom1_2.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1_2));
            var sintrom1_4 = v.FindViewById<ImageView>(Resource.Id.sintrom1_4);
            sintrom1_4.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1_4));
            var sintrom1_8 = v.FindViewById<ImageView>(Resource.Id.sintrom1_8);
            sintrom1_8.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1_8)); 
            
            builder.SetView(v).Create().Show(); 
        } 

        private void PillClicked(object sender, EventArgs e) {
        }
         
       

    }
}