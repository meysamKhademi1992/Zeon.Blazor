using System.Globalization;
using Zeon.Blazor.ZDateTimePicker.Constants;
namespace Zeon.Blazor.ZDateTimePicker.Extensions;

public static class StringExtensions
{
    public static DateTime JalaliToGregorian(this string value, DateType toType, char dateSpliter, ref bool isValid)
    {
        var outDateTime = new DateTime();
        try
        {

            switch (toType)
            {
                case DateType.DateTime:
                    {
                        if (value.Length == 16)
                        {
                            var datetime = value.Split(" ");
                            var date = datetime[0];
                            var time = datetime[1];
                            var ymd = date.Split(dateSpliter);
                            var hm = time.Split(":");
                            int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0;
                            int.TryParse(ymd[0], out yyyy);
                            int.TryParse(ymd[1], out MM);
                            int.TryParse(ymd[2], out dd);
                            int.TryParse(hm[0], out HH);
                            int.TryParse(hm[1], out mm);

                            PersianCalendar pc = new PersianCalendar();
                            outDateTime = pc.ToDateTime(yyyy, MM, dd, HH, mm, 0, 0);
                            isValid = true;
                        }
                        break;
                    }
                case DateType.Date:
                    {
                        if (value.Length == 10)
                        {
                            var date = value.Split(dateSpliter);
                            int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0;
                            int.TryParse(date[0], out yyyy);
                            int.TryParse(date[1], out MM);
                            int.TryParse(date[2], out dd);

                            PersianCalendar pc = new PersianCalendar();
                            outDateTime = pc.ToDateTime(yyyy, MM, dd, HH, mm, 0, 0);
                            isValid = true;

                        }
                        break;
                    }

            }
            return outDateTime;
        }
        catch (Exception)
        {
            return outDateTime;
        }

    }
    public static DateTime ParseGregorian(this string value, DateType toType, ref bool isvalid)
    {
        var outDateTime = new DateTime();
        try
        {
            switch (toType)
            {
                case DateType.DateTime:
                    {
                        if (value.Length == 16)
                        {
                            var datetime = value.Split(" ");
                            var date = datetime[0];
                            var time = datetime[1];
                            isvalid = DateTime.TryParse(date, out outDateTime);
                        }
                        break;
                    }
                case DateType.Date:
                    {
                        if (value.Length == 10)
                        {
                            isvalid = DateTime.TryParse(value, out outDateTime);
                        }
                        break;
                    }

            }
            return outDateTime;
        }
        catch (Exception)
        {
            return outDateTime;
        }

    }
    public static TimeSpan TimeParse(this string value, ref bool isValid)
    {
        var outTime = new TimeSpan();
        try
        {
            if (value.Length == 5)
            {
                var time = value.Split(":");
                int HH = 0, mm = 0, ss = 0;
                int.TryParse(time[0], out HH);
                int.TryParse(time[1], out mm);
                int.TryParse(time[2], out ss);
                PersianCalendar pc = new PersianCalendar();
                isValid = TimeSpan.TryParse(value, out outTime);
            }
            return outTime;
        }
        catch (Exception)
        {
            return outTime;
        }
    }
}

