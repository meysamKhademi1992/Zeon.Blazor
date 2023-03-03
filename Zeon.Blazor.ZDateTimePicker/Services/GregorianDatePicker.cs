using System.Globalization;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Services
{
    public class GregorianDatePicker : DatePicker
    {
        private const string DIRECTION = "ltr";
        private const string NEXT_MONTH_TEXT = "Next Month";
        private const string PREV_MONTH_TEXT = "Prev Month";
        private const string TODAY_TEXT = "Today";
        private const string OK_TEXT = "Ok";
        private const string YEAR_TEXT = "Year";
        private const string MONTH_TEXT = "Month";
        private const string HOUR_TEXT = "Hour";
        private const string MINUTE_TEXT = "Minute";

        public GregorianDatePicker(int createNumberOfYears)
        {
            CreateNumberOfYears = createNumberOfYears;
        }

        internal override int CreateNumberOfYears { get; set; }
        internal override string Direction { get => DIRECTION; }
        internal override string NextMonthText { get => NEXT_MONTH_TEXT; }
        internal override string PrevMonthText { get => PREV_MONTH_TEXT; }
        internal override string OkText { get => OK_TEXT; }
        internal override string TodayText { get => TODAY_TEXT; }
        internal override string YearText { get => YEAR_TEXT; }
        internal override string MonthText { get => MONTH_TEXT; }
        internal override string HourText { get => HOUR_TEXT; }
        internal override string MinuteText { get => MINUTE_TEXT; }

        internal override Task<string> Convert(DateTime dateTime, string format)
        {
            return Task.FromResult(dateTime.ToString(format));
        }

        internal override Task<(DateTime dateTime, bool isValid)> Convert(string dateTime)
        {
            var isValid = DateTime.TryParse(dateTime, out var dateTimeResult);
            return Task.FromResult((dateTimeResult, isValid));
        }

        internal override string GetMonthName(DateTime dateTime)
        {
            string displayName = dateTime.ToString("MMM", CultureInfo.InvariantCulture);
            return displayName;
        }

        internal override DateTime GetPrevMonth(DateTime dateTime)
        {
            return dateTime.AddMonths(-1);
        }

        internal override DateTime GetNextMonth(DateTime dateTime)
        {
            return dateTime.AddMonths(1);
        }

        internal override DateTime ChangeYear(DateTime dateTime, int year)
        {
            return new DateTime(year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        internal override DateTime ChangeMonth(DateTime dateTime, int month)
        {
            return new DateTime(dateTime.Year, month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        internal override int GetCountDayOfMonth(DateTime dateTime)
        {
            var isLeapYear = DateTime.IsLeapYear(dateTime.Year);
            var count = dateTime.Month >= 1 && dateTime.Month <= 6 ? 31 : dateTime.Month >= 7 && dateTime.Month <= 11 ? 30 : dateTime.Month == 12 && !isLeapYear ? 28 : dateTime.Month == 12 && isLeapYear ? 29 : 0;
            return count;
        }

        internal override int GetYear(DateTime dateTime)
        {
            return dateTime.Year;
        }

        internal override int GetMonth(DateTime dateTime)
        {
            return dateTime.Month;
        }

        internal override DateTime ChangeHour(DateTime dateTime, int hour)
        {
            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
            return newDateTime;
        }

        internal override DateTime ChangeMinute(DateTime dateTime, int minute)
        {
            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
            return newDateTime;
        }

        internal override int GetYearItem(int index)
        {
            return DateTime.Now.Year - CreateNumberOfYears / 2 + index;
        }

        internal override string GetYearDisplayItem(int year)
        {
            return year.ToString();
        }

        internal override string GetMonthDisplayItem(int month)
        {
            return month.ToString();
        }

        internal override string GetDayDisplayItem(int day)
        {
            return day.ToString();
        }

        internal override string GetHourDisplayItem(int hour)
        {
            return hour.ToString();
        }

        internal override string GetMinuteDisplayItem(int minute)
        {
            return minute.ToString();
        }
        internal override string GetWeekChar(int dayOfWeek)
        {
            return dayOfWeek == 0 ? "Sa" : dayOfWeek == 1 ? "Su" : dayOfWeek == 2 ? "Mo" : dayOfWeek == 3 ? "Tu" : dayOfWeek == 4 ? "We" : dayOfWeek == 5 ? "Th" : dayOfWeek == 6 ? "Fr" : dayOfWeek.ToString();
        }

        internal override DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        internal override DateTime GetResult(DateTime dateTime, InputType inputPickerType)
        {
            return inputPickerType switch
            {
                InputType.DateTime => dateTime,
                InputType.Date => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day),
                InputType.TimeSpan => new DateTime(1, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second),
                _ => dateTime,
            };
        }
    }
}
