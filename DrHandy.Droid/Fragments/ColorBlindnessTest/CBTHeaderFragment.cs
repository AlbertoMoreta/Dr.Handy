using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using DrHandy.Droid.Activities;
using DrHandy.Droid.Custom_Views;
using DrHandy.Droid.Interfaces;
using DrHandy.Logic;

namespace DrHandy.Droid.Fragments.ColorBlindnessTest {
    public class CBTHeaderFragment : Fragment, IHealthFragment {
        private List<Model.ColorBlindnessQuestion> _questions;
        private ImageView _questionImage;
        private CustomTextView _infoText;

        private Logic.ColorBlindnessLogic _logic;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            Init();

        }

        private void Init() { 
            _logic = ColorBlindnessLogic.Instance();
            if (_logic.CurrentQuestion >= 24) { _logic.CurrentQuestion = 0;
                _logic.CorrectAnswersCount = 0;
                _logic.RGColorBlindnessCount = 0;
                _logic.TotalColorBlindnessCount = 0;
            }
            _questions = _logic.GetQuestions();
           
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
             var view = inflater.Inflate(Resource.Layout.fragment_cbt_header, container, false);
            _questionImage = view.FindViewById<ImageView>(Resource.Id.test_image);
            _infoText = view.FindViewById<CustomTextView>(Resource.Id.info_text);
            UpdateQuestion(_logic.CurrentQuestion); 

            return view;
        }


        public void UpdateQuestion(int plate) {

            //Show help dialogs
            if (plate == 0) { new CBTInfoDialog(Activity, 1); }
            if (plate == 17) { new CBTInfoDialog(Activity, 2); }

            ((ModuleDetailActivity) Activity).ToolbarTitle.Text = _questions.ElementAt(plate).Question;

            //Update Question Image
            _questionImage.SetImageResource(Resources.GetIdentifier(_questions.ElementAt(plate).ImageName, "drawable", Activity.PackageName));

            //Update Question Number
            _infoText.Text = (plate + 1) + " / " + ColorBlindnessLogic.TOTAL_QUESTIONS;
        }
         

        public void ShowResult()  {

            ((ModuleDetailActivity)Activity).ToolbarTitle.Text = Resources.GetString(Resource.String.results);
            _questionImage.Visibility = ViewStates.Gone;
            _infoText.SetTextSize(ComplexUnitType.Px, Resources.GetDimension(Resources.GetIdentifier("text_size_large", "dimen",
                Activity.PackageName)));
            Log.Verbose("TAG", "Correct: " + _logic.CorrectAnswersCount);
            Log.Verbose("TAG", "RG: " + _logic.RGColorBlindnessCount);
            Log.Verbose("TAG", "Total: " + _logic.TotalColorBlindnessCount);

            if (_logic.CorrectAnswersCount >= 17) {
                _infoText.Text =
                    Resources.GetString(Resources.GetIdentifier("no_daltonism", "string",
                        Activity.PackageName));
            } else if(_logic.RGColorBlindnessCount >= _logic.TotalColorBlindnessCount) {
                _infoText.Text =
                    Resources.GetString(Resources.GetIdentifier("red_and_green_daltonism", "string",
                        Activity.PackageName));
            } else if (_logic.RGColorBlindnessCount < _logic.TotalColorBlindnessCount) {
                _infoText.Text =
                    Resources.GetString(Resources.GetIdentifier("total_daltonism", "string",
                        Activity.PackageName));
            }  
        }
    }
}