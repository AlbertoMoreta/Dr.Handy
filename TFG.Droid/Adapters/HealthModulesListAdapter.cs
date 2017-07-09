using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using TFG.DataBase;
using TFG.Droid.Custom_Views;
using TFG.Droid.Utils;
using TFG.Model;
using Object = Java.Lang.Object;

namespace TFG.Droid.Adapters {
    class HealthModulesListAdapter : RecyclerView.Adapter {  

        //ViewHolder For The Health Modules
        private class ViewHolder : RecyclerView.ViewHolder {
            public RelativeLayout Header { get; set; }
            public TextView ModuleName { get; set; }
            public TextView ModuleDescriptionShort { get; set; }
            public CustomTextView ModuleDescriptionLong { get; set; }
            public int ModuleDescriptionHeight { get; set; }
            public ImageView ModuleIcon { get; set; }
            public FloatingActionButton AddButton { get; set; }
            public View RevealView { get; set; }
            public View Background { get; set; }

            public ViewHolder(View itemView) : base(itemView) {
                Header = itemView.FindViewById<RelativeLayout>(Resource.Id.header);
                ModuleName = itemView.FindViewById<TextView>(Resource.Id.module_name);
                ModuleDescriptionShort = itemView.FindViewById<TextView>(Resource.Id.module_description_short);
                ModuleDescriptionLong = itemView.FindViewById<CustomTextView>(Resource.Id.module_description_long);
                ModuleIcon = itemView.FindViewById<ImageView>(Resource.Id.module_icon);
                AddButton = itemView.FindViewById<FloatingActionButton>(Resource.Id.module_addbutton);
                RevealView = itemView.FindViewById<View>(Resource.Id.reveal);
                Background = itemView.FindViewById<View>(Resource.Id.background);
            }
        } 


        private Context _context;
        private LayoutInflater _inflater;
        private List<HealthModule> _modules;
        private List<ModuleViewCell> _viewCells = new List<ModuleViewCell>();
        private bool Enabled { get; set; } = true;

        public HealthModulesListAdapter(Context context, List<HealthModule> healthModules) {
            _context = context;
            _inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
            _modules = healthModules;
        } 
         

        public void AddModule(HealthModule module) {
            _modules.Add(module);
        }

        public void SetModules(List<HealthModule> modules) {
            _modules = modules;
        }

        public override int ItemCount {
            get {
                return _modules.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
            var itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.module_viewcell, parent, false);

            var viewHolder = new ViewHolder(itemView);

            viewHolder.AddButton.Click += delegate { OnAddButtonClick(viewHolder); };
            viewHolder.ItemView.Click += delegate {
                if (Enabled) {
                    if (ExpandedView != null) {
                        if (ExpandedView != viewHolder) {
                            OnHeaderClick(ExpandedView); //Collapse previous HealthModule
                            ExpandedView = viewHolder;
                        } else {
                            ExpandedView = null;
                        }
                    } else {
                        ExpandedView = viewHolder;
                    }

                    OnHeaderClick(viewHolder); //Expand selected HealthModule
                }
            };

            return viewHolder;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            HealthModulesListAdapter.ViewHolder viewHolder = holder as HealthModulesListAdapter.ViewHolder;
            var item = _modules[position];

            var moduleColor = ContextCompat.GetColorStateList(_context, _context.Resources.GetIdentifier(item.Color, "color", _context.PackageName));

            if (viewHolder != null) {
                var drawable = (LayerDrawable) viewHolder.Background.Background;
                var background = (GradientDrawable) drawable.FindDrawableByLayerId(Resource.Id.background);
                background.SetColor(moduleColor);
            }

            viewHolder.ModuleName.Text = item.Name;
            viewHolder.ModuleIcon.Background = item.GetIcon(_context);
            viewHolder.ModuleName.SetTextColor(Color.DimGray);
            viewHolder.ModuleDescriptionShort.Text = viewHolder.ModuleDescriptionLong.Text = item.Description;
            viewHolder.ModuleDescriptionLong.SetTextColor(moduleColor);
            viewHolder.RevealView.Background = item.GetHeader(_context);

            if (DBHelper.Instance.CheckIfExists(item) && DBHelper.Instance.CheckIfVisible(item)) {
                viewHolder.AddButton.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.ic_clear));
                viewHolder.AddButton.BackgroundTintList = ContextCompat.GetColorStateList(_context, Resource.Color.red);

            } else {
                viewHolder.AddButton.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.ic_add));
                viewHolder.AddButton.BackgroundTintList = ContextCompat.GetColorStateList(_context, Resource.Color.green);
            }

        }

        public bool IsEnabled(int position) {
            return Enabled;
        }

        private ViewHolder ExpandedView { get; set; }

        private void OnAddButtonClick(ViewHolder viewHolder)  {

            var module = _modules.Find(x => x.Name == viewHolder.ModuleName.Text);

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
            ValueAnimator headerAnimator = null;

            if (v.ModuleDescriptionLong.Visibility == ViewStates.Visible) { 

                if (Enabled) {
                    AnimationUtils.StartTranslateAnimation(v.ModuleIcon, v.ModuleIcon.Left, v.ModuleIcon.Top);
                    AnimationUtils.StartScaleAnimation(v.ModuleIcon, 1f, 1f);
                    AnimationUtils.HideViewCircular(v.RevealView, cx, cy, radius);
                    AnimationUtils.RevealViewCircular(v.Background, cx, cy, v.Background.MeasuredHeight);
                    AnimationUtils.StartTranslateAnimation(v.ModuleName, v.ModuleName.Left, v.ModuleName.Top);
                    AnimationUtils.StartScaleAnimation(v.ModuleName, 1f, 1f);
                    headerAnimator = AnimationUtils.ExpandView(v.Header,
                        v.Header.MeasuredHeight - v.ModuleIcon.MeasuredHeight - v.ModuleName.MeasuredHeight * 2);
                    AnimationUtils.FadeAnimation(v.ModuleDescriptionLong, 0f);
                    AnimationUtils.ExpandView(v.ModuleDescriptionLong, 0).Start();
                    AnimationUtils.FadeAnimation(v.ModuleDescriptionShort, 1f);
                    AnimationUtils.FadeAnimation(v.AddButton, 0f);
                    v.ModuleName.SetTextColor(Color.DimGray);
                }

            } else {
                    AnimationUtils.HideViewCircular(v.Background, cx, cy, v.Background.MeasuredHeight, 300);
                    AnimationUtils.RevealViewCircular(v.RevealView, cx, cy, radius);
                    AnimationUtils.StartTranslateAnimation(v.ModuleIcon, (v.Header.Width - v.ModuleIcon.Width) / 2,
                        (v.ModuleIcon.Height / 2));
                    AnimationUtils.StartScaleAnimation(v.ModuleIcon, 1.5f, 1.5f);
                    var finalX = (v.Header.Width - v.ModuleName.Width) / 2;
                    AnimationUtils.StartTranslateAnimation(v.ModuleName, finalX, v.ModuleIcon.Height * 2);
                    AnimationUtils.StartScaleAnimation(v.ModuleName, 1.5f, 1.5f);
                    headerAnimator = AnimationUtils.ExpandView(v.Header,
                        v.Header.MeasuredHeight + v.ModuleIcon.MeasuredHeight + v.ModuleName.MeasuredHeight * 2);
                    AnimationUtils.FadeAnimation(v.ModuleDescriptionLong, 1f);
                    AnimationUtils.ExpandView(v.ModuleDescriptionLong, v.ModuleDescriptionHeight, true).Start();
                    AnimationUtils.FadeAnimation(v.ModuleDescriptionShort, 0f);
                    AnimationUtils.FadeAnimation(v.AddButton, 1f);
                    v.ModuleName.SetTextColor(Color.White); 
            }

            if (headerAnimator != null) {
                headerAnimator.AnimationStart += delegate { Enabled = false; };
                headerAnimator.AnimationEnd += delegate { Enabled = true; };
                headerAnimator.Start();
            }
        }
    }
}