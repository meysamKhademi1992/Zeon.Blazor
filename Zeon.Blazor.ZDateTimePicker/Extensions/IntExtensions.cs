using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Extensions
{
    public static class IntExtensions
    {
        public static int GetYear(this int year, DatePickerType type)
        {
            switch (type)
            {
                case DatePickerType.Jalali:
                    {
                        int jalaliYear = 0;

                        jalaliYear = year - 621;

                        return jalaliYear;
                    }
                case DatePickerType.Gregorian:
                    {
                        return year;
                    }
                default:
                    return year;
            }
        }
        public static string GetWeekChar(this int dayOfWeek, DatePickerType type)
        {
            switch (type)
            {
                case DatePickerType.Jalali:
                    {
                        return dayOfWeek == 0 ? "ش" : dayOfWeek == 1 ? "ی" : dayOfWeek == 2 ? "د" : dayOfWeek == 3 ? "س" : dayOfWeek == 4 ? "چ" : dayOfWeek == 5 ? "پ" : dayOfWeek == 6 ? "ج" : dayOfWeek.ToString();
                    }
                case DatePickerType.Gregorian:
                    {
                        return dayOfWeek == 0 ? "Sa" : dayOfWeek == 1 ? "Su" : dayOfWeek == 2 ? "Mo" : dayOfWeek == 3 ? "Tu" : dayOfWeek == 4 ? "We" : dayOfWeek == 5 ? "Th" : dayOfWeek == 6 ? "Fr" : dayOfWeek.ToString();
                    }
                default:
                    return dayOfWeek.ToString();
            }
        }

    }
}
