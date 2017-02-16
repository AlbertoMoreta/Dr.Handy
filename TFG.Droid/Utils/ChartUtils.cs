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

        public static List<BarEntry> StepCounter_StepsToBarEntries(List<StepCounterItem> items) {

            List<BarEntry> entries = new List<BarEntry>(); 

            for (int i = 0; i < items.Count; i++) {
                entries.Add(new BarEntry(i, items.ElementAt(i).Steps));
            }

            return entries; 
        }

        public static List<BarEntry> StepCounter_CaloriesToBarEntries(List<StepCounterItem> items) {

            List<BarEntry> entries = new List<BarEntry>(); 

            for (int i = 0; i < items.Count; i++) {
                entries.Add(new BarEntry(i, items.ElementAt(i).Calories));
            }

            return entries;
        }

        public static List<BarEntry> StepCounter_DistanceToBarEntries(List<StepCounterItem> items) {

            List<BarEntry> entries = new List<BarEntry>(); 

            for (int i = 0; i < items.Count; i++) {
                entries.Add(new BarEntry(i, (float) items.ElementAt(i).Distance));
            }

            return entries;
        }

        public static List<StepCounterItem> GetStepCounterItemsFromMetric(VisualizationMetric metric, DateTime date) {

            var startDate = new DateTime();
            var endDate = new DateTime();
            switch (metric) {
                case VisualizationMetric.Weekly:
                    startDate = date.AddDays(0 - (int) date.DayOfWeek);
                    endDate = date.AddDays(6 - (int) date.DayOfWeek);
                    break;
                case VisualizationMetric.Yearly:
                    startDate = new DateTime(date.Year, 0, 0);
                    endDate = new DateTime(date.Year, 11, 31);
                    break;
            }

            return DBHelper.Instance.GetStepCounterItemsFromDateRange(startDate, endDate);
        }


    }
}