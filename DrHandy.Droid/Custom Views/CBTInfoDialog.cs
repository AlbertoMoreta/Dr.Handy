using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Content;

namespace DrHandy.Droid.Custom_Views {
    class CBTInfoDialog : AlertDialog {

        private static Context _context;
        public CustomTextView InfoText { get; set; }
        public ImageView ImageLeft { get; set; }
        public ImageView ImageRight { get; set; }  

        //Dialog showing instruction for Color Blindness test
        //Type: 1 - Info dialog for number questions
        //      2 - Info dialog for line questions
        public CBTInfoDialog(Context context, int type = 1) : base(context) {
            _context = context;
            Init(type);
        }
        private void Init(int type) {

            var builder = new Builder(_context);
            var v = ((Activity)_context).LayoutInflater.Inflate(Resource.Layout.cbt_info_dialog, null);
            InfoText = v.FindViewById<CustomTextView>(Resource.Id.info_text);
            ImageLeft = v.FindViewById<ImageView>(Resource.Id.image_left);
            ImageRight = v.FindViewById<ImageView>(Resource.Id.image_right);


            switch (type) {
                case 1:
                    InfoText.Text = _context.Resources.GetString(Resource.String.number_question_info);
                    ImageLeft.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.ishihara_08));
                    ImageRight.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.cbt_numbers_example));
                    break;
                case 2:
                    InfoText.Text = _context.Resources.GetString(Resource.String.line_question_info);
                    ImageLeft.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.ishihara_35));
                    ImageRight.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.cbt_lines_example));
                    break;
            }

            var dialog = builder.SetView(v).Create();
            dialog.Show();

            v.FindViewById<Button>(Resource.Id.accept_button).Click += delegate { dialog.Hide(); };

            
        }
    }
}