using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TFG.Droid.Activities;
using TFG.Droid.Interfaces;
using TFG.Logic;

namespace TFG.Droid.Fragments.ColorBlindnessTest {
    public class CBTBodyFragment : Fragment, IHealthFragment {

        

        private List<Model.ColorBlindnessQuestion> _questions;
        private List<Button> _answers = new List<Button>();
        private TextView _question;
        private Logic.ColorBlindnessLogic _logic;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            Init();
        }

        private void Init() { 
            _logic = ColorBlindnessLogic.Instance(Activity); 
            _questions = _logic.GetQuestions();

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_cbt_body, container, false);
            _question = view.FindViewById<TextView>(Resource.Id.question);
            InitAnswers(view);
            UpdateQuestion(0);
            UpdateAnswers(0);
            return view;
        }

        private void InitAnswers(View view) {
            for (var i = 0; i < ColorBlindnessLogic.ANSWERS_COUNT; i++) {
                var answer =
                    view.FindViewById<Button>(Resources.GetIdentifier("answer" + (i + 1), "id", Activity.PackageName));

                answer.Click += OnButtonClicked;
                _answers.Add(answer);
            }
            
        }

        private void UpdateQuestion(int plateNumber) { 
            _question.Text = _questions.ElementAt(plateNumber).Question;
        }

        private void UpdateAnswers(int plateNumber)  {
            for (var i = 0; i < _answers.Count; i++)  {
                _answers.ElementAt(i).Text = _questions.ElementAt(plateNumber).Answers.ElementAt(i);
            }
        }

        private void OnButtonClicked(object sender, EventArgs eventArgs) {
            _logic.CurrentQuestion ++;
            if (_logic.CurrentQuestion < ColorBlindnessLogic.TOTAL_QUESTIONS)  {
                UpdateQuestion(_logic.CurrentQuestion);
                UpdateAnswers(_logic.CurrentQuestion);

                ((CBTHeaderFragment) ((ModuleDetailActivity) Activity).HeaderFragment).UpdateQuestion(
                    _logic.CurrentQuestion);
            }
        }
    }
}