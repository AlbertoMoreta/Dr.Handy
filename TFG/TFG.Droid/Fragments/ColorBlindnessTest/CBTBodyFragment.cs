using System;
using System.Collections.Generic;
using System.Linq; 

using Android.App; 
using Android.OS; 
using Android.Views;
using Android.Widget;
using TFG.Droid.Activities;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Logic;
using TFG.Model;

namespace TFG.Droid.Fragments.ColorBlindnessTest {
    public class CBTBodyFragment : Fragment, IHealthFragment {

        

        private List<Model.ColorBlindnessQuestion> _questions;
        private List<Button> _answers = new List<Button>();
        private TextView _question;
        private LinearLayout _questionsLayout;
        private TableLayout _resultsTable;
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
            var view = inflater.Inflate(Resource.Layout.fragment_cbt_body, container, false);
            _questionsLayout = view.FindViewById<LinearLayout>(Resource.Id.questions_layout);
            _resultsTable = view.FindViewById<TableLayout>(Resource.Id.table_results);
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

                answer.Click += OnAnswerSelected;
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

        private void OnAnswerSelected(object sender, EventArgs eventArgs) {
            _logic.SubmitAnswer(((Button) sender).Text);

            _logic.CurrentQuestion ++;
            if (_logic.CurrentQuestion < ColorBlindnessLogic.TOTAL_QUESTIONS) {
                UpdateQuestion(_logic.CurrentQuestion);
                UpdateAnswers(_logic.CurrentQuestion);

                ((CBTHeaderFragment) ((ModuleDetailActivity) Activity).HeaderFragment)
                                                        .UpdateQuestion( _logic.CurrentQuestion);
            } else {
                ShowResults();
            }
        }

        private void ShowResults()  {
            InitResultsTable();
            _questionsLayout.Visibility = ViewStates.Gone; 
            _resultsTable.Visibility = ViewStates.Visible;
            ((CBTHeaderFragment) ((ModuleDetailActivity) Activity).HeaderFragment)
                                                        .ShowResult();
        }

        private void InitResultsTable() {
            foreach (ColorBlindnessQuestion question in _logic.Questions) {
                var row = (TableRow) LayoutInflater.From(Activity).Inflate(Resource.Layout.result_row, null);
                row.FindViewById<CustomTextView>(Resource.Id.question_number).Text = question.Number.ToString();
                row.FindViewById<CustomTextView>(Resource.Id.answer).Text = question.UserAnswer;
                row.FindViewById<CustomTextView>(Resource.Id.correct_answer).Text = question.CorrectAnswer;
                _resultsTable.AddView(row);
            }
        }
    }
}