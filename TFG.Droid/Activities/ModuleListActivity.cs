using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using TFG.Droid.Adapters;
using TFG.Droid.Custom_Views;
using AnimationUtils = TFG.Droid.Utils.AnimationUtils;

namespace TFG.Droid {
    [Activity(Label = "ModuleListActivity", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask)]
    public class ModuleListActivity : BaseActivity {

        private ListView _modulesList;
        private int _titleStartX;
        private int _titleStartY;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            var theme = Resources.GetIdentifier("AppTheme_purple", "style", PackageName);
            if (theme != -1) { SetTheme(theme); }

            SetContentView(Resource.Layout.modules_list);
            SetUpToolBar();

            HealthModulesListAdapter adapter = new HealthModulesListAdapter(this);
            
            _modulesList = FindViewById<ListView>(Resource.Id.listView);
            _modulesList.Adapter = adapter;

            _modulesList.ItemClick += OnGroupClicked; 

        }


        private void OnGroupClicked(object s, AdapterView.ItemClickEventArgs e) {
            var duration = 500;
            int offset = 0; 
            _modulesList.SmoothScrollToPositionFromTop(e.Position, offset, duration);
            _modulesList.SetSelection(e.Position);

            //View Cell Background
            var reveal = e.View.FindViewById<View>(Resource.Id.reveal);
            var title = e.View.FindViewById<CustomTextView>(Resource.Id.module_name);
            if (_titleStartX == 0) _titleStartX = title.Left;
            if (_titleStartY == 0) _titleStartY = title.Top;

            var descriptionShort = e.View.FindViewById<TextView>(Resource.Id.module_description_short);
            var detailPanel = e.View.FindViewById<LinearLayout>(Resource.Id.detail_panel);
            var addButton = e.View.FindViewById<Button>(Resource.Id.module_addbutton);
            //Icon Background
            var background = e.View.FindViewById<View>(Resource.Id.background);
            //Calculate icon center
            var icon = e.View.FindViewById<ImageView>(Resource.Id.module_icon);
             
            var cx = (icon.Left + icon.Right) / 2;
            var cy = (icon.Top + icon.Bottom) / 2;
            var radius = _modulesList.Width;

            if (detailPanel.Visibility == ViewStates.Visible) { 
                AnimationUtils.FadeAnimation(detailPanel, 0f);
                AnimationUtils.StartScaleAnimation(detailPanel,0,0);
                AnimationUtils.HideViewCircular(reveal, cx, cy, radius);
                AnimationUtils.StartTranslateAnimation(icon, icon.Left, icon.Top);
                AnimationUtils.StartScaleAnimation(icon, 1f, 1f);
                AnimationUtils.FadeAnimation(descriptionShort, 1f);
                AnimationUtils.FadeAnimation(addButton, 0f);
                AnimationUtils.StartTranslateAnimation(title, title.Left, title.Top);
                AnimationUtils.StartScaleAnimation(title, 1f, 1f);
                AnimationUtils.RevealViewCircular(background, cx, cy, 50);
                detailPanel.Visibility = ViewStates.Gone;
                // _modulesList.CollapseGroup(e.Position);
            }  else {
                AnimationUtils.FadeAnimation(detailPanel, 1f);
                AnimationUtils.StartScaleAnimation(detailPanel, 0, detailPanel.Height);
                AnimationUtils.HideViewCircular(background, cx, cy, 50); 
                AnimationUtils.StartTranslateAnimation(icon, (_modulesList.Width / 2) - cx, e.View.Height / 5);
                AnimationUtils.StartScaleAnimation(icon, 1.5f, 1.5f);
                AnimationUtils.FadeAnimation(descriptionShort, 0f);
                AnimationUtils.FadeAnimation(addButton, 1f);
                AnimationUtils.StartTranslateAnimation(title, (_modulesList.Width / 2) - cx, (int) (e.View.Height /1.2));
                AnimationUtils.StartScaleAnimation(title, 1.5f, 1.5f);
                AnimationUtils.RevealViewCircular(reveal, cx, cy, radius);
                detailPanel.Visibility = ViewStates.Visible;
                //_modulesList.ExpandGroup(e.Position);
            }


        }

        
    }
}