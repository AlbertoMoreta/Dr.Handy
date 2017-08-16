using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using TFG.Droid.Custom_Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Uri = Android.Net.Uri; 
using Android.Preferences;
using Android.Graphics;
using System.Net;
using Refractored.Controls;
using TFG.Droid.Utils;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using TFG.Droid.Activities;

namespace TFG.Droid {
    [Activity(Label = "BaseActivity", Theme ="@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
    public class BaseActivity : AppCompatActivity{

        public Toolbar ToolBar { get; set; }
        public CustomTextView ToolbarTitle { get; set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            var theme = Resources.GetIdentifier("AppTheme_red", "style", PackageName);
            if (theme != -1) { SetTheme(theme); }
        }


        protected void SetUpToolBar(bool isTransparent = true, bool showBackButton = false, bool showLoginInfo = false) { 
            var toolBar = ToolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
            
            if (toolBar != null) {  
                SetSupportActionBar(toolBar);
                SupportActionBar.SetDisplayShowTitleEnabled(false);
                ToolbarTitle = toolBar.FindViewById<CustomTextView>(Resource.Id.title);
                ToolbarTitle.Text = Title;
                if (isTransparent) { toolBar.Background = null; }
                if (showBackButton) {
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    var lp = (RelativeLayout.LayoutParams)ToolbarTitle.LayoutParameters;
                    var typedArray = ObtainStyledAttributes(new int[] { Resource.Attribute.actionBarSize });
                    lp.RightMargin = typedArray.GetDimensionPixelSize(0, 0);
                    typedArray.Recycle();
                    ToolbarTitle.LayoutParameters = lp;
                }
                if (showLoginInfo) {
                    var iv = FindViewById<CircleImageView>(Resource.Id.user_image);
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    var userImage = prefs.GetString("UserImage", null);
                    iv.Click += OnAccountImageClicked;
                    iv.SetImageBitmap(GetImageBitmapFromUrl(userImage));
                    iv.Invalidate();
                }

            }
        }

        public override bool OnSupportNavigateUp() {
            OnBackPressed();
            return true;
        } 

        private Bitmap GetImageBitmapFromUrl(string url) {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient()) {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0) {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private void OnAccountImageClicked(object sender, EventArgs e){
            var popup = new PopupMenu(this, ((CircleImageView) sender));  
            popup.MenuInflater.Inflate(Resource.Menu.sign_out_popup_menu, popup.Menu);

            //Sign Out user
            popup.MenuItemClick += delegate {

                SignInUtils.Instance.SignOut();
                Finish();
            };

            popup.Show();

        }
    }
}