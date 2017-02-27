using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget; 
using Android.Views;
using Android.Widget;
using TFG.Droid.Custom_Views;
using TFG.Model;

namespace TFG.Droid.Adapters {
    class SintromCalendarAdapter : RecyclerView.Adapter {

        //ViewHolder for the calendar cells
        public class ViewHolder : RecyclerView.ViewHolder {
            private DateTime _date;

            public DateTime Date {
                get { return _date; }
                set {
                    _date = value;
                    DateText.Text = _date.ToString("dd/MM/yyyy");
                }
            } 
            public CustomTextView DateText { get; set; }
            public ImageView Icon { get; set; }
            public CustomTextView Fraction { get; set; }

            public ViewHolder(View itemView) : base(itemView) {
                DateText = itemView.FindViewById<CustomTextView>(Resource.Id.date);
                Icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
                Fraction = itemView.FindViewById<CustomTextView>(Resource.Id.fraction);
            }
        }

        private Context _context; 
        private DateTime _date;
        private SintromConfigureTreatmentDialog _dialog;

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

            var viewHolder = new ViewHolder(itemView);

            itemView.Click += delegate { ShowConfigurationDialog(viewHolder.Date); };

            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            ViewHolder viewHolder = holder as ViewHolder;
            var date = new DateTime(_date.Year, _date.Month, 1).AddDays(position - _firstDayMonth);  
            var items = DBHelper.Instance.GetSintromItemFromDate(date);

            viewHolder.Date = date;

            if (date.Month == _date.Month) {
                viewHolder.ItemView.Clickable = true;
                if (items.Count != 0) {
                    viewHolder.Icon.SetImageDrawable(items[0].ImageName.Equals("")
                        ? null
                        : ContextCompat.GetDrawable(_context,
                            _context.Resources.GetIdentifier(items[0].ImageName,
                                "drawable", _context.PackageName)));

                    viewHolder.Fraction.Text = items[0].Fraction;
                } else {
                    viewHolder.Icon.SetImageDrawable(null);
                    viewHolder.Fraction.Text = null;
                }

                if (date.Date.Equals(Date.Date)) {
                    viewHolder.ItemView.Background = ContextCompat.GetDrawable(_context,
                        Resource.Drawable.background_selector);
                } else {
                    viewHolder.ItemView.SetBackgroundColor(Color.White);
                } 
            } else {
                viewHolder.ItemView.Clickable = false;
                viewHolder.ItemView.SetBackgroundColor(Color.LightGray);
                viewHolder.Icon.SetImageDrawable(null);
                viewHolder.Fraction.Text = null;
            }
        }

        private void ShowConfigurationDialog(DateTime date) {

            _dialog = new SintromConfigureTreatmentDialog(_context);
            _dialog.SelectedDate = date;  

            var items = DBHelper.Instance.GetSintromItemFromDate(date);
            var currentItem = items.Count > 0 ? items[0] : null;  

            if(currentItem != null) {
                //Set initial quantity value if exists
                var sintromArray = _context.Resources.GetStringArray(Resource.Array.sintrom_array);
                _dialog.SelectedMedicine = currentItem.Medicine;
                _dialog.Medicine.SetSelection(sintromArray.ToList().IndexOf(currentItem.Medicine.Split(new char[] {' '}, 2)[1]));
            }
            

            foreach (var image in _dialog.PillImages) {
                //Set initial value if exists
                if (currentItem != null)  {
                    if (image.Tag.Equals(currentItem.ImageName)) {
                        _dialog.SelectedImageName = currentItem.ImageName;
                        image.Background = ContextCompat.GetDrawable(_context, Resource.Drawable.background_selector);
                    }
                } 
            }
 
            _dialog.AcceptButton.Click += SaveInfo; 

            _dialog.Dialog.Show();
        }  

        //Save Sintrom configuration
        private void SaveInfo(object sender, EventArgs e) {
            DBHelper.Instance.InsertSintromItem(new SintromTreatmentItem(_dialog.SelectedDate, _dialog.SelectedImageName, _dialog.SelectedMedicine, _dialog.Control.Checked));
            
            NotifyDataSetChanged();

            _dialog.Dialog.Hide();

            Toast.MakeText(_context, _context.GetString(Resource.String.sintrom_updateinfo_success), ToastLength.Short).Show();
        }

    }
}