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
            public CustomTextView ModuleName { get; set; }
            public ImageView ModuleImage { get; set; }
            public HealthCard HealthCard { get; set; }

            public CardViewHolder(View itemView) : base(itemView) {
                ModuleName = itemView.FindViewById<CustomTextView>(Resource.Id.module_name);
                ModuleImage = itemView.FindViewById<ImageView>(Resource.Id.module_image);
            }
        }

        private Context _context;
        private List<HealthCard> _cards = new List<HealthCard>();
        private HealthCardClickListener _listener;
        private RelativeLayout _moduleLayout;
        private LinearLayout _fragmentContainer;


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

            _moduleLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.module_layout);
            _fragmentContainer = itemView.FindViewById<LinearLayout>(Resource.Id.fragments_container);

            var viewHolder = new CardViewHolder(itemView);

            viewHolder.ItemView.Click += delegate { _listener.OnHealthCardClick(viewHolder.HealthCard.HealthModule); };

            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            CardViewHolder viewHolder = holder as CardViewHolder;
            var item = _cards[position];

            viewHolder.HealthCard = item;

            FragmentManager fragmentManager = ((Activity) _context).FragmentManager;
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            var fragment = HealthModulesInfoExtension.GetHealthCardFragmentFromHealthModuleName(item.Name);
            var backgroundImage = HealthModulesInfoExtension.GetHealthModuleHeaderFromHealthModuleName(_context, item.Name);
            if (fragment != null) {
                _fragmentContainer.Visibility = ViewStates.Visible;
                _moduleLayout.Visibility = ViewStates.Gone;
                fragmentTransaction.Replace(Resource.Id.fragments_container, fragment as Fragment);
                fragmentTransaction.Commit();
                _fragmentContainer.Background = backgroundImage;
            } else {
                _moduleLayout.Visibility = ViewStates.Visible;
                _fragmentContainer.Visibility = ViewStates.Gone;
                viewHolder.ModuleName.Text = item.Name;
                _moduleLayout.Background = backgroundImage;
            }
  

            
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
