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

        public static List<BarEntry> StepCounter_StepsToBarEntries(VisualizationMetric metric, DateTime date) {

            List<BarEntry> entries = new List<BarEntry>();

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime(); 
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
            List<StepCounterItem> items = DBHelper.Instance.GetStepCounterItemsFromDateRange(startDate, endDate); 
            for(int i = 0; i < items.Count; i++) {
                entries.Add(new BarEntry(items.ElementAt(i).Steps, i));
            }

            return entries; 
        }

        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo) {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static void PopulateChart(this BarChart chart, List<BarEntry> values, VisualizationMetric metric)  { 

            if (chart != null) {
                chart.Data = new BarData(new BarDataSet(values, ""));
                chart.AnimateY(700);
                chart.NotifyDataSetChanged();
                chart.Invalidate();
            }

            var label = "";
            switch (metric) {
                case VisualizationMetric.Weekly: label = "weekday"; break; 
                case VisualizationMetric.Yearly: label = "month"; break;
            }  

        } 


    }
}