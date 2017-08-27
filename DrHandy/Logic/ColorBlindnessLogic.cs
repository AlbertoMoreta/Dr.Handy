using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DrHandy.Logic {
    class ColorBlindnessLogic  {

        public static readonly int TOTAL_QUESTIONS = 24;
        public static readonly int ANSWERS_COUNT = 4;

        public int CurrentQuestion { get; set; } = 0;
        public List<Model.ColorBlindnessQuestion> Questions { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int RGColorBlindnessCount { get; set; }
        public int TotalColorBlindnessCount { get; set; }

        private static ColorBlindnessLogic _instance;

        public static ColorBlindnessLogic Instance() {
            if (_instance == null)  {
                _instance = new ColorBlindnessLogic();
            }

            return _instance;
        }


        public List<Model.ColorBlindnessQuestion> GetQuestions()  {

            if (Questions == null) {
                string jsonFile;
                string fileName = "JSON/ColorBlindnessTest.json";
#if __IOS__
                jsonFile = File.ReadAllText(fileName);
#elif __ANDROID__
                var context = Android.App.Application.Context;
                StreamReader stream = new StreamReader(context.Assets.Open(fileName));
                jsonFile = stream.ReadToEnd();
                stream.Close();

#endif
                Questions = JsonConvert.DeserializeObject<List<Model.ColorBlindnessQuestion>>(jsonFile);
            }

            return Questions;;
        }

        public void SubmitAnswer(string answer){
            if(CurrentQuestion < TOTAL_QUESTIONS) {
                var question = Questions.ElementAt(CurrentQuestion);
                question.UserAnswer = answer;
                var correctAnswer = question.Answers.ElementAt(question.CorrectAnswerPos);
                var RGColorBlindnessAnswer = question.Answers.ElementAt(question.RGColorBlindnessPos);

                if (answer.Equals(correctAnswer)) { CorrectAnswersCount++; }
                else if (answer.Equals(RGColorBlindnessAnswer)) { RGColorBlindnessCount++;}
                else { TotalColorBlindnessCount++; }
            }
        } 
    }
}
