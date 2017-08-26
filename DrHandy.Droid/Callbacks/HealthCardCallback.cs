using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using DrHandy.Droid.Adapters;

namespace DrHandy.Droid.Callbacks {
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

        public override void OnChildDraw(Canvas cValue, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState,
            bool isCurrentlyActive) {

            if (actionState == ItemTouchHelper.ActionStateDrag && isCurrentlyActive) {
                viewHolder.ItemView.Alpha = 0.7f;
            }

            base.OnChildDraw(cValue, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder) {
             viewHolder.ItemView.Alpha = 1.0f;
            base.ClearView(recyclerView, viewHolder);
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