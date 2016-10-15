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
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using TFG.Droid.Adapters;

namespace TFG.Droid.Callbacks {
    class HealthCardCallback : ItemTouchHelper.Callback {

        /**
         *  Callback for the list of HealthCards  
         **/

        private readonly HealthCardAdapter _adapter;

        public HealthCardCallback(HealthCardAdapter adapter) {
            _adapter = adapter;
        }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder) {
            int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down | ItemTouchHelper.Left | ItemTouchHelper.Right;  //Support for drag and drop
            int swipeFlags = 0;     //No support for swipe
            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target) {
            //When the cards are moved
            _adapter.OnItemMove(viewHolder.AdapterPosition, target.AdapterPosition);
            return true;

        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction) {
            //Do nothing on swipe
        }

        //Start drag event from long press
        public override bool IsLongPressDragEnabled {
            get {
                return true;
            }
        }

        public override bool IsItemViewSwipeEnabled {
            get {
                return false;
            }
        }



    }
}