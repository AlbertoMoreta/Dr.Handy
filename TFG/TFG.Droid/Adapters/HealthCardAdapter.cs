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

namespace TFG.Droid.Adapters { 
    class HealthCardAdapter : BaseAdapter{

        //ViewHolder For The Cards
        private class ViewHolder : Java.Lang.Object {
            public HealthCard Card { get; set; }
        }
              
        private List<HealthCard> cards = new List<HealthCard>();

        private Context context;
        private LayoutInflater inflater;

        public HealthCardAdapter(Context context) {
            this.context = context;
            inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
        } 

        public void SetCards(List<HealthCard> cards) {
            this.cards = cards;
        }

        public void AddCard(HealthCard card) {
            cards.Add(card);
        } 

        public override int Count {
            get {
                return cards.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position) {
            return cards.ElementAt(position);
        }

        public override long GetItemId(int position) {
            return position;
        }

        //Get Card
        public override View GetView(int position, View convertView, ViewGroup parent) {

            ViewHolder viewHolder = null;

            if (convertView == null) {
                viewHolder = new ViewHolder();
                convertView = new HealthCard(context);
                viewHolder.Card = convertView as HealthCard;
                convertView.Tag = viewHolder; 
            } else {
                viewHolder = convertView.Tag as ViewHolder;
            }

            viewHolder.Card.Name = cards.ElementAt(position).Name;

            return convertView;

        }  


    }
}