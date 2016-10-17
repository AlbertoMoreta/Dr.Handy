using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TFG.Logic;

namespace TFG.Droid.Fragments.ColorBlindnessTest {
    public class CBTHeaderFragment : Fragment
    {
        private List<Model.ColorBlindnessQuestion> _questions;
        private ImageView _questionImage;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            Init();

        }

        private void Init() {
            Logic.ColorBlindnessLogic logic = new ColorBlindnessLogic(Activity);
            _questions = logic.GetQuestions();

           
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
             var view = inflater.Inflate(Resource.Layout.fragment_cbt_header, container, false);
            _questionImage = view.FindViewById<ImageView>(Resource.Id.test_image);
            _questionImage.SetImageResource(Resources.GetIdentifier(_questions.ElementAt(0).ImageName, "drawable", Activity.PackageName));

            return view;
        }
    }
}