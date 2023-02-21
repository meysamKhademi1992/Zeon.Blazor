using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;
using Zeon.Blazor.ZDateTimePicker.Extensions;

namespace Zeon.Blazor.ZDateTimePicker;

public partial class ZDateTimePicker : ComponentBase
{
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm";

    private string _datePickerCardDisplay = "none";
    private string _panelDisplay = "none";
    private string _inputDate = string.Empty;
    private string _currentHourPicker = string.Empty;
    private string _currentMinutePicker = string.Empty;
    private string _displayMonth = string.Empty;

    private char DateSpliter { get; set; }

    private string SelectedDateTime { get; set; } = string.Empty;

    private DateTime CurrentDateTime { get; set; }
    private TimeSpan CurrentTime { get; set; }
    private DateTime PickerDateTime { get; set; }

    private bool _isValid = false;
    private bool IsValid
    {
        get
        {
            return _isValid;
        }
        set
        {
            _isValid = value;
            ChangedIsValid.InvokeAsync(IsValid);
        }
    }
    private string CurrentHourPicker
    {
        get
        {
            return _currentHourPicker;
        }
        set
        {
            _currentHourPicker = value;
            var hour = 0;
            int.TryParse(value, out hour);
            if (hour >= 0 && hour <= 23)
            {
                PickerDateTime = PickerDateTime.ChangeHour(hour);
                ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));

            }

        }
    }
    private string CurrentMinutePicker
    {
        get
        {
            return _currentMinutePicker;
        }
        set
        {
            _currentMinutePicker = value;

            if (int.TryParse(value, out var minute) && minute >= 0 && minute <= 59)
            {
                PickerDateTime = PickerDateTime.ChangeMinute(minute);
                ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
            }
        }
    }
    public string InputDate
    {
        get
        {
            return _inputDate;
        }
        set
        {
            _inputDate = value;
            DateSpliter = value.Contains('/') ? '/' : '-';
            InvokeAsync(async () => await ParseInput(CalendarType, value, InputPickerType));

            if (ChangedDateTime.HasDelegate)
                ChangedDateTime.InvokeAsync(CurrentDateTime);

            if (ChangedTime.HasDelegate)
                ChangedTime.InvokeAsync(CurrentTime);

            PickerDateTime = CurrentDateTime;
            _displayMonth = PickerDateTime.GetMonthName(CalendarType);
            CurrentHourPicker = CurrentDateTime.Hour.ToString().PadLeft(2, '0');
            CurrentMinutePicker = CurrentDateTime.Minute.ToString().PadLeft(2, '0');

            switch (CalendarType)

            {
                case DatePickerType.Jalali:
                    {
                        switch (InputPickerType)
                        {
                            case InputType.DateTime:
                                {

                                   
                                    IsValid = _isValid;
                                    break;
                                }
                            case InputType.Date:
                                {
                                    CurrentDateTime = value.JalaliToGregorian(DateType.Date, DateSpliter, ref _isValid);
                                    CurrentTime = new TimeSpan(CurrentDateTime.Hour, CurrentDateTime.Minute, 0);
                                    IsValid = _isValid;

                                    break;
                                }
                            case InputType.TimeSpan:
                                {
                                    CurrentDateTime = new DateTime();
                                    CurrentTime = value.TimeParse(ref _isValid);
                                    IsValid = _isValid;

                                    break;
                                }
                        }
                        break;
                    }
                case DatePickerType.Gregorian:
                    {
                        switch (InputPickerType)
                        {
                            case InputType.DateTime:
                                {
                                    CurrentDateTime = value.ParseGregorian(DateType.DateTime, ref _isValid);
                                    CurrentTime = new TimeSpan(CurrentDateTime.Hour, CurrentDateTime.Minute, 0);
                                    IsValid = _isValid;

                                    break;
                                }
                            case InputType.Date:
                                {
                                    CurrentDateTime = value.ParseGregorian(DateType.Date, ref _isValid);
                                    CurrentTime = new TimeSpan(CurrentDateTime.Hour, CurrentDateTime.Minute, 0);
                                    IsValid = _isValid;

                                    break;
                                }
                            case InputType.TimeSpan:
                                {
                                    CurrentDateTime = new DateTime();
                                    CurrentTime = value.TimeParse(ref _isValid);
                                    IsValid = _isValid;

                                    break;
                                }
                        }
                        break;
                    }
            }

        }
    }

    private async Task ParseInput(DatePickerType datePickerType, string value, InputType inputType)
    {
        var result = await _dateTimeParser.Parse(datePickerType, value, inputType);
        IsValid = result.isValid;
        CurrentDateTime = result.dateTime;
        CurrentTime = result.timeSpan;
    }

    private readonly IDateTimeParser _dateTimeParser;
    public ZDateTimePicker(IDateTimeParser dateTimeParser)
    {
        _dateTimeParser = dateTimeParser;
    }

    [Parameter]
    public string OkText { get; set; } = "ثبت";
    [Parameter]
    public string SetTodayText { get; set; } = "امروز";
    [Parameter]
    public string YearLabel { get; set; } = "سال";
    [Parameter]
    public string MonthLabel { get; set; } = "ماه";
    [Parameter]
    public string HourLabel { get; set; } = "ساعت";
    [Parameter]
    public string MinuteLabel { get; set; } = "دقیقه";
    [Parameter]
    public string NextMonthLabel { get; set; } = "ماه قبل";
    [Parameter]
    public string PrevMonthLabel { get; set; } = "ماه بعد";
    [Parameter]
    public DateTime? DefaultDateTime { get; set; }
    [Parameter]
    public TimeSpan? DefaultTime { get; set; }
    [Parameter]
    public DatePickerType CalendarType { get; set; } = DatePickerType.Jalali;
    [Parameter]
    public InputType InputPickerType { get; set; } = InputType.Date;
    [Parameter]
    public EventCallback<DateTime> ChangedDateTime { get; set; }
    [Parameter]
    public EventCallback<TimeSpan> ChangedTime { get; set; }
    [Parameter]
    public EventCallback<bool> ChangedIsValid { get; set; }


    protected override void OnInitialized()
    {
        var input = InputPickerType != InputType.TimeSpan ? DefaultDateTime?.ToString(DATE_TIME_FORMAT) : DefaultTime?.ToString("HH:mm");
        DateSpliter = input != null ? input.Contains('/') ? '/' : '-' : '-';

        switch (CalendarType)
        {
            case DatePickerType.Jalali:
                {
                    switch (InputPickerType)
                    {
                        case InputType.DateTime:
                            {
                                InputDate = input != null && input.Length >= 16 ? (DateTime.Parse(input.Substring(0, 16))).GregorianToJalali(DateSpliter, DateType.DateTime) : "";
                                break;
                            }
                        case InputType.Date:
                            {
                                InputDate = input != null && input.Length >= 10 ? (DateTime.Parse(input.Substring(0, 10))).GregorianToJalali(DateSpliter, DateType.Date) : "";

                                break;
                            }
                        case InputType.TimeSpan:
                            {
                                InputDate = input != null && input.Length >= 12 ? input.Substring(0, 5) : "";

                                break;
                            }
                    }
                    break;
                }
            case DatePickerType.Gregorian:
                {
                    switch (InputPickerType)
                    {
                        case InputType.DateTime:
                            {
                                InputDate = input != null && input.Length >= 16 ? input.Substring(0, 16) : "";

                                break;
                            }
                        case InputType.Date:
                            {
                                InputDate = input != null && input.Length >= 10 ? input.Substring(0, 10) : "";

                                break;
                            }
                        case InputType.TimeSpan:
                            {
                                InputDate = input != null && input.Length >= 12 ? input.Substring(0, 5) : "";

                                break;
                            }
                    }
                    break;
                }
        }
    }

    public void OpenClose_Onclick()
    {
        _datePickerCardDisplay = _datePickerCardDisplay == "block" ? "none" : "block";
    }
    private void OpenPanel_Onclick()
    {
        _panelDisplay = "block";
    }
    private void Year_Onclick(int value)
    {
        PickerDateTime = PickerDateTime.ChangeYear(value, CalendarType);
        ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    private void Month_Onclick(int value)
    {
        PickerDateTime = PickerDateTime.ChangeMonth(value, CalendarType);
        ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));

    }
    private void ChangeDateInPanel_Onclick()
    {
        _panelDisplay = "none";
    }
    public void ChangeDateTimePicker_Onclick(string value)
    {
        var selectedDatePicker = DateTime.Parse(value);
        switch (CalendarType)
        {
            case DatePickerType.Jalali:
                {

                    switch (InputPickerType)
                    {
                        case InputType.DateTime:
                            {

                                SelectedDateTime = selectedDatePicker.GregorianToJalali(DateSpliter, DateType.DateTime);

                                break;

                            }
                        case InputType.Date:
                            {

                                SelectedDateTime = selectedDatePicker.GregorianToJalali(DateSpliter, DateType.Date);

                                break;

                            }
                        case InputType.TimeSpan:
                            {
                                SelectedDateTime = value;
                                break;
                            }
                    }
                }
                break;

            case DatePickerType.Gregorian:
                {
                    break;
                }
            default:
                break;
        }
        PickerDateTime = selectedDatePicker;
        _displayMonth = PickerDateTime.GetMonthName(CalendarType);

    }
    public void NextMonth_Onclick()
    {
        PickerDateTime = PickerDateTime.GetNextMonth(CalendarType);
        ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    public void PrevMonth_Onclick()
    {
        PickerDateTime = PickerDateTime.GetPrevMonth(CalendarType);
        ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));

    }
    public void SetToday_Onclick()
    {
        PickerDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PickerDateTime.Hour, PickerDateTime.Minute, 0);
        ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    public void Ok_Onclick()
    {
        InputDate = SelectedDateTime;
        _datePickerCardDisplay = "none";
    }
}
