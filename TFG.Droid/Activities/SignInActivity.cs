﻿using System;
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

namespace TFG.Droid.Activities { 
    [Activity(Label = "SignInActivity", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]

    public class SignInActivity : BaseActivity{

        private static readonly int RC_SIGN_IN = 9001;

        private GoogleApiClient _googleApiClient;
        private HealthModule _healthModule;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.sign_in);
            SetUpToolBar();

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


            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestEmail()
                    .RequestIdToken(Resources.GetString(Resource.String.default_web_client_id)) 
                    .Build();

            // Build a GoogleApiClient with access to the Google Sign-In API and the
            // options specified by gso.
            _googleApiClient = new GoogleApiClient.Builder(this)  
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso) 
                .Build();

            signInButton.Click += delegate { GoogleSignIn(); };
        }


        protected override void OnResume() {
            base.OnResume();
            _googleApiClient.Disconnect();

            if(_googleApiClient != null && _googleApiClient.IsConnected) { 
                StartModuleDetailActivity();
            }
        }

        private void GoogleSignIn() { 
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == RC_SIGN_IN)  {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);

                if (result.IsSuccess) {
                    //Put User Id Token in Shared Preferences
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    var editor = prefs.Edit();
                    editor.PutString("IdToken", result.SignInAccount.IdToken);
                    editor.Apply();
                     
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
