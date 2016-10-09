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
using Android.Support.V7.Widget; 
using TFG.Droid.Custom_Views;
using Java.Util;

namespace TFG.Droid.Adapters {
    class HealthCardAdapter : RecyclerView.Adapter {

        //ViewHolder for the Cards
        public class CardViewHolder : RecyclerView.ViewHolder {

            //Name of the Module
            public TextView Name { get; set; }

            public CardViewHolder(View itemView) : base(itemView) {
                Name = itemView.FindViewById<TextView>(Resource.Id.module_name);
            }
        }

        private List<HealthCard> _cards = new List<HealthCard>();

        public HealthCardAdapter(List<HealthCard> cards) {
            _cards = cards;
        }

        public override int ItemCount {
            get {
                return _cards.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
            View itemView = LayoutInflater.From(parent.Context).
            Inflate(Resource.Layout.health_card, parent, false);

            return new CardViewHolder(itemView); 
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            CardViewHolder viewHolder = holder as CardViewHolder;
            viewHolder.Name.Text = _cards[position].Name; 
        }

        public bool OnItemMove(int fromPosition, int toPosition) {
            if(fromPosition < toPosition) {
                for (int i = fromPosition; i<toPosition; i++) {
                    var initialCard = _cards[i];
                    var finalCard = _cards[i + 1];
                    _cards.RemoveAt(i + 1);
                    _cards.Insert(i + 1, initialCard);
                    _cards.RemoveAt(i);
                    _cards.Insert(i, finalCard); 
                }
            }else {
                for (int i = fromPosition; i > toPosition; i--) {
                    var initialCard = _cards[i];
                    var finalCard = _cards[i - 1];
                    _cards.RemoveAt(i - 1);
                    _cards.Insert(i - 1, initialCard);
                    _cards.RemoveAt(i);
                    _cards.Insert(i, finalCard);
                }
            }
            NotifyItemMoved(fromPosition, toPosition);
            return true;
        }

        public void OnItemDismiss(int position) {
            _cards.RemoveAt(position);
            NotifyItemRemoved(position);
        }

        public void SetCards(List<HealthCard> cards) {
            _cards = cards;
        }
    }
}