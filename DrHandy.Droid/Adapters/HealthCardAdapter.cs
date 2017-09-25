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
using DrHandy.Droid.Custom_Views;
using Java.Util;
using DrHandy.DataBase;
using DrHandy.Droid.Listeners;
using DrHandy.Model;

namespace DrHandy.Droid.Adapters {
    
    /*
     * HealthCardAdapter - Adapter for displaying the health cards on the main screen
     */
    class HealthCardAdapter : RecyclerView.Adapter {

        //ViewHolder for the Cards
        public class CardViewHolder : RecyclerView.ViewHolder {

            //Fragment for the Module
            public RelativeLayout ModuleLayout { get; set; }
            public LinearLayout FragmentContainer { get; set; }
            public HealthCard HealthCard { get; set; }

            public CardViewHolder(View itemView) : base(itemView) {
                ModuleLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.module_layout);
                FragmentContainer = itemView.FindViewById<LinearLayout>(Resource.Id.fragments_container);
            }
        }

        private Context _context;
        private List<HealthCard> _cards = new List<HealthCard>();
        private HealthCardClickListener _listener; 


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

            var fragment = item.HealthModule.GetHealthCardFragment();
            var backgroundImage = item.HealthModule.GetHeader(_context);
            if (fragment != null) {     //If the module has a custom card fragment
                viewHolder.FragmentContainer.Visibility = ViewStates.Visible;
                viewHolder.ModuleLayout.Visibility = ViewStates.Gone;
                var oldFragment = fragmentManager.FindFragmentById(viewHolder.FragmentContainer.Id);
                if (oldFragment != null) {
                    fragmentTransaction.Remove(oldFragment);    //Remove previous fragment if exists
                }
                viewHolder.FragmentContainer.Id = (int)SystemClock.CurrentThreadTimeMillis();   //Give unique ID
                fragmentTransaction.Replace(viewHolder.FragmentContainer.Id, fragment as Fragment);
                fragmentTransaction.CommitAllowingStateLoss();
                viewHolder.FragmentContainer.Background = backgroundImage;
            } else {    //Show default fragment
                viewHolder.ModuleLayout.Visibility = ViewStates.Visible;
                viewHolder.FragmentContainer.Visibility = ViewStates.Gone;
                var moduleName = viewHolder.ModuleLayout.FindViewById<CustomTextView>(Resource.Id.module_name);
                var moduleImage = viewHolder.ModuleLayout.FindViewById<ImageView>(Resource.Id.module_icon);
                moduleName.Text = item.Name;
                moduleImage.Background = item.Icon;
                viewHolder.ModuleLayout.Background = backgroundImage;
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
