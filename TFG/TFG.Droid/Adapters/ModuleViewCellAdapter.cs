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
using TFG.Droid.Custom_Views;
using TFG.Model;

namespace TFG.Droid.Adapters {
    class ModuleViewCellAdapter : BaseAdapter {  

        //ViewHolder For The Health Modules
        private class ViewHolder : Java.Lang.Object {
            public ModuleViewCell ViewCell { get; set; }
        }

        private Context _context;
        private LayoutInflater _inflater;
        private List<HealthModule> _modules = HealthModules.GetHealthModules;
        private List<ModuleViewCell> _viewCells = new List<ModuleViewCell>();

        public ModuleViewCellAdapter(Context context) {
            _context = context;
            _inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
        } 
         

        public void AddModule(HealthModule module) {
            _modules.Add(module);
        }

        public void SetModules(List<HealthModule> modules) {
            _modules = modules;
        } 

        public override int Count {
            get {
                return _modules.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position) {
            return _viewCells.ElementAt(position);
        }

        public override long GetItemId(int position) {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent) {

            ViewHolder viewHolder = null;
            HealthModule module = _modules.ElementAt(position);

            if (convertView == null) {
                viewHolder = new ViewHolder();
                convertView = new ModuleViewCell(_context);
                viewHolder.ViewCell = convertView as ModuleViewCell;
                convertView.Tag = viewHolder;
                viewHolder.ViewCell.AddButton.Click += delegate { OnAddButtonClick(module); };
            } else {
                viewHolder = convertView.Tag as ViewHolder;
            }


            viewHolder.ViewCell.Name = module.HealthModuleName();
            viewHolder.ViewCell.Description = module.HealthModuleDescription();
            viewHolder.ViewCell.AddButtonImage = DBHelper.Instance.CheckIfExists(module) &&
                                                 DBHelper.Instance.CheckIfVisible(module) ? _context.GetDrawable(Resource.Drawable.ic_remove)
                                                                                          : _context.GetDrawable(Resource.Drawable.ic_add);


            return convertView;

        }  

        private void OnAddButtonClick(HealthModule module) {
            if (DBHelper.Instance.CheckIfExists(module)) {
                if (DBHelper.Instance.CheckIfVisible(module)) {
                    DBHelper.Instance.ChangeModuleVisibility(module, false);
                }else {
                    DBHelper.Instance.ChangeModuleVisibility(module, true);
                }
            }else {
                DBHelper.Instance.AddHealthModule(module);
            }

        }
         
       
    }
}