using System.Globalization;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Services
{
    public class JalaliDatePicker : DatePicker
    {
        private const string ALLOW_CHARACTERS = "yMdHhm-/:";
        private const char DATE_SPLITER = '-';
        private const string YEAR_yyyy = "yyyy";
        private const string MONTH_MM = "MM";
        private const string MONTH_MMM = "MMM";
        private const string DAY_dd = "dd";
        private const string HOUR_HH = "HH";
        private const string HOUR_hh = "hh";
        private const string MINUTE_mm = "mm";
        public JalaliDatePicker(int createNumberOfYears)
        {
            CreateNumberOfYears = createNumberOfYears;
        }
        public override int CreateNumberOfYears { get; set; }

        private string Hour24Label(int hour)
        {
            string am = "ق.ظ";
            string pm = "ب.ظ";
            return hour <= 12 ? am : pm;
        }

        private string Hour24To12(int hour)
        {
            return hour > 12 ? (hour - 12).ToString().PadLeft(2, '0') : hour.ToString().PadLeft(2, '0');
        }

        private (string yyyy, string MM, string dd, string HH, string mm) jalaliDateTimeSpliter(string jalaliDateTime)
        {
            return (jalaliDateTime[0..4], jalaliDateTime[5..7], jalaliDateTime[8..2], jalaliDateTime[11..2], jalaliDateTime[14..16]);
        }

        private string GregorianToJalali(DateTime value, char dateSpliter, DateType toType)
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
                                int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0, ss = 0;

                                PersianCalendar pc = new PersianCalendar();
                                yyyy = pc.GetYear(value);
                                MM = pc.GetMonth(value);
                                dd = pc.GetDayOfMonth(value);
                                HH = pc.GetHour(value);
                                mm = pc.GetMinute(value);
                                ss = pc.GetSecond(value);

                                stringValue = $"{yyyy}{dateSpliter}{MM.ToString().PadLeft(2, '0')}{dateSpliter}{dd.ToString().PadLeft(2, '0')} {HH.ToString().PadLeft(2, '0')}:{mm.ToString().PadLeft(2, '0')}:{ss.ToString().PadLeft(2, '0')}".Trim();
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

        private DateTime JalaliToGregorian(string value, DateType toType, char dateSpliter, ref bool isValid)
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

        public override Task<string> Convert(DateTime dateTime, string format)
        {
            var isValid = format.All(q => ALLOW_CHARACTERS.Contains(q));
            if (!isValid)
                throw new FormatException(format);

            var jalaliFormat = format;
            var jalaliDateTime = GregorianToJalali(dateTime, DATE_SPLITER, DateType.DateTime);
            var jalaliDateTimeSplited = jalaliDateTimeSpliter(jalaliDateTime);

            jalaliFormat.Replace(YEAR_yyyy, jalaliDateTimeSplited.yyyy);
            jalaliFormat.Replace(MONTH_MM, jalaliDateTimeSplited.MM);
            jalaliFormat.Replace(MONTH_MMM, GetMonthName(dateTime));
            jalaliFormat.Replace(DAY_dd, jalaliDateTimeSplited.dd);
            jalaliFormat.Replace(HOUR_HH, jalaliDateTimeSplited.HH);
            jalaliFormat.Replace(HOUR_hh, Hour24To12(int.Parse(jalaliDateTimeSplited.HH)));
            jalaliFormat.Replace(MINUTE_mm, jalaliDateTimeSplited.mm);

            jalaliFormat += format.Contains(HOUR_hh) ? " " + Hour24Label(int.Parse(jalaliDateTimeSplited.HH)) : "";
            return Task.FromResult(jalaliDateTime);
        }

        public override Task<(DateTime dateTime, bool isValid)> Convert(string dateTime)
        {
            bool isValid = false;
            var dateTimeResult = JalaliToGregorian(dateTime, DateType.DateTime, DATE_SPLITER, ref isValid);
            return Task.FromResult((dateTimeResult, isValid));
        }

        public override string GetMonthName(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int jalaliMonth = pc.GetMonth(dateTime);
            string displayName = jalaliMonth == 1 ? "فروردین" : jalaliMonth == 2 ? "اردیبهشت" : jalaliMonth == 3 ? "خرداد" : jalaliMonth == 4 ? "تیر" : jalaliMonth == 5 ? "مرداد" : jalaliMonth == 6 ? "شهریور" : jalaliMonth == 7 ? "مهر" : jalaliMonth == 8 ? "آبان" : jalaliMonth == 9 ? "آذر" : jalaliMonth == 10 ? "دی" : jalaliMonth == 11 ? "بهمن" : jalaliMonth == 12 ? "اسفند" : "";
            return displayName;
        }

        public override DateTime GetPrevMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetMonth(dateTime) == 1 ? pc.GetYear(dateTime) - 1 : pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime) == 1 ? 12 : pc.GetMonth(dateTime) - 1;

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

            int dd = monthCount >= pc.GetDayOfMonth(dateTime) ? pc.GetDayOfMonth(dateTime) : monthCount == 30 ? 30 : 29;
            var prevMonth = pc.ToDateTime(yyyy, MM, dd, dateTime.Hour, dateTime.Minute, 0, 0);
            return prevMonth;
        }

        public override DateTime GetNextMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetMonth(dateTime) == 12 ? pc.GetYear(dateTime) + 1 : pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime) == 12 ? 1 : pc.GetMonth(dateTime) + 1;

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

            int dd = monthCount >= pc.GetDayOfMonth(dateTime) ? pc.GetDayOfMonth(dateTime) : monthCount == 30 ? 30 : 29;
            var nextMonth = pc.ToDateTime(yyyy, MM, dd, dateTime.Hour, dateTime.Minute, 0, 0);
            return nextMonth;
        }

        public override DateTime ChangeMonth(DateTime dateTime, int month)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));
            if (month > 12 || month < 1)
                throw new ArgumentOutOfRangeException(nameof(month));

            PersianCalendar pc = new PersianCalendar();
            var yyyy = pc.GetYear(dateTime);
            var dd = pc.GetDayOfMonth(dateTime);
            var newDateTime = pc.ToDateTime(yyyy, month, dd, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            return newDateTime;
        }

        public override DateTime ChangeYear(DateTime dateTime, int year)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));
            if (year > 9999 || year < 623)
                throw new ArgumentOutOfRangeException(nameof(year));

            PersianCalendar pc = new PersianCalendar();
            var MM = pc.GetMonth(dateTime);
            var dd = pc.GetDayOfMonth(dateTime);
            var newDateTime = pc.ToDateTime(year, MM, dd, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            return newDateTime;
        }

        public override int GetCountDayOfMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime);
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
            var count = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;
            return count;
        }

        public override int GetYear(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int jalaliYear = pc.GetYear(dateTime);
            return jalaliYear;
        }

        public override int GetMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            int jalaliMonth = 0;
            PersianCalendar pc = new PersianCalendar();
            jalaliMonth = pc.GetMonth(dateTime);
            return jalaliMonth;
        }

        public override DateTime ChangeHour(DateTime dateTime, int hour)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
            return newDateTime;
        }

        public override DateTime ChangeMinute(DateTime dateTime, int minute)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
            return newDateTime;
        }

        public override int GetYearItem(int index)
        {
            return GetYear(DateTime.Now) - CreateNumberOfYears + index;
        }

        public override string GetWeekChar(int dayOfWeek)
        {
            return dayOfWeek == 0 ? "ش" : dayOfWeek == 1 ? "ی" : dayOfWeek == 2 ? "د" : dayOfWeek == 3 ? "س" : dayOfWeek == 4 ? "چ" : dayOfWeek == 5 ? "پ" : dayOfWeek == 6 ? "ج" : dayOfWeek.ToString();
        }

        public override DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime);
            int dd = pc.GetDayOfMonth(dateTime);
            int HH = pc.GetHour(dateTime);
            int mm = pc.GetMinute(dateTime);
            var newDateTime = pc.ToDateTime(yyyy, MM, 1, HH, mm, 0, 0);
            return newDateTime;
        }
    }
}
