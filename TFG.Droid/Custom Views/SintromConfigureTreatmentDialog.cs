using System;
using System.Collections.Generic;
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
using TFG.Droid.Utils;

namespace TFG.Droid.Custom_Views {
    public class SintromConfigureTreatmentDialog : AlertDialog {

        private static Context _context; 
        public List<ImageView> PillImages { get; set; } 
        public AlertDialog Dialog { get; set; }
        public CustomTextView CurrentDate { get; set; }
        public SwitchCompat Control { get; set; }
        public EditText INR { get; set; }
        public Spinner Medicine { get; set; }
        public Button AcceptButton { get; set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate {
            get { return _selectedDate; }
            set {
                _selectedDate = value;
                //Set Dialog title
                CurrentDate.Text = value.ToString("D");
            }
        }
        public string SelectedImageName { get; set; }
        public string SelectedMedicine { get; set; }

        
        public SintromConfigureTreatmentDialog(Context context) : base(context) {
            _context = context;
            Init();
        } 

        private void Init() { 

            var builder = new Builder(_context); 
            var v = ((Activity) _context).LayoutInflater.Inflate(Resource.Layout.sintrom_configuration_dialog, null);

            CurrentDate = v.FindViewById<CustomTextView>(Resource.Id.current_date);

            var noControlLayout = v.FindViewById<LinearLayout>(Resource.Id.no_control_layout);
            var inputINR = v.FindViewById<RelativeLayout>(Resource.Id.input_inr);
            INR = v.FindViewById<EditText>(Resource.Id.inr);
            INR.TextChanged += SintromUtils.INRTextChanged;
            Control = v.FindViewById<SwitchCompat>(Resource.Id.control);
            Control.CheckedChange += (s, e) => {
                noControlLayout.Visibility = e.IsChecked ? ViewStates.Gone : ViewStates.Visible;
                inputINR.Visibility = e.IsChecked ? ViewStates.Visible : ViewStates.Gone;
            };
            Medicine = v.FindViewById<Spinner>(Resource.Id.medicine);
            var sintromArray = _context.Resources.GetStringArray(Resource.Array.sintrom_array);
            Medicine.Adapter = new ArrayAdapter<string>(_context,
                Android.Resource.Layout.SimpleSpinnerDropDownItem, sintromArray);
            //On Spinner item change
            Medicine.ItemSelected += (s, e) => {
                SelectedMedicine =
                    _context.GetString(Resource.String.sintrom_name) + " " + ((TextView) e.View).Text;
            }; 


            //Initialize sintrom pill images
            PillImages = new List<ImageView>(); 
            var sintrom1 = v.FindViewById<ImageView>(Resource.Id.sintrom1);
            sintrom1.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1));
            sintrom1.Tag = "sintrom_1";
            sintrom1.Click += PillClicked;
            PillImages.Add(sintrom1); 
            var sintrom3_4 = v.FindViewById<ImageView>(Resource.Id.sintrom3_4);
            sintrom3_4.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_3_4));
            sintrom3_4.Tag = "sintrom_3_4";
            sintrom3_4.Click += PillClicked;
            PillImages.Add(sintrom3_4);
            var sintrom1_2 = v.FindViewById<ImageView>(Resource.Id.sintrom1_2);
            sintrom1_2.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1_2));
            sintrom1_2.Tag = "sintrom_1_2";
            sintrom1_2.Click += PillClicked;
            PillImages.Add(sintrom1_2);
            var sintrom1_4 = v.FindViewById<ImageView>(Resource.Id.sintrom1_4);
            sintrom1_4.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1_4));
            sintrom1_4.Tag = "sintrom_1_4";
            sintrom1_4.Click += PillClicked;
            PillImages.Add(sintrom1_4);
            var sintrom1_8 = v.FindViewById<ImageView>(Resource.Id.sintrom1_8);
            sintrom1_8.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.sintrom_1_8));
            sintrom1_8.Tag = "sintrom_1_8";
            sintrom1_8.Click += PillClicked;
            PillImages.Add(sintrom1_8);

            AcceptButton = v.FindViewById<Button>(Resource.Id.accept_button);

            Dialog = builder.SetView(v).Create(); 

        }

        //Set image name of selected pill and change background
        private void PillClicked(object sender, EventArgs e) {
            var clickedImage = ((ImageView) sender);
            var clickedImageBackground = clickedImage.Background;

            foreach (var image in PillImages) {
                if (clickedImageBackground == null) {
                    SelectedImageName = clickedImage.Tag.ToString();
                    image.Background = image.Equals(clickedImage)
                        ? ContextCompat.GetDrawable(_context, Resource.Drawable.background_selector)
                        : null;
                } else  {
                    SelectedImageName = "";
                    image.Background = null;
                } 
            }
        }

    }
}