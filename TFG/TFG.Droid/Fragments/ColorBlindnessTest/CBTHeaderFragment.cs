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
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Logic;

namespace TFG.Droid.Fragments.ColorBlindnessTest {
    public class CBTHeaderFragment : Fragment, IHealthFragment {
        private List<Model.ColorBlindnessQuestion> _questions;
        private ImageView _questionImage;
        private CustomTextView _resultTextView;

        private Logic.ColorBlindnessLogic _logic;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            Init();

        }

        private void Init() { 
            _logic = ColorBlindnessLogic.Instance();  
            _questions = _logic.GetQuestions();
           
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
             var view = inflater.Inflate(Resource.Layout.fragment_cbt_header, container, false);
            _questionImage = view.FindViewById<ImageView>(Resource.Id.test_image);
            _resultTextView = view.FindViewById<CustomTextView>(Resource.Id.result);
            UpdateQuestion(_logic.CurrentQuestion); 

            return view;
        }


        public void UpdateQuestion(int plate) {
            var a = _logic.CurrentQuestion;
            UpdateImage(plate);
        }

        public void UpdateImage(int plate) { 
            _questionImage.SetImageResource(Resources.GetIdentifier(_questions.ElementAt(plate).ImageName, "drawable", Activity.PackageName));
        }

        public void ShowResult()  {
            _questionImage.Visibility = ViewStates.Gone;
            _resultTextView.Visibility = ViewStates.Visible;
            _resultTextView.Text = "Result";
        }
    }
}