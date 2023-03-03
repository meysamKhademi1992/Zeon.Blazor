using System.Globalization;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;

namespace Zeon.Blazor.ZDateTimePicker.Services
{

    public class JalaliDatePicker : DatePicker
    {
        private const string DIRECTION = "rtl";
        private const string NEXT_MONTH_TEXT = "ماه بعد";
        private const string PREV_MONTH_TEXT = "ماه قبل";
        private const string TODAY_TEXT = "امروز";
        private const string OK_TEXT = "تایید";
        private const string YEAR_TEXT = "سال";
        private const string MONTH_TEXT = "ماه";
        private const string HOUR_TEXT = "ساعت";
        private const string MINUTE_TEXT = "دقیقه";
        private const string YEAR_yyyy = "yyyy";
        private const string MONTH_MM = "MM";
        private const string MONTH_MMM = "MMM";
        private const string DAY_dd = "dd";
        private const string HOUR_HH = "HH";
        private const string HOUR_hh = "hh";
        private const string MINUTE_mm = "mm";
        private const string SECONDS_ss = "ss";
        private const string MILI_SECONDS_fff = "fff";
        private const string TIME_TYPE_tt = "tt";
        private const char DATE_SPLITER = '-';

        public JalaliDatePicker(int createNumberOfYears)
        {
            CreateNumberOfYears = createNumberOfYears;
        }


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

        private (string yyyy, string MM, string dd, string HH, string mm, string ss, string fff) JalaliDateTimeSpliter(string jalaliDateTime)
        {
            return (jalaliDateTime[0..4], jalaliDateTime[5..7], jalaliDateTime[8..10], jalaliDateTime[11..13], jalaliDateTime[14..16], jalaliDateTime[17..19], jalaliDateTime[20..22]);
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
                                double fff = 0;
                                PersianCalendar pc = new PersianCalendar();
                                yyyy = pc.GetYear(value);
                                MM = pc.GetMonth(value);
                                dd = pc.GetDayOfMonth(value);
                                HH = pc.GetHour(value);
                                mm = pc.GetMinute(value);
                                ss = pc.GetSecond(value);
                                fff = pc.GetMilliseconds(value);
                                stringValue = $"{yyyy}{dateSpliter}{MM.ToString().PadLeft(2, '0')}{dateSpliter}{dd.ToString().PadLeft(2, '0')} {HH.ToString().PadLeft(2, '0')}:{mm.ToString().PadLeft(2, '0')}:{ss.ToString().PadLeft(2, '0')}:{fff.ToString().PadLeft(3, '0')}".Trim();
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
            value = PersianNumberToEnglish(value);
            var outDateTime = new DateTime();
            try
            {

                switch (toType)
                {
                    case DateType.DateTime:
                        {
                            var datetime = value.Split(" ");
                            var date = datetime[0];
                            var time = datetime[1];
                            var ymd = date.Split(dateSpliter);
                            var hms = time.Split(":");
                            var h = hms[0];
                            var m = hms[1];
                            var sms = hms[2].Split(".");
                            var s = sms[0];
                            var ms = sms[1];
                            int yyyy = 0, MM = 0, dd = 0, HH = 0, mm = 0, ss = 0, fff = 0;
                            int.TryParse(ymd[0], out yyyy);
                            int.TryParse(ymd[1], out MM);
                            int.TryParse(ymd[2], out dd);
                            int.TryParse(h, out HH);
                            int.TryParse(m, out mm);
                            int.TryParse(s, out ss);
                            int.TryParse(ms, out fff);


                            PersianCalendar pc = new PersianCalendar();
                            outDateTime = pc.ToDateTime(yyyy, MM, dd, HH, mm, ss, fff);
                            isValid = true;

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

        private string EnglishNumberToPersian(string text)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['0'] = '۰',
                ['1'] = '۱',
                ['2'] = '۲',
                ['3'] = '۳',
                ['4'] = '۴',
                ['5'] = '۵',
                ['6'] = '۶',
                ['7'] = '۷',
                ['8'] = '۸',
                ['9'] = '۹'
            };
            foreach (var item in text)
            {
                if (LettersDictionary.Any(q => q.Key == item))
                    text = text.Replace(item, LettersDictionary[item]);
            }
            return text;
        }

        private string PersianNumberToEnglish(string text)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in text)
            {
                if (LettersDictionary.Any(q => q.Key == item))
                    text = text.Replace(item, LettersDictionary[item]);
            }
            return text;
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
            var jalaliFormat = format;
            var jalaliDateTime = GregorianToJalali(dateTime, DATE_SPLITER, DateType.DateTime);
            var jalaliDateTimeSplited = JalaliDateTimeSpliter(jalaliDateTime);

            jalaliFormat = jalaliFormat.Replace(YEAR_yyyy, jalaliDateTimeSplited.yyyy);
            jalaliFormat = jalaliFormat.Replace(MONTH_MMM, GetMonthName(dateTime));
            jalaliFormat = jalaliFormat.Replace(MONTH_MM, jalaliDateTimeSplited.MM);
            jalaliFormat = jalaliFormat.Replace(DAY_dd, jalaliDateTimeSplited.dd);
            jalaliFormat = jalaliFormat.Replace(HOUR_HH, jalaliDateTimeSplited.HH);
            jalaliFormat = jalaliFormat.Replace(HOUR_hh, Hour24To12(int.Parse(jalaliDateTimeSplited.HH)));
            jalaliFormat = jalaliFormat.Replace(MINUTE_mm, jalaliDateTimeSplited.mm);
            jalaliFormat = jalaliFormat.Replace(SECONDS_ss, jalaliDateTimeSplited.ss);
            jalaliFormat = jalaliFormat.Replace(MILI_SECONDS_fff, jalaliDateTimeSplited.fff);
            jalaliFormat = jalaliFormat.Replace(TIME_TYPE_tt, Hour24Label(int.Parse(jalaliDateTimeSplited.HH)));

            string persianjalaliFormat = EnglishNumberToPersian(jalaliFormat);

            return Task.FromResult(persianjalaliFormat);
        }

        internal override Task<(DateTime dateTime, bool isValid)> Convert(string dateTime)
        {
            bool isValid = false;
            var dateTimeResult = JalaliToGregorian(dateTime, DateType.DateTime, DATE_SPLITER, ref isValid);
            return Task.FromResult((dateTimeResult, isValid));
        }

        internal override string GetMonthName(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int jalaliMonth = pc.GetMonth(dateTime);
            string displayName = jalaliMonth == 1 ? "فروردین" : jalaliMonth == 2 ? "اردیبهشت" : jalaliMonth == 3 ? "خرداد" : jalaliMonth == 4 ? "تیر" : jalaliMonth == 5 ? "مرداد" : jalaliMonth == 6 ? "شهریور" : jalaliMonth == 7 ? "مهر" : jalaliMonth == 8 ? "آبان" : jalaliMonth == 9 ? "آذر" : jalaliMonth == 10 ? "دی" : jalaliMonth == 11 ? "بهمن" : jalaliMonth == 12 ? "اسفند" : "";
            return displayName;
        }

        internal override DateTime GetPrevMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetMonth(dateTime) == 1 ? pc.GetYear(dateTime) - 1 : pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime) == 1 ? 12 : pc.GetMonth(dateTime) - 1;

            bool isLeapYear = pc.IsLeapYear(yyyy);

            var monthCount = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;

            int dd = monthCount >= pc.GetDayOfMonth(dateTime) ? pc.GetDayOfMonth(dateTime) : monthCount == 30 ? 30 : 29;
            var prevMonth = pc.ToDateTime(yyyy, MM, dd, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            return prevMonth;
        }

        internal override DateTime GetNextMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetMonth(dateTime) == 12 ? pc.GetYear(dateTime) + 1 : pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime) == 12 ? 1 : pc.GetMonth(dateTime) + 1;

            bool isLeapYear = pc.IsLeapYear(yyyy);

            var monthCount = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;

            int dd = monthCount >= pc.GetDayOfMonth(dateTime) ? pc.GetDayOfMonth(dateTime) : monthCount == 30 ? 30 : 29;
            var nextMonth = pc.ToDateTime(yyyy, MM, dd, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            return nextMonth;
        }

        internal override DateTime ChangeMonth(DateTime dateTime, int month)
        {
            month = int.Parse(PersianNumberToEnglish(month.ToString()));
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

        internal override DateTime ChangeYear(DateTime dateTime, int year)
        {
            year = int.Parse(PersianNumberToEnglish(year.ToString()));

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

        internal override int GetCountDayOfMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime);
            bool isLeapYear = pc.IsLeapYear(yyyy);

            var count = MM >= 1 && MM <= 6 ? 31 : MM >= 7 && MM <= 11 ? 30 : MM == 12 && !isLeapYear ? 29 : MM == 12 && isLeapYear ? 30 : 0;
            return count;
        }

        internal override int GetYear(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int jalaliYear = pc.GetYear(dateTime);
            return jalaliYear;
        }

        internal override int GetMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));
            PersianCalendar pc = new PersianCalendar();
            int jalaliMonth = pc.GetMonth(dateTime);
            return jalaliMonth;
        }

        internal override DateTime ChangeHour(DateTime dateTime, int hour)
        {
            hour = int.Parse(PersianNumberToEnglish(hour.ToString()));

            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
            return newDateTime;
        }

        internal override DateTime ChangeMinute(DateTime dateTime, int minute)
        {
            minute = int.Parse(PersianNumberToEnglish(minute.ToString()));

            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
            return newDateTime;
        }

        internal override int GetYearItem(int index)
        {
            return GetYear(DateTime.Now) - CreateNumberOfYears / 2 + index;
        }

        internal override string GetYearDisplayItem(int year)
        {
            return EnglishNumberToPersian(year.ToString());
        }

        internal override string GetMonthDisplayItem(int month)
        {
            return EnglishNumberToPersian(month.ToString());
        }
        internal override string GetDayDisplayItem(int day)
        {
            return EnglishNumberToPersian(day.ToString());
        }

        internal override string GetHourDisplayItem(int hour)
        {
            return EnglishNumberToPersian(hour.ToString());
        }

        internal override string GetMinuteDisplayItem(int minute)
        {
            return EnglishNumberToPersian(minute.ToString());
        }

        internal override string GetWeekChar(int dayOfWeek)
        {
            return dayOfWeek == 0 ? "ش" : dayOfWeek == 1 ? "ی" : dayOfWeek == 2 ? "د" : dayOfWeek == 3 ? "س" : dayOfWeek == 4 ? "چ" : dayOfWeek == 5 ? "پ" : dayOfWeek == 6 ? "ج" : dayOfWeek.ToString();
        }

        internal override DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            if (dateTime < new DateTime().AddYears(623))
                throw new ArgumentOutOfRangeException(nameof(dateTime));

            PersianCalendar pc = new PersianCalendar();
            int yyyy = pc.GetYear(dateTime);
            int MM = pc.GetMonth(dateTime);
            int dd = pc.GetDayOfMonth(dateTime);
            int HH = pc.GetHour(dateTime);
            int mm = pc.GetMinute(dateTime);
            int ss = pc.GetSecond(dateTime);
            var newDateTime = pc.ToDateTime(yyyy, MM, 1, HH, mm, ss, 0);
            return newDateTime;
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
