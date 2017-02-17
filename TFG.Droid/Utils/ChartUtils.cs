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

        public static List<BarEntry> StepCounter_StepsToBarEntries(List<StepCounterItem> items, int labelsCount = -1) {

            List<BarEntry> entries = new List<BarEntry>();

            if(labelsCount == -1) { labelsCount = items.Count; } 

            for (int i = 0; i < labelsCount; i++) {
                if (i < items.Count) {
                    entries.Add(new BarEntry(i, items.ElementAt(i).Steps));
                } else { 
                    entries.Add(new BarEntry(i, null));
                }
            }

            return entries; 
        }

        public static List<BarEntry> StepCounter_CaloriesToBarEntries(List<StepCounterItem> items, int labelsCount = -1) {

            List<BarEntry> entries = new List<BarEntry>();

            if (labelsCount == -1) { labelsCount = items.Count; }

            for (int i = 0; i < labelsCount; i++) {
                if (i < items.Count) {
                    entries.Add(new BarEntry(i, items.ElementAt(i).Calories));
                } else {
                    entries.Add(new BarEntry(i, null));
                }
            }

            return entries;
        }

        public static List<BarEntry> StepCounter_DistanceToBarEntries(List<StepCounterItem> items, int labelsCount = -1) {

            List<BarEntry> entries = new List<BarEntry>();

            if (labelsCount == -1) { labelsCount = items.Count; }

            for (int i = 0; i < labelsCount; i++) {
                if (i < items.Count) {
                    entries.Add(new BarEntry(i, (float) items.ElementAt(i).Distance));
                } else {
                    entries.Add(new BarEntry(i, null));
                }
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