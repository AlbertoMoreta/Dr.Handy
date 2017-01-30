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
using AnimationUtils = TFG.Droid.Utils.AnimationUtils;

namespace TFG.Droid {
    [Activity(Label = "ModuleListActivity", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask)]
    public class ModuleListActivity : BaseActivity {

        private ExpandableListView _modulesList;

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

            //Background View
            var reveal = e.ClickedView.FindViewById<View>(Resource.Id.reveal);

            var background = e.ClickedView.FindViewById<View>(Resource.Id.background);
            //Calculate icon center
            var icon = e.ClickedView.FindViewById<ImageView>(Resource.Id.module_icon);
             
            var cx = (icon.Left + icon.Right) / 2;
            var cy = (icon.Top + icon.Bottom) / 2;
            var radius = _modulesList.Width;

            if (_modulesList.IsGroupExpanded(e.GroupPosition)) {
                AnimationUtils.HideViewCircular(reveal, cx, cy, radius);
                AnimationUtils.AnimateIcon(icon, cx, cy, 1f);
                AnimationUtils.RevealViewCircular(background, cx, cy, 50);
                _modulesList.CollapseGroup(e.GroupPosition);
            }  else { 
                AnimationUtils.HideViewCircular(background, cx, cy, 50); 
                AnimationUtils.AnimateIcon(icon, _modulesList.Width / 2, 30, 1.5f);
                AnimationUtils.RevealViewCircular(reveal, cx, cy, radius);
                _modulesList.ExpandGroup(e.GroupPosition);
            }


        }

        
    }
}