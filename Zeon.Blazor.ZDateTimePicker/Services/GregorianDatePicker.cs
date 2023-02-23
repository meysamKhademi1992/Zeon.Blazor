using System.Globalization;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;
using Zeon.Blazor.ZDateTimePicker.Extensions;

namespace Zeon.Blazor.ZDateTimePicker.Services
{
    public class GregorianDatePicker : DatePicker
    {


        public GregorianDatePicker(int createNumberOfYears)
        {
            CreateNumberOfYears = createNumberOfYears;
        }

        public override int CreateNumberOfYears { get; set; }

        public override Task<string> Convert(DateTime dateTime, string format)
        {
            return Task.FromResult(dateTime.ToString(format));
        }

        public override Task<(DateTime dateTime, bool isValid)> Convert(string dateTime)
        {
            var isValid = DateTime.TryParse(dateTime, out var dateTimeResult);
            return Task.FromResult((dateTimeResult, isValid));
        }

        public override string GetMonthName(DateTime dateTime)
        {
            string displayName = dateTime.ToString("MMM", CultureInfo.InvariantCulture);
            return displayName;
        }

        public override DateTime GetPrevMonth(DateTime dateTime)
        {
            return dateTime.AddMonths(-1);
        }

        public override DateTime GetNextMonth(DateTime dateTime)
        {
            return dateTime.AddMonths(1);
        }

        public override DateTime ChangeYear(DateTime dateTime, int year)
        {
            return new DateTime(year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public override DateTime ChangeMonth(DateTime dateTime, int month)
        {
            return new DateTime(dateTime.Year, month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public override int GetCountDayOfMonth(DateTime dateTime)
        {
            var isLeapYear = DateTime.IsLeapYear(dateTime.Year);
            var count = dateTime.Month >= 1 && dateTime.Month <= 6 ? 31 : dateTime.Month >= 7 && dateTime.Month <= 11 ? 30 : dateTime.Month == 12 && !isLeapYear ? 28 : dateTime.Month == 12 && isLeapYear ? 29 : 0;
            return count;
        }

        public override int GetYear(DateTime dateTime)
        {
            return dateTime.Year;
        }

        public override int GetMonth(DateTime dateTime)
        {
            return dateTime.Month;
        }

        public override DateTime ChangeHour(DateTime dateTime, int hour)
        {
            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
            return newDateTime;
        }

        public override DateTime ChangeMinute(DateTime dateTime, int minute)
        {
            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
            return newDateTime;
        }

        public override int GetYearItem(int index)
        {
            return DateTime.Now.Year - CreateNumberOfYears + index;
        }

        public override string GetWeekChar(int dayOfWeek)
        {
            return dayOfWeek == 0 ? "Sa" : dayOfWeek == 1 ? "Su" : dayOfWeek == 2 ? "Mo" : dayOfWeek == 3 ? "Tu" : dayOfWeek == 4 ? "We" : dayOfWeek == 5 ? "Th" : dayOfWeek == 6 ? "Fr" : dayOfWeek.ToString();
        }
    }
}
