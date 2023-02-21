

using System.Globalization;
using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ChangeYear(this DateTime currentDate, int newYear, DatePickerType type)
    {
        DateTime date = new DateTime();
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (currentDate > new DateTime().AddYears(623))
                    {
                        PersianCalendar pc = new PersianCalendar();
                        var MM = pc.GetMonth(currentDate);
                        var dd = pc.GetDayOfMonth(currentDate);
                        date = pc.ToDateTime(newYear, MM, dd, currentDate.Hour, currentDate.Minute, currentDate.Second, currentDate.Millisecond);
                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    date = new DateTime(newYear, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
                    break;
                }
        }
        return date;

    }
    public static DateTime ChangeMonth(this DateTime currentDate, int newMonth, DatePickerType type)
    {
        DateTime date = new DateTime();
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (currentDate > new DateTime().AddYears(623))
                    {
                        PersianCalendar pc = new PersianCalendar();
                        var yyyy = pc.GetYear(currentDate);
                        var MM = pc.GetMonth(currentDate);
                        var dd = pc.GetDayOfMonth(currentDate);
                        date = pc.ToDateTime(yyyy, newMonth, dd, currentDate.Hour, currentDate.Minute, currentDate.Second, currentDate.Millisecond);
                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    date = new DateTime(currentDate.Year, newMonth, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
                    break;
                }
        }
        return date;

    }
    public static DateTime ChangeHour(this DateTime currentDate, int newHour)
    {
        DateTime date = new DateTime();
        date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, newHour, currentDate.Minute, currentDate.Second);
        return date;

    }
    public static DateTime ChangeMinute(this DateTime currentDate, int newMinute)
    {
        DateTime date = new DateTime();
        date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, newMinute, currentDate.Second);
        return date;

    }
    public static DateTime GetFirstDayOfMonth(this DateTime value, DatePickerType type)
    {
        DateTime date = new DateTime();
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (value > new DateTime().AddYears(623))
                    {
                        int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0;

                        PersianCalendar pc = new PersianCalendar();
                        yyyy = pc.GetYear(value);
                        MM = pc.GetMonth(value);
                        dd = pc.GetDayOfMonth(value);
                        HH = pc.GetHour(value);
                        mm = pc.GetMinute(value);
                        date = pc.ToDateTime(yyyy, MM, 1, HH, mm, 0, 0);
                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    date = new DateTime(value.Year, value.Month, 1, value.Hour, value.Minute, value.Second);
                    break;
                }
        }
        return date;

    }
    public static DateTime GetNextMonth(this DateTime value, DatePickerType type)
    {
        DateTime nextDate = new DateTime();
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (value > new DateTime().AddYears(623))
                    {
                        int yyyy = 0, MM = 0, dd = 0;

                        PersianCalendar pc = new PersianCalendar();
                        yyyy = pc.GetMonth(value) == 12 ? (pc.GetYear(value) + 1) : pc.GetYear(value);
                        MM = pc.GetMonth(value) == 12 ? 1 : (pc.GetMonth(value) + 1);

                        bool isLeapYear = false;
                        if (MM == 12)
                        {
                            try
                            {
                                pc.ToDateTime(yyyy, 12, 30, 0, 0, 0, 0);
                                isLeapYear = true;
                            }
                            catch
                            {
                                isLeapYear = false;
                            }
                        }
                        var monthCount = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;

                        dd = monthCount >= (pc.GetDayOfMonth(value)) ? pc.GetDayOfMonth(value) : monthCount == 30 ? 30 : 29;
                        nextDate = pc.ToDateTime(yyyy, MM, dd, value.Hour, value.Minute, 0, 0);
                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    var isLeapYear = DateTime.IsLeapYear(value.Year);

                    nextDate = value.AddMonths(1);
                    break;
                }
        }
        return nextDate;

    }
    public static DateTime GetPrevMonth(this DateTime value, DatePickerType type)
    {

        DateTime nextDate = new DateTime();
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (value > new DateTime().AddYears(623))
                    {
                        int yyyy = 0, MM = 0, dd = 0;

                        PersianCalendar pc = new PersianCalendar();
                        yyyy = pc.GetMonth(value) == 1 ? (pc.GetYear(value) - 1) : pc.GetYear(value);
                        MM = pc.GetMonth(value) == 1 ? 12 : (pc.GetMonth(value) - 1);

                        bool isLeapYear = false;
                        if (MM == 12)
                        {
                            try
                            {
                                pc.ToDateTime(yyyy, 12, 30, 0, 0, 0, 0);
                                isLeapYear = true;
                            }
                            catch
                            {
                                isLeapYear = false;
                            }
                        }
                        var monthCount = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;

                        dd = monthCount >= (pc.GetDayOfMonth(value)) ? pc.GetDayOfMonth(value) : monthCount == 30 ? 30 : 29;
                        nextDate = pc.ToDateTime(yyyy, MM, dd, value.Hour, value.Minute, 0, 0);

                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    var isLeapYear = DateTime.IsLeapYear(value.Year);

                    nextDate = value.AddMonths(-1);
                    break;
                }
        }
        return nextDate;
    }

    public static int GetYear(this DateTime dateTime, DatePickerType type)
    {
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (dateTime > new DateTime().AddYears(623))
                    {
                        int jalaliYear = 0;
                        PersianCalendar pc = new PersianCalendar();
                        jalaliYear = pc.GetYear(dateTime);
                        return jalaliYear;
                    }
                    else
                    {
                        return dateTime.Year - 621;
                    }
                }
            case DatePickerType.Gregorian:
                {
                    return dateTime.Year;
                }
            default:
                return dateTime.Year;
        }
    }
    public static int GetMonth(this DateTime dateTime, DatePickerType type)
    {
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (dateTime > new DateTime().AddYears(623))
                    {
                        int jalaliMonth = 0;
                        PersianCalendar pc = new PersianCalendar();
                        jalaliMonth = pc.GetMonth(dateTime);
                        return jalaliMonth;
                    }
                    else
                    {
                        return 0;
                    }
                }
            case DatePickerType.Gregorian:
                {
                    return dateTime.Month;
                }
            default:
                return dateTime.Month;
        }
    }
    public static int GetCountDayOfMonth(this DateTime value, DatePickerType type)
    {
        int count = 0;
        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (value > new DateTime().AddYears(623))
                    {
                        int yyyy = 0, MM = 0, dd = 0;

                        PersianCalendar pc = new PersianCalendar();
                        yyyy = pc.GetYear(value);
                        MM = pc.GetMonth(value);
                        dd = pc.GetDayOfMonth(value);
                        bool isLeapYear = false;
                        if (MM == 12)
                        {
                            try
                            {
                                pc.ToDateTime(yyyy, 12, 30, 0, 0, 0, 0);
                                isLeapYear = true;
                            }
                            catch
                            {
                                isLeapYear = false;
                            }
                        }
                        count = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;

                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    var isLeapYear = DateTime.IsLeapYear(value.Year);
                    count = value.Month >= 1 && value.Month <= 6 ? 31 : value.Month >= 7 && value.Month <= 11 ? 30 : value.Month == 12 && !isLeapYear ? 28 : value.Month == 12 && isLeapYear ? 29 : 0;
                    break;
                }
        }
        return count;

    }

    public static string GetMonthName(this DateTime dateTime, DatePickerType type)
    {
        string displayName = "-";

        switch (type)
        {
            case DatePickerType.Jalali:
                {
                    if (dateTime > new DateTime().AddYears(623))
                    {
                        int jalaliMonth = 0;
                        PersianCalendar pc = new PersianCalendar();
                        jalaliMonth = pc.GetMonth(dateTime);
                        displayName = jalaliMonth == 1 ? "فروردین" : jalaliMonth == 2 ? "اردیبهشت" : jalaliMonth == 3 ? "خرداد" : jalaliMonth == 4 ? "تیر" : jalaliMonth == 5 ? "مرداد" : jalaliMonth == 6 ? "شهریور" : jalaliMonth == 7 ? "مهر" : jalaliMonth == 8 ? "آبان" : jalaliMonth == 9 ? "آذر" : jalaliMonth == 10 ? "دی" : jalaliMonth == 11 ? "بهمن" : jalaliMonth == 12 ? "اسفند" : "";
                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    displayName = dateTime.ToString("MMM", CultureInfo.InvariantCulture);
                    break;
                }
            default:
                break;
        }
        return displayName;

    }
    public static string GregorianToJalali(this DateTime value, char dateSpliter, DateType toType)
    {
        string stringValue = "-";
        try
        {
            if (value > new DateTime().AddYears(623))
            {
                switch (toType)
                {
                    case DateType.DateTime:
                        {
                            int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0;

                            PersianCalendar pc = new PersianCalendar();
                            yyyy = pc.GetYear(value);
                            MM = pc.GetMonth(value);
                            dd = pc.GetDayOfMonth(value);
                            HH = pc.GetHour(value);
                            mm = pc.GetMinute(value);

                            stringValue = $"{yyyy.ToString()}{dateSpliter}{MM.ToString().PadLeft(2, '0')}{dateSpliter}{dd.ToString().PadLeft(2, '0')} {HH.ToString().PadLeft(2, '0')}:{mm.ToString().PadLeft(2, '0')}".Trim();
                            break;
                        }
                    case DateType.Date:
                        {
                            int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0;

                            PersianCalendar pc = new PersianCalendar();
                            yyyy = pc.GetYear(value);
                            MM = pc.GetMonth(value);
                            dd = pc.GetDayOfMonth(value);
                            HH = pc.GetHour(value);
                            mm = pc.GetMinute(value);

                            stringValue = $"{yyyy.ToString()}{dateSpliter}{MM.ToString().PadLeft(2, '0')}{dateSpliter}{dd.ToString().PadLeft(2, '0')}".Trim();
                            break;
                        }
                }
            }
            return stringValue;
        }
        catch (ArgumentOutOfRangeException)
        {
            return stringValue;
        }

    }


}
