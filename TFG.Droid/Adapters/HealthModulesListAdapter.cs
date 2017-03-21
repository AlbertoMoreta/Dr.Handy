using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using TFG.DataBase;
using TFG.Droid.Custom_Views;
using TFG.Droid.Utils;
using TFG.Model;
using Object = Java.Lang.Object;

namespace TFG.Droid.Adapters {
    class HealthModulesListAdapter : BaseAdapter {  

        //ViewHolder For The Health Modules
        private class ViewHolder : Java.Lang.Object {
            public RelativeLayout Header { get; set; }
            public TextView ModuleName { get; set; }
            public TextView ModuleDescriptionShort { get; set; }
            public CustomTextView ModuleDescriptionLong { get; set; }
            public int ModuleDescriptionHeight { get; set; }
            public ImageView ModuleIcon { get; set; }
            public FloatingActionButton AddButton { get; set; }
            public View RevealView { get; set; }
            public View Background { get; set; }
        } 


        private Context _context;
        private LayoutInflater _inflater;
        private List<HealthModuleType> _modules = HealthModulesInfo.GetHealthModules;
        private List<ModuleViewCell> _viewCells = new List<ModuleViewCell>();

        public HealthModulesListAdapter(Context context) {
            _context = context;
            _inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
        } 
         

        public void AddModule(HealthModuleType module) {
            _modules.Add(module);
        }

        public void SetModules(List<HealthModuleType> modules) {
            _modules = modules;
        } 

        public override Object GetItem(int position) {
            return _viewCells.ElementAt(position);
        }

        public override long GetItemId(int position) {
            return position;
        }

        public override int Count { get { return _modules.Count;  } }

        private ViewHolder ExpandedView { get; set; }

