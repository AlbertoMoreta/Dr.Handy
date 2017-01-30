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

        private ExpandableListView _modulesList;
        private int _titleStartX;
        private int _titleStartY;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            var theme = Resources.GetIdentifier("AppTheme_purple", "style", PackageName);
            if (theme != -1) { SetTheme(theme); }

            SetContentView(Resource.Layout.modules_list);
            SetUpToolBar();

            ExpandableListAdapter adapter = new ExpandableListAdapter(this);
            
            _modulesList = FindViewById<ExpandableListView>(Resource.Id.listView);
            _modulesList.SetAdapter(adapter);

            _modulesList.GroupClick += OnGroupClicked; 

        }


        private void OnGroupClicked(object s, ExpandableListView.GroupClickEventArgs e) {
            var duration = 500;
            int offset = 0; 
            _modulesList.SmoothScrollToPositionFromTop(e.GroupPosition, offset, duration);
            _modulesList.SetSelection(e.GroupPosition);

            //View Cell Background
            var reveal = e.ClickedView.FindViewById<View>(Resource.Id.reveal);
            var title = e.ClickedView.FindViewById<CustomTextView>(Resource.Id.module_name);
            if (_titleStartX == 0) _titleStartX = title.Left;
            if (_titleStartY == 0) _titleStartY = title.Top;

            var description = e.ClickedView.FindViewById<TextView>(Resource.Id.module_description);
            var addButton = e.ClickedView.FindViewById<Button>(Resource.Id.module_addbutton);
            //Icon Background
            var background = e.ClickedView.FindViewById<View>(Resource.Id.background);
            //Calculate icon center
            var icon = e.ClickedView.FindViewById<ImageView>(Resource.Id.module_icon);
             
            var cx = (icon.Left + icon.Right) / 2;
            var cy = (icon.Top + icon.Bottom) / 2;
            var radius = _modulesList.Width;

            if (_modulesList.IsGroupExpanded(e.GroupPosition)) {
                AnimationUtils.HideViewCircular(reveal, cx, cy, radius);
                AnimationUtils.StartTranslateAndScaleAnimation(icon, icon.Left, icon.Top, 1f);
                AnimationUtils.FadeAnimation(description, 1f);
                AnimationUtils.FadeAnimation(addButton, 0f);
                AnimationUtils.StartTranslateAndScaleAnimation(title, title.Left, title.Top, 1f);
                AnimationUtils.RevealViewCircular(background, cx, cy, 50);
                _modulesList.CollapseGroup(e.GroupPosition);
            }  else { 
                AnimationUtils.HideViewCircular(background, cx, cy, 50); 
                AnimationUtils.StartTranslateAndScaleAnimation(icon, (_modulesList.Width / 2) - cx, e.ClickedView.Height / 5, 1.5f);
                AnimationUtils.FadeAnimation(description, 0f);
                AnimationUtils.FadeAnimation(addButton, 1f);
                AnimationUtils.StartTranslateAndScaleAnimation(title, (_modulesList.Width / 2) - cx, (int) (e.ClickedView.Height /1.2), 1.5f);
                AnimationUtils.RevealViewCircular(reveal, cx, cy, radius);
                _modulesList.ExpandGroup(e.GroupPosition);
            }


        }

        
    }
}