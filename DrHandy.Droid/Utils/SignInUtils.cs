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
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Preferences;

namespace DrHandy.Droid.Utils {
    class SignInUtils { 

        public static readonly int RC_SIGN_IN = 9001;

        private static SignInUtils _instance;
        public static SignInUtils Instance {
            get{
                if (_instance == null) {
                    _instance = new SignInUtils();
                }
                return _instance;
            }
        }

        private Context _context;
        public GoogleApiClient Client { get; private set; }

        private SignInUtils() { }

        public void InitClient(Context context) {
            _context = context;
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                  .RequestEmail()
                  .RequestIdToken(_context.Resources.GetString(Resource.String.default_web_client_id))
                  .Build();

            // Build a GoogleApiClient with access to the Google Sign-In API and the
            // options specified by gso.
            Client = new GoogleApiClient.Builder(_context) 
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .Build();
        }

        public void SignIn() {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(Client); 
            ((Activity) _context).StartActivityForResult(signInIntent, RC_SIGN_IN);
            Connect();
        } 

        public async void SignOut() {
            if (Client.IsConnected) {
                var prefs = PreferenceManager.GetDefaultSharedPreferences(_context);
                var editor = prefs.Edit();
                editor.PutString("Id", null);
                editor.PutString("UserImage", null);
                editor.Apply();
                await Auth.GoogleSignInApi.RevokeAccess(Client);
                Disconnect();
            }
        }

        public void Connect() {
            Client.Connect();
        }

        public void Disconnect() {
            Client.Disconnect();
        }


    }
}