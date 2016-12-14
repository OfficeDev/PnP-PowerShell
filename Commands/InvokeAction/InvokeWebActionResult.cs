using System;
using System.Data;

namespace SharePointPnP.PowerShell.Commands.InvokeAction
{
    public class InvokeWebActionResult
    {
        public int TotalWebCount { get; internal set; }
        public int ProcessedWebCount { get; internal set; }
        public int? ProcessedPostWebCount { get; internal set; }

        public int TotalListCount { get; internal set; }
        public int ProcessedListCount { get; internal set; }
        public int? ProcessedPostListCount { get; internal set; }

        public int TotalListItemCount { get; internal set; }
        public int ProcessedListItemCount { get; internal set; }

        public bool IsListNameSpecified { get; internal set; }

        public double? AverageWebTime { get; internal set; }
        public double? AverageListTime { get; internal set; }
        public double? AverageListItemTime { get; internal set; }

        public double? AveragePostWebTime { get; internal set; }
        public double? AveragePostListTime { get; internal set; }

        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }

        public TimeSpan TotalExecutionTime => EndDate - StartDate;

        public DataTable ToDataTable()
        {
            DataTable resultTable = new DataTable("InvokeWebActionResult");
            DataColumn titleColumn = new DataColumn("Title", typeof(string));
            DataColumn valueColumn = new DataColumn("Value", typeof(string));

            resultTable.Columns.Add(titleColumn);
            resultTable.Columns.Add(valueColumn);

            if (!IsListNameSpecified)
            {
                AddTableRow(resultTable, "Number of webs", TotalWebCount);

                if (ProcessedWebCount != 0)
                    AddTableRow(resultTable, "Number of webs proccessed", ProcessedWebCount);

                if (ProcessedPostWebCount.HasValue)
                    AddTableRow(resultTable, "Number of webs post proccessed", ProcessedPostWebCount);

                if (ProcessedListCount != 0 || ProcessedPostListCount.HasValue || ProcessedListItemCount != 0)
                {
                    AddTableRow(resultTable, "Number of lists", TotalListCount);

                    if (ProcessedListCount != 0)
                        AddTableRow(resultTable, "Number of lists proccessed", ProcessedListCount);

                    if(ProcessedPostListCount.HasValue)
                        AddTableRow(resultTable, "Number of lists post proccessed", ProcessedPostListCount);
                }
            }

            if (ProcessedListItemCount != 0 || ProcessedPostListCount.HasValue)
            {
                AddTableRow(resultTable, "Number of list items", TotalListItemCount);

                if (ProcessedListItemCount != 0)
                    AddTableRow(resultTable, "Number of list items proccessed", ProcessedListItemCount);
            }

            if (!IsListNameSpecified)
            {
                if (AverageWebTime.HasValue)
                {
                    TimeSpan averageWebTimeSpan = TimeSpan.FromSeconds(AverageWebTime.Value);
                    AddTableRow(resultTable, "Average web action time", GetTimeSpanText(averageWebTimeSpan));
                }

                if (AveragePostWebTime.HasValue)
                {
                    TimeSpan averagePostWebTimeSpan = TimeSpan.FromSeconds(AveragePostWebTime.Value);
                    AddTableRow(resultTable, "Average web post action time", GetTimeSpanText(averagePostWebTimeSpan));
                }
            }

            if (ProcessedListCount != 0 || ProcessedListItemCount != 0)
            {
                if (AverageListTime.HasValue)
                {
                    TimeSpan averageListTimeSpan = TimeSpan.FromSeconds(AverageListTime.Value);
                    AddTableRow(resultTable, "Average list action time", GetTimeSpanText(averageListTimeSpan));
                }

                if (AveragePostListTime.HasValue)
                {
                    TimeSpan averagePostListTimeSpan = TimeSpan.FromSeconds(AveragePostListTime.Value);
                    AddTableRow(resultTable, "Average list action time", GetTimeSpanText(averagePostListTimeSpan));
                }
            }

            if (AverageListItemTime.HasValue)
            {
                TimeSpan averageListItemTimeSpan = TimeSpan.FromSeconds(AverageListItemTime.Value);
                AddTableRow(resultTable, "Average list item action time", GetTimeSpanText(averageListItemTimeSpan));
            }

            AddTableRow(resultTable, "Start date", StartDate);
            AddTableRow(resultTable, "End date", EndDate);

            AddTableRow(resultTable, "Total execution time", GetTimeSpanText(TotalExecutionTime));

            return resultTable;
        }

        private string GetTimeSpanText(TimeSpan timeSpan)
        {
            var text = string.Empty;

            if (timeSpan.Days > 0)
                text += $"{timeSpan.Days} days";

            text += timeSpan.ToString("hh\\:mm\\:ss\\.ff");

            return text;
        }

        private static void AddTableRow(DataTable table, string title, object value)
        {
            var row = table.NewRow();

            row["Title"] = title;
            row["Value"] = value?.ToString();

            table.Rows.Add(row);
        }
    }
}
