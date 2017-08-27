using System;
using System.Collections.Generic;
using System.Linq; 

using Android.App; 
using Android.OS; 
using Android.Views;
using Android.Widget;
using DrHandy.Droid.Activities;
using DrHandy.Droid.Custom_Views;
using DrHandy.Droid.Interfaces;
using DrHandy.Logic;
using DrHandy.Model;

namespace DrHandy.Droid.Fragments.ColorBlindnessTest {
    public class CBTBodyFragment : Fragment, IHealthFragment {

        

        private List<ColorBlindnessQuestion> _questions;
        private List<Button> _answers = new List<Button>(); 
        private LinearLayout _questionsLayout;
        private TableLayout _resultsTable;
        private LinearLayout _resultsLayout;
        private ColorBlindnessLogic _logic;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            Init();
        }

        private void Init() { 
            _logic = ColorBlindnessLogic.Instance();
            if (_logic.CurrentQuestion >= 24) { _logic.CurrentQuestion = 0; } 
            _questions = _logic.GetQuestions();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_cbt_body, container, false);
            _questionsLayout = view.FindViewById<LinearLayout>(Resource.Id.questions_layout);
            _resultsLayout = view.FindViewById<LinearLayout>(Resource.Id.results_layout);
            _resultsTable = view.FindViewById<TableLayout>(Resource.Id.table_results); 
            InitAnswers(view); 
            UpdateAnswers(_logic.CurrentQuestion);
            return view;
        }

        private void InitAnswers(View view) {
            for (var i = 0; i < ColorBlindnessLogic.ANSWERS_COUNT; i++) {
                var answer =
                    view.FindViewById<Button>(Resources.GetIdentifier("answer" + (i + 1), "id", Activity.PackageName));

                answer.Click += OnAnswerSelected;
                _answers.Add(answer);
            }
            
        } 

        private void UpdateAnswers(int plateNumber)  {
            for (var i = 0; i < _answers.Count; i++)  {
                _answers.ElementAt(i).Text = _questions.ElementAt(plateNumber).Answers.ElementAt(i);
            }
        }

        private void OnAnswerSelected(object sender, EventArgs eventArgs) {
            foreach (var answer in _answers) { answer.Enabled = false; }

            _logic.SubmitAnswer(((Button) sender).Text);

            _logic.CurrentQuestion ++;
            if (_logic.CurrentQuestion < ColorBlindnessLogic.TOTAL_QUESTIONS) { 
                UpdateAnswers(_logic.CurrentQuestion);

                ((CBTHeaderFragment) ((ModuleDetailActivity) Activity).HeaderFragment)
                                                        .UpdateQuestion( _logic.CurrentQuestion);
            } else {
                ShowResults();
            }

            foreach (var answer in _answers) { answer.Enabled = true; }
        }

        private void ShowResults()  {
            InitResultsTable();
            _questionsLayout.Visibility = ViewStates.Gone; 
            _resultsLayout.Visibility = ViewStates.Visible;
            ((CBTHeaderFragment) ((ModuleDetailActivity) Activity).HeaderFragment)
                                                        .ShowResult();
        }

        private void InitResultsTable() {
            foreach (ColorBlindnessQuestion question in _logic.Questions) {
                var row = (TableRow) LayoutInflater.From(Activity).Inflate(Resource.Layout.result_row, null);

                //Add question number to result row
                var questionNumber = row.FindViewById<CustomTextView>(Resource.Id.question_number);
                questionNumber.LayoutParameters = new TableRow.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);
                questionNumber.Text = question.Number.ToString();

                //Add answer to result row
                var answer = row.FindViewById<CustomTextView>(Resource.Id.answer);
                answer.LayoutParameters = new TableRow.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);
                answer.Text = question.UserAnswer;

                //Add correct answer to result row
                var correctAnswer = row.FindViewById<CustomTextView>(Resource.Id.correct_answer);
                correctAnswer.LayoutParameters = new TableRow.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);
                correctAnswer.Text = question.Answers.ElementAt(question.CorrectAnswerPos);

                _resultsTable.AddView(row);
            }
        }
    }
}