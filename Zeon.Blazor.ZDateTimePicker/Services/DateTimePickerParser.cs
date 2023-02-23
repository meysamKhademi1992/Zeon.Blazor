using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;
using Zeon.Blazor.ZDateTimePicker.Extensions;

namespace Zeon.Blazor.ZDateTimePicker.Services
{
    public class DateTimePickerParserService : IDateTimePickerParser
    {
        private readonly Dictionary<DatePickerType, DatePickerTypeDelegate> _datePickerTypes;
        private readonly Dictionary<InputType, InputEventHandlerDelegate> _jalaliInputTypes;
        private readonly Dictionary<InputType, InputEventHandlerDelegate> _gregorianInputTypes;
        public DateTimePickerParserService()
        {
            _datePickerTypes = new(); _jalaliInputTypes = new(); _gregorianInputTypes = new();

            _datePickerTypes.Add(DatePickerType.Jalali, DatePickerJalaliType);
            _datePickerTypes.Add(DatePickerType.Gregorian, DatePickerGregorianType);

            _jalaliInputTypes.Add(InputType.DateTime, JalaliDateTimeInput);
            _jalaliInputTypes.Add(InputType.Date, JalaliDateInput);
            _jalaliInputTypes.Add(InputType.TimeSpan, JalaliTimeInput);

            _gregorianInputTypes.Add(InputType.DateTime, GregorianDateTimeInput);
            _gregorianInputTypes.Add(InputType.Date, GregorianDateInput);
            _gregorianInputTypes.Add(InputType.TimeSpan, GregorianTimeInput);
        }

        public Task<(DateTime dateTime, TimeSpan timeSpan, bool isValid)> Parse(DatePickerType datePickerType, string value, char dateSpliter, InputType inputType)
        {
            return _datePickerTypes.Single(q => q.Key == datePickerType).Value.Invoke(value, dateSpliter, inputType);
        }

        private delegate Task<(DateTime, TimeSpan, bool)> InputEventHandlerDelegate(string value, char dateSpliter);
        private delegate Task<(DateTime, TimeSpan, bool)> DatePickerTypeDelegate(string value, char dateSpliter, InputType inputType);

        private Task<(DateTime, TimeSpan, bool)> DatePickerGregorianType(string value, char dateSpliter, InputType inputType)
        {
            return _gregorianInputTypes.Single(q => q.Key == inputType).Value.Invoke(value, dateSpliter);
        }

        private Task<(DateTime, TimeSpan, bool)> DatePickerJalaliType(string value, char dateSpliter, InputType inputType)
        {
            return _jalaliInputTypes.Single(q => q.Key == inputType).Value.Invoke(value, dateSpliter);
        }

        private Task<(DateTime, TimeSpan, bool)> JalaliTimeInput(string value, char dateSpliter)
        {
            bool isValid = false;
            var dateTime = new DateTime();
            var time = value.TimeParse(ref isValid);
            return Task.FromResult((dateTime, time, isValid));
        }
        private Task<(DateTime, TimeSpan, bool)> JalaliDateInput(string value, char dateSpliter)
        {
            bool isValid = false;
            var dateTime = value.JalaliToGregorian(DateType.Date, dateSpliter, ref isValid);
            var time = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
            return Task.FromResult((dateTime, time, isValid));
        }

        private Task<(DateTime, TimeSpan, bool isValid)> JalaliDateTimeInput(string value, char dateSpliter)
        {
            bool isValid = false;
            var dateTime = value.JalaliToGregorian(DateType.DateTime, dateSpliter, ref isValid);
            var time = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
            return Task.FromResult((dateTime, time, isValid));
        }
        private Task<(DateTime, TimeSpan, bool)> GregorianTimeInput(string value, char dateSpliter)
        {
            bool isValid = false;
            var dateTime = new DateTime();
            var time = value.TimeParse(ref isValid);
            return Task.FromResult((dateTime, time, isValid));
        }
        private Task<(DateTime, TimeSpan, bool)> GregorianDateInput(string value, char dateSpliter)
        {
            bool isValid = false;
            var dateTime = value.ParseGregorian(DateType.Date, ref isValid);
            var time = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
            return Task.FromResult((dateTime, time, isValid));
        }

        private Task<(DateTime, TimeSpan, bool)> GregorianDateTimeInput(string value, char dateSpliter)
        {
            bool isValid = false;
            var dateTime = value.ParseGregorian(DateType.DateTime, ref isValid);
            var time = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
            return Task.FromResult((dateTime, time, isValid));
        }

    }
}