        public override View GetView(int position, View convertView, ViewGroup parent) {

            ViewHolder viewHolder = null;
            HealthModuleType module = _modules.ElementAt(position);

            if (convertView == null) {
                viewHolder = new ViewHolder(); 
                convertView = _inflater.Inflate(Resource.Layout.module_viewcell, null);

                viewHolder.Header = convertView.FindViewById<RelativeLayout>(Resource.Id.header);
                viewHolder.ModuleName = convertView.FindViewById<TextView>(Resource.Id.module_name);
                viewHolder.ModuleDescriptionShort = convertView.FindViewById<TextView>(Resource.Id.module_description_short);
                viewHolder.ModuleDescriptionLong = convertView.FindViewById<CustomTextView>(Resource.Id.module_description_long);
                viewHolder.ModuleDescriptionLong.Measure(View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                    View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                viewHolder.ModuleDescriptionHeight = viewHolder.ModuleDescriptionLong.MeasuredHeight;
                viewHolder.ModuleIcon = convertView.FindViewById<ImageView>(Resource.Id.module_icon);
                viewHolder.AddButton = convertView.FindViewById<FloatingActionButton>(Resource.Id.module_addbutton); 
                viewHolder.RevealView = convertView.FindViewById<View>(Resource.Id.reveal);
                viewHolder.Background = convertView.FindViewById<View>(Resource.Id.background);

                convertView.Tag = viewHolder;
                viewHolder.AddButton.Click += delegate { OnAddButtonClick(module); };
                viewHolder.Header.Click += delegate {
                    if (ExpandedView != null) {
                        if (ExpandedView != viewHolder)  { 
                            OnHeaderClick(ExpandedView);    //Collapse previous HealthModule
                            ExpandedView = viewHolder;
                        } else  {
                            ExpandedView = null; 
                        }
                    } else { 
                        ExpandedView = viewHolder;
                    }

                    OnHeaderClick(viewHolder);  //Expand selected HealthModule
                };
                 
            } else {
                viewHolder = convertView.Tag as ViewHolder;
            }

            /*var drawable = (LayerDrawable) ContextCompat.GetDrawable(_context, Resource.Drawable.module_icon).Mutate();
            var moduleColorName = module.HealthModuleColor();
            var background = (GradientDrawable) drawable.FindDrawableByLayerId(Resource.Id.background).Mutate();
            background.SetColor(ContextCompat.GetColor(_context, _context.Resources.GetIdentifier(moduleColorName, "color", _context.PackageName)));
            //drawable.SetDrawableByLayerId(Resource.Id.icon, );

            viewHolder.ViewCell.IconDrawable = drawable;*/

            viewHolder.ModuleName.Text = module.HealthModuleName();
            viewHolder.ModuleDescriptionShort.Text = viewHolder.ModuleDescriptionLong.Text = module.HealthModuleDescription();
            viewHolder.RevealView.Background =
                HealthModulesInfoExtension.GetHealthModuleHeaderFromHealthModuleName(_context, module.HealthModuleName());
            if (DBHelper.Instance.CheckIfExists(module) && DBHelper.Instance.CheckIfVisible(module)) {
                viewHolder.AddButton.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.ic_clear));
                viewHolder.AddButton.BackgroundTintList = ContextCompat.GetColorStateList(_context, Resource.Color.red);


            } else {
                viewHolder.AddButton.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.ic_add));
                viewHolder.AddButton.BackgroundTintList = ContextCompat.GetColorStateList(_context, Resource.Color.green);
            } 


            return convertView;

        }

        private void OnAddButtonClick(HealthModuleType module) {
            if (DBHelper.Instance.CheckIfExists(module)) {
                if (DBHelper.Instance.CheckIfVisible(module)) {
                    DBHelper.Instance.ChangeModuleVisibility(module, false);
                }else {
                    DBHelper.Instance.ChangeModuleVisibility(module, true);
                }
            }else {
                DBHelper.Instance.AddHealthModule(module);
            }

            NotifyDataSetChanged();

        }

        private void OnHeaderClick(ViewHolder v) {
            var iconMarginLeft = ((ViewGroup.MarginLayoutParams) v.ModuleIcon.LayoutParameters).LeftMargin;
            var iconMarginTop = ((ViewGroup.MarginLayoutParams) v.ModuleIcon.LayoutParameters).TopMargin;
            var cx = (v.ModuleIcon.Left + v.ModuleIcon.Right - iconMarginLeft) / 2;
            var cy = (v.ModuleIcon.Top + v.ModuleIcon.Bottom - iconMarginTop) / 2;
            var radius = v.Header.Width;

            if (v.ModuleDescriptionLong.Visibility == ViewStates.Visible) {
                AnimationUtils.StartTranslateAnimation(v.ModuleIcon, v.ModuleIcon.Left, v.ModuleIcon.Top);
                AnimationUtils.StartScaleAnimation(v.ModuleIcon, 1f, 1f);
                AnimationUtils.HideViewCircular(v.RevealView, cx, cy, radius);
                AnimationUtils.RevealViewCircular(v.Background, cx, cy, v.Background.MeasuredHeight);
                AnimationUtils.StartTranslateAnimation(v.ModuleName, v.ModuleName.Left, v.ModuleName.Top);
                AnimationUtils.StartScaleAnimation(v.ModuleName, 1f, 1f); 
                AnimationUtils.ExpandView(v.Header, v.Header.MeasuredHeight - v.ModuleIcon.MeasuredHeight - v.ModuleName.MeasuredHeight * 2);
                AnimationUtils.FadeAnimation(v.ModuleDescriptionLong, 0f);
                AnimationUtils.ExpandView(v.ModuleDescriptionLong, 0);
                AnimationUtils.FadeAnimation(v.ModuleDescriptionShort, 1f);
                AnimationUtils.FadeAnimation(v.AddButton, 0f); 
            } else {
                AnimationUtils.HideViewCircular(v.Background, cx, cy, v.Background.MeasuredHeight, 300);
                AnimationUtils.RevealViewCircular(v.RevealView, cx, cy, radius);
                AnimationUtils.StartTranslateAnimation(v.ModuleIcon, (v.Header.Width - v.ModuleIcon.Width) / 2, (v.ModuleIcon.Height / 2));
                AnimationUtils.StartScaleAnimation(v.ModuleIcon, 1.5f, 1.5f); 
                var finalX = (v.Header.Width - v.ModuleName.Width) / 2;
                AnimationUtils.StartTranslateAnimation(v.ModuleName, finalX, v.ModuleIcon.Height * 2);
                AnimationUtils.StartScaleAnimation(v.ModuleName, 1.5f, 1.5f);
                AnimationUtils.ExpandView(v.Header, v.Header.MeasuredHeight + v.ModuleIcon.MeasuredHeight + v.ModuleName.MeasuredHeight * 2);
                AnimationUtils.FadeAnimation(v.ModuleDescriptionLong, 1f);
                AnimationUtils.ExpandView(v.ModuleDescriptionLong, v.ModuleDescriptionHeight, true);
                AnimationUtils.FadeAnimation(v.ModuleDescriptionShort, 0f);
                AnimationUtils.FadeAnimation(v.AddButton, 1f); 
            }
        }
    }
}