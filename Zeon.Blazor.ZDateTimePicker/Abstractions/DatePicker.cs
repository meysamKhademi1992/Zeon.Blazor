using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Abstractions
{
    public abstract class DatePicker
    {
        internal abstract string Direction { get; }
        internal abstract string NextMonthText { get; }
        internal abstract string PrevMonthText { get; }
        internal abstract string OkText { get; }
        internal abstract string TodayText { get; }
        internal abstract string YearText { get; }
        internal abstract string MonthText { get; }
        internal abstract string HourText { get; }
        internal abstract string MinuteText { get; }
        internal abstract Task<string> Convert(DateTime dateTime, string format);
        internal abstract Task<(DateTime dateTime, bool isValid)> Convert(string dateTime);
        internal abstract DateTime GetResult(DateTime dateTime, InputType inputPickerType);
        internal abstract DateTime GetFirstDayOfMonth(DateTime dateTime);
        internal abstract DateTime GetNextMonth(DateTime dateTime);
        internal abstract DateTime GetPrevMonth(DateTime dateTime);
        internal abstract DateTime ChangeMonth(DateTime dateTime, int month);
        internal abstract DateTime ChangeYear(DateTime dateTime, int year);
        internal abstract DateTime ChangeHour(DateTime dateTime, int hour);
        internal abstract DateTime ChangeMinute(DateTime dateTime, int minute);
        internal abstract int GetCountDayOfMonth(DateTime dateTime);
        internal abstract int GetYear(DateTime dateTime);
        internal abstract int GetYearItem(int createNumberOfYears, int index);
        internal abstract string GetYearDisplayItem(int year);
        internal abstract string GetMonthDisplayItem(int month);
        internal abstract string GetDayDisplayItem(int day);
        internal abstract string GetHourDisplayItem(int hour);
        internal abstract string GetMinuteDisplayItem(int minute);
        internal abstract int GetMonth(DateTime dateTime);
        internal abstract string GetMonthName(DateTime dateTime);
        internal abstract string GetWeekChar(int dayOfWeek);
    }
}
