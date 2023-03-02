using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeon.Blazor.ZDateTimePicker.Abstractions
{
    public abstract class DatePicker
    {
        public abstract int CreateNumberOfYears { get; set; }
        public abstract string Direction { get; }
        public abstract string NextMonthText { get; }
        public abstract string PrevMonthText { get; }
        public abstract string OkText { get; }
        public abstract string TodayText { get; }
        public abstract string YearText { get; }
        public abstract string MonthText { get; }
        public abstract string HourText { get; }
        public abstract string MinuteText { get; }
        public abstract Task<string> Convert(DateTime dateTime, string format);
        public abstract Task<(DateTime dateTime, bool isValid)> Convert(string dateTime);
        public abstract DateTime GetFirstDayOfMonth(DateTime dateTime);
        public abstract DateTime GetNextMonth(DateTime dateTime);
        public abstract DateTime GetPrevMonth(DateTime dateTime);
        public abstract DateTime ChangeMonth(DateTime dateTime, int month);
        public abstract DateTime ChangeYear(DateTime dateTime, int year);
        public abstract DateTime ChangeHour(DateTime dateTime, int hour);
        public abstract DateTime ChangeMinute(DateTime dateTime, int minute);
        public abstract int GetCountDayOfMonth(DateTime dateTime);
        public abstract int GetYear(DateTime dateTime);
        public abstract int GetYearItem(int index);
        public abstract string GetYearDisplayItem(int year);
        public abstract string GetMonthDisplayItem(int month);
        public abstract string GetDayDisplayItem(int day);
        public abstract string GetHourDisplayItem(int hour);
        public abstract string GetMinuteDisplayItem(int minute);
        public abstract int GetMonth(DateTime dateTime);
        public abstract string GetMonthName(DateTime dateTime);
        public abstract string GetWeekChar(int dayOfWeek);

    }
}
