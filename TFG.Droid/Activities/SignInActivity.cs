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
using Android.Content.PM;
using TFG.DataBase;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Support.V4.App; 
using static Android.Gms.Common.Apis.GoogleApiClient;
using TFG.Model;
using TFG.Droid.Custom_Views;
using Android.Preferences;
using TFG.Droid.Utils;

namespace TFG.Droid.Activities { 
    [Activity(Label = "SignInActivity", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]

    public class SignInActivity : BaseActivity{

         
        private HealthModule _healthModule;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.sign_in);
            SetUpToolBar();

            //Initialize GoogleApiClient
            SignInUtils.Instance.InitClient(this); 
             
            var shortName = Intent.GetStringExtra("ShortName");
            _healthModule = DBHelper.Instance.GetHealthModuleByShortName(shortName); 

            //Toolbar title
            ToolbarTitle.Text = Resources.GetString(Resource.String.access, _healthModule.Name);

            //Set background image
            Window.DecorView.Background = _healthModule.GetBackground(this);

            //Set module icon
            var icon = FindViewById<ImageView>(Resource.Id.module_icon); 
            icon.Background = _healthModule.GetIcon(this);

            //Set text description
            var description = FindViewById<CustomTextView>(Resource.Id.sign_in_description);
            description.Text = Resources.GetString(Resource.String.sign_in_description, _healthModule.Name); 

            // Set the dimensions of the sign-in button.
            var signInButton = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            signInButton.SetSize(SignInButton.SizeWide);  

            signInButton.Click += delegate { SignInUtils.Instance.SignIn(); };
        }


        protected override void OnStart() {
            base.OnStart();
            SignInUtils.Instance.Connect();

            //Check if the user is already signed in
            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var idToken = prefs.GetString("IdToken", null);
            if (idToken != null) {
                StartModuleDetailActivity();
            }
        } 

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == SignInUtils.RC_SIGN_IN)  {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);

                if (result.IsSuccess) {
                    //Put User Id Token in Shared Preferences
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    var editor = prefs.Edit();
                    editor.PutString("IdToken", result.SignInAccount.IdToken);
                    editor.PutString("UserImage", result.SignInAccount.PhotoUrl.ToString());
                    editor.Apply();
                    SignInUtils.Instance.Connect();
                    StartModuleDetailActivity();
                }
                
            }
        }

        private void StartModuleDetailActivity() {
            var intent = new Intent(this, typeof(ModuleDetailActivity));
            intent.PutExtra("ShortName", _healthModule.ShortName);
            StartActivity(intent);
            Finish();

        }

    }
}
