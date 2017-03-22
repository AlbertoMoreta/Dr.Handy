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
using TFG.DataBase;
using TFG.Droid.Listeners;
using TFG.Model;

namespace TFG.Droid.Adapters {
    class HealthCardAdapter : RecyclerView.Adapter {

        //ViewHolder for the Cards
        public class CardViewHolder : RecyclerView.ViewHolder {

            //Fragment for the Module
            public LinearLayout Fragment{ get; set; }

            public CardViewHolder(View itemView) : base(itemView) {
                Fragment = itemView.FindViewById<LinearLayout>(Resource.Id.fragment_container);
            }
        }

        private Context _context;
        private List<HealthCard> _cards = new List<HealthCard>();
        private HealthCardClickListener _listener;
        private CardView _cardView;


        public HealthCardAdapter(Context context, List<HealthCard> cards) {
            _context = context;
            _cards = cards;
        }

        public override int ItemCount {
            get {
                return _cards.Count;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
            var itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.health_card, parent, false);

            _cardView = itemView.FindViewById<CardView>(Resource.Id.cardview);
             
            return new CardViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            CardViewHolder viewHolder = holder as CardViewHolder;
            var item = _cards[position];

            FragmentManager fragmentManager = ((Activity) _context).FragmentManager;
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            var fragment = HealthModulesInfoExtension.GetHealthCardFragmentFromHealthModuleName(item.Name);
            if (fragment != null) {
                fragmentTransaction.Add(Resource.Id.fragment_container, fragment as Fragment);
            } 

            fragmentTransaction.Commit();

            _cardView.Background = HealthModulesInfoExtension.GetHealthModuleHeaderFromHealthModuleName(_context, item.Name); 

            viewHolder.ItemView.Click += delegate { _listener.OnHealthCardClick(item.HealthModule); };
        }

        public bool OnItemMove(int fromPosition, int toPosition) {
            if(fromPosition < toPosition) {
                for (int i = fromPosition; i<toPosition; i++) {
                    DBHelper.Instance.ChangeModulePosition(_cards[i + 1].HealthModule, i);
                    DBHelper.Instance.ChangeModulePosition(_cards[i].HealthModule, i + 1);
                    Swap(_cards, i, i + 1);
                }
            }else {
                for (int i = fromPosition; i > toPosition; i--) {
                    DBHelper.Instance.ChangeModulePosition(_cards[i - 1].HealthModule, i);
                    DBHelper.Instance.ChangeModulePosition(_cards[i].HealthModule, i - 1);
                    Swap(_cards, i, i - 1);
                }
            }
            NotifyItemMoved(fromPosition, toPosition);
            return true;
        }

        public void Swap<T>(IList<T> list, int fromPosition, int toPosition) {
            T tmp = list[fromPosition];
            list[fromPosition] = list[toPosition];
            list[toPosition] = tmp; 
        }

        public void OnItemDismiss(int position) {
            _cards.RemoveAt(position);
            NotifyItemRemoved(position);
        }

        public void SetCards(List<HealthCard> cards) {
            _cards = cards;
        }

        public void SetHealthCardClickListener(HealthCardClickListener listener)  {
            _listener = listener;
        }

    }
}
