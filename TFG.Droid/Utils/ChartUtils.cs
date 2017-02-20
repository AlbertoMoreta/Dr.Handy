using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using TFG.Model; 

namespace TFG.Droid.Utils {
    public static class ChartUtils {

        public enum VisualizationMetric {
            Weekly, Yearly
        }

        public static List<BarEntry> StepCounter_StepsToBarEntries(List<StepCounterItem> items, VisualizationMetric metric, int labelsCount = -1) {

            List<BarEntry> entries = new List<BarEntry>();

            if(labelsCount == -1) { labelsCount = items.Count; }

            var count = 0; //Iterate over StepCounterItems

            for (int i = 0; i < labelsCount; i++) {
                var steps = -1;

                if (count < items.Count) {
                    var item = items.ElementAt(count);

                    var currentDate = metric == VisualizationMetric.Weekly
                        ? (int) item.Date.DayOfWeek == i
                        : (item.Date.Month) - 1 == i;

                    if (currentDate) {
                        steps = items.ElementAt(count).Steps; 
                        count++;
                    } 
                }  

                entries.Add(steps != -1 ? new BarEntry(i, steps) : new BarEntry(i, null));
            }

            return entries; 
        }

        public static List<BarEntry> StepCounter_CaloriesToBarEntries(List<StepCounterItem> items, VisualizationMetric metric, int labelsCount = -1) {

            List<BarEntry> entries = new List<BarEntry>();

            if (labelsCount == -1) { labelsCount = items.Count; }

            var count = 0; //Iterate over StepCounterItems

            for (int i = 0; i < labelsCount; i++) {
                var calories = -1;

                if (count < items.Count) {
                    var item = items.ElementAt(count);

                    var currentDate = metric == VisualizationMetric.Weekly
                        ? (int) item.Date.DayOfWeek == i
                        : (item.Date.Month) - 1 == i;

                    if (currentDate) {
                        calories = items.ElementAt(count).Calories;
                        count++;
                    }
                }


                entries.Add(calories != -1 ? new BarEntry(i, calories) : new BarEntry(i, null));
            }

            return entries;
        }

        public static List<BarEntry> StepCounter_DistanceToBarEntries(List<StepCounterItem> items, VisualizationMetric metric, int labelsCount = -1) {

            List<BarEntry> entries = new List<BarEntry>();

            if (labelsCount == -1) { labelsCount = items.Count; }

            var count = 0; //Iterate over StepCounterItems

            for (int i = 0; i < labelsCount; i++) { 
                var distance = -1.0;

                if (count < items.Count) {
                    var item = items.ElementAt(count);

                    var currentDate = metric == VisualizationMetric.Weekly
                        ? (int) item.Date.DayOfWeek == i
                        : (item.Date.Month) - 1 == i;

                    if (currentDate) {
                        distance = items.ElementAt(count).Distance;
                        count++;
                    } 
                }

                entries.Add(distance != -1.0 ? new BarEntry(i, (float) distance) : new BarEntry(i, null));
            }

            return entries;
        }

        public static List<StepCounterItem> GetStepCounterItemsFromMetric(VisualizationMetric metric, DateTime date) {
            List<StepCounterItem> items = null;

            var startDate = new DateTime();
            var endDate = new DateTime();
            switch (metric) {
                case VisualizationMetric.Weekly:
                    startDate = date.AddDays(0 - (int) date.DayOfWeek);
                    endDate = date.AddDays(6 - (int) date.DayOfWeek);
                    items = DBHelper.Instance.GetStepCounterItemsFromDateRange(startDate, endDate);
                    break;
                case VisualizationMetric.Yearly:
                    items = DBHelper.Instance.GetStepCounterItemsMonthly(date.Year.ToString());
                    break;
            } 

            return items;
        }


    }
}