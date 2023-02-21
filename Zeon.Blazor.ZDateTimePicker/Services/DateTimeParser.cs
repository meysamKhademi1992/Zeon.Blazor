using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;
using Zeon.Blazor.ZDateTimePicker.Extensions;

namespace Zeon.Blazor.ZDateTimePicker.Services
{
    public class DateTimeParser : IDateTimeParser
    {
        private readonly Dictionary<DatePickerType, DatePickerTypeDelegate> _datePickerTypes;
        private readonly Dictionary<InputType, InputEventHandlerDelegate> _jalaliInputTypes;
        private readonly Dictionary<InputType, InputEventHandlerDelegate> _gregorianInputTypes;
        public DateTimeParser()
        {
            _datePickerTypes = new(); _jalaliInputTypes = new(); _gregorianInputTypes = new();

            _datePickerTypes.Add(DatePickerType.Jalali, DatePickerJalaliType);
            _datePickerTypes.Add(DatePickerType.Gregorian, DatePickerGregorianType);

            _jalaliInputTypes.Add(InputType.DateTime, JalaliDateTimeInput);
            _jalaliInputTypes.Add(InputType.Date, JalaliDateInput);
            _jalaliInputTypes.Add(InputType.TimeSpan, JalaliDateTimeInput);

            _gregorianInputTypes.Add(InputType.DateTime, GregorianDateTimeInput);
            _gregorianInputTypes.Add(InputType.Date, GregorianDateInput);
            _gregorianInputTypes.Add(InputType.TimeSpan, GregorianDateTimeInput);
        }

        public Task<(DateTime dateTime, TimeSpan timeSpan, bool isValid)> Parse(DatePickerType datePickerType, string value, InputType inputType)
        {
            return _datePickerTypes.Single(q => q.Key == datePickerType).Value.Invoke(value, inputType);
        }

        private delegate Task<(DateTime, TimeSpan, bool)> InputEventHandlerDelegate(string value, string dateSpliter);
        private delegate Task<(DateTime, TimeSpan, bool)> DatePickerTypeDelegate(string value, InputType inputType);

        private Task<(DateTime, TimeSpan, bool)> DatePickerGregorianType(string value, InputType inputType)
        {
            return _gregorianInputTypes.Single(q => q.Key == inputType).Value.Invoke(value);
        }

        private Task<(DateTime, TimeSpan, bool)> DatePickerJalaliType(string value, string dateSpliter, InputType inputType)
        {
            return _jalaliInputTypes.Single(q => q.Key == inputType).Value.Invoke(value);
        }

        private Task<(DateTime, TimeSpan, bool)> JalaliDateInput(string value, string dateSpliter)
        {
            throw new NotImplementedException();
        }

        private Task<(DateTime, TimeSpan, bool isValid)> JalaliDateTimeInput(string value, string dateSpliter)
        {
            var dateTime = value.JalaliToGregorian(DateType.DateTime, dateSpliter, ref isValid);
            var time = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
            return 
        }
        private Task<(DateTime, TimeSpan, bool)> GregorianDateInput(string value, string dateSpliter)
        {
            throw new NotImplementedException();
        }

        private Task<(DateTime, TimeSpan, bool)> GregorianDateTimeInput(string value, string dateSpliter)
        {
            throw new NotImplementedException();
        }

    }
}
