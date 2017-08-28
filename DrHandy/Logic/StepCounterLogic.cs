using System;
using System.Collections.Generic;
using System.Text;

namespace DrHandy.Logic {
    class StepCounterLogic {

        private static StepCounterLogic _instance;

        public static StepCounterLogic Instance() {
            if (_instance == null) {
                _instance = new StepCounterLogic();
            }

            return _instance;
        }

        public int GetCaloriesFromSteps(int steps) {
            return steps/20;
        }

        public double GetDistanceFromSteps(int steps) {
            return Math.Round((steps*0.00075), 2);
        }

    }
}
