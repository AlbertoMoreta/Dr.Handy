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

namespace TFG.Droid.Adapters {
    class StableArrayAdapter : ArrayAdapter<View>{

        readonly int INVALID_ID = -1;

        Dictionary<View, int> _idMap = new Dictionary<View, int>(); 

        public StableArrayAdapter(Context context, int textViewResourceId, List<View> objects) 
            : base(context, textViewResourceId, objects) { 
            for (int i = 0; i < objects.Count; ++i) {
                _idMap.Add(objects.ElementAt(i), i);
            }
        }

        public override long GetItemId(int position) {
            if (position < 0 || position >= _idMap.Count) {
                return INVALID_ID;
            }
            View item = GetItem(position);
            return _idMap[item];
        }

        public override bool HasStableIds {
            get {
                return true;
            }
        }

    }
}