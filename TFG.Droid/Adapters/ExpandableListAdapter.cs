using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using TFG.Droid.Custom_Views;
using TFG.Model;
using Object = Java.Lang.Object;

namespace TFG.Droid.Adapters {
    class ExpandableListAdapter : BaseExpandableListAdapter {  

        //ViewHolder For The Health Modules
        private class GroupViewHolder : Java.Lang.Object {
            public ModuleViewCell ViewCell { get; set; }
        }

        //ViewHolder For The Detailed Description
        private class ChildViewHolder : Java.Lang.Object {
            public CustomTextView ModuleDescription { get; set; }
        }


        private Context _context;
        private LayoutInflater _inflater;
        private List<HealthModuleType> _modules = HealthModulesInfo.GetHealthModules;
        private List<ModuleViewCell> _viewCells = new List<ModuleViewCell>();

        public ExpandableListAdapter(Context context) {
            _context = context;
            _inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
        } 
         

        public void AddModule(HealthModuleType module) {
            _modules.Add(module);
        }

        public void SetModules(List<HealthModuleType> modules) {
            _modules = modules;
        }



        public override int GroupCount { get { return _modules.Count; } }
        public override bool HasStableIds { get; } = true; 

        public override Object GetGroup(int groupPosition) {
            return _viewCells.ElementAt(groupPosition);
        }

        public override long GetGroupId(int groupPosition) { 
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent) { 

            GroupViewHolder viewHolder = null;
            HealthModuleType module = _modules.ElementAt(groupPosition);

            if (convertView == null) {
                viewHolder = new GroupViewHolder();
                convertView = new ModuleViewCell(_context);
                viewHolder.ViewCell = convertView as ModuleViewCell;
                convertView.Tag = viewHolder;
                viewHolder.ViewCell.AddButton.Click += delegate { OnAddButtonClick(module); };
            } else {
                viewHolder = convertView.Tag as GroupViewHolder;
            }

            /*var drawable = (LayerDrawable) ContextCompat.GetDrawable(_context, Resource.Drawable.module_icon).Mutate();
            var moduleColorName = module.HealthModuleColor();
            var background = (GradientDrawable) drawable.FindDrawableByLayerId(Resource.Id.background).Mutate();
            background.SetColor(ContextCompat.GetColor(_context, _context.Resources.GetIdentifier(moduleColorName, "color", _context.PackageName)));
            //drawable.SetDrawableByLayerId(Resource.Id.icon, );

            viewHolder.ViewCell.IconDrawable = drawable;*/
            viewHolder.ViewCell.Name = module.HealthModuleName();
            viewHolder.ViewCell.Description = module.HealthModuleDescription();
            viewHolder.ViewCell.AddButtonImage = DBHelper.Instance.CheckIfExists(module) &&
                                                 DBHelper.Instance.CheckIfVisible(module) ? ContextCompat.GetDrawable(_context, Resource.Drawable.ic_remove)
                                                                                          : ContextCompat.GetDrawable(_context, Resource.Drawable.ic_add);


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


        public override Object GetChild(int groupPosition, int childPosition) {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition) {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition) {
            return 1;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent) {
            ChildViewHolder viewHolder;
            HealthModuleType module = _modules.ElementAt(groupPosition);

            if (convertView == null) {
                viewHolder = new ChildViewHolder();
                convertView = _inflater.Inflate(Resource.Layout.expandable_listview_child, parent, false);
                viewHolder.ModuleDescription = convertView.FindViewById<CustomTextView>(Resource.Id.description);
                viewHolder.ModuleDescription.Text = _modules.ElementAt(groupPosition).HealthModuleDescription(); 
                convertView.Tag = viewHolder;
            } else {
                viewHolder = convertView.Tag as ChildViewHolder;
            }

            viewHolder.ModuleDescription.Text = _modules.ElementAt(groupPosition).HealthModuleDescription();

            return convertView;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition) {
            return false;
        }
    }
}