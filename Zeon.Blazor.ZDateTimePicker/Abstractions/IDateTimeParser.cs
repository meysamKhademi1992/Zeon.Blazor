using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Abstractions
{
    public interface IDateTimeParser
    {
        public Task<(DateTime dateTime, TimeSpan timeSpan, bool isValid)> Parse(DatePickerType datePickerType, string value, char dateSpliter, InputType inputType);
    }
}
