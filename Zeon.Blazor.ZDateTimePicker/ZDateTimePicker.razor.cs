using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;
using Zeon.Blazor.ZDateTimePicker.Extensions;
using Zeon.Blazor.ZDateTimePicker.Services;

namespace Zeon.Blazor.ZDateTimePicker;

public partial class ZDateTimePicker : ComponentBase
{
    private const DatePickerType DEFAULT_DATE_PICKER_TYPE = DatePickerType.Jalali;
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm";
    private const string TIME_FORMAT = "HH:mm";

    private readonly Dictionary<DatePickerType, DatePicker> _datePickerTypes;

    private DatePickerType _datePickerType = DEFAULT_DATE_PICKER_TYPE;
    private DatePicker _datePicker;
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
            DateTimeIsValid.InvokeAsync(IsValid);
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
                InvokeAsync(async () => await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT)));
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
                InvokeAsync(async () => await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT)));
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
            InvokeAsync(async () => await ParseInput(value));
        }
    }


    public ZDateTimePicker()
    {
        _datePickerTypes = new();
        _datePickerTypes.Add(DatePickerType.Jalali, new JalaliDatePicker(CreateNumberOfYears));
        _datePickerTypes.Add(DatePickerType.Gregorian, new GregorianDatePicker(CreateNumberOfYears));
        _datePicker = GetDatePickerInstance(DEFAULT_DATE_PICKER_TYPE);
    }

    private async Task ParseInput(string dateTime)
    {
        var result = await _datePicker.Convert(dateTime);
        IsValid = result.isValid;
        CurrentDateTime = result.dateTime;
        await ChangeDateTime();
    }

    private async Task ChangeDateTime()
    {
        if (ChangedDateTime.HasDelegate)
            await ChangedDateTime.InvokeAsync(CurrentDateTime);

        PickerDateTime = CurrentDateTime;
        _displayMonth = _datePicker.GetMonthName(PickerDateTime);
        CurrentHourPicker = CurrentDateTime.Hour.ToString().PadLeft(2, '0');
        CurrentMinutePicker = CurrentDateTime.Minute.ToString().PadLeft(2, '0');
    }

    [Parameter]
    public int CreateNumberOfYears { get; set; } = 60;
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
    public string Format { get; set; } = "yyyy-MM-dd HH:mm";

    [Parameter]
    public DatePickerType DatePickerType { get => _datePickerType; set => SetDatePickerType(value); }

    private void SetDatePickerType(DatePickerType value)
    {
        _datePickerType = value;
        _datePicker = GetDatePickerInstance(value);
    }

    private DatePicker GetDatePickerInstance(DatePickerType value)
    {
        return _datePickerTypes.Single(q => q.Key == value).Value;
    }

    [Parameter]
    public InputType InputPickerType { get; set; } = InputType.Date;

    [Parameter]
    public DateTime? DefaultDateTime { get; set; }

    [Parameter]
    public EventCallback<DateTime> ChangedDateTime { get; set; }

    [Parameter]
    public EventCallback<bool> DateTimeIsValid { get; set; }


    protected async override void OnInitialized()
    {
        var dateTimeFormat = await _datePicker.Convert((DateTime)DefaultDateTime!, Format);
        var dateTime = await _datePicker.Convert((DateTime)DefaultDateTime!, DATE_TIME_FORMAT);
    }

    public void OpenClose_Onclick()
    {
        _datePickerCardDisplay = _datePickerCardDisplay == "block" ? "none" : "block";
    }
    private void OpenPanel_Onclick()
    {
        _panelDisplay = "block";
    }
    private async void Year_Onclick(int value)
    {
        PickerDateTime = _datePicker.ChangeYear(PickerDateTime, value);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    private async void Month_Onclick(int value)
    {
        PickerDateTime = _datePicker.ChangeMonth(PickerDateTime, value);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    private void ChangeDateInPanel_Onclick()
    {
        _panelDisplay = "none";
    }
    public async Task ChangeDateTimePicker_Onclick(string value)
    {
        var selectedDatePicker = DateTime.Parse(value);
        SelectedDateTime = await _datePicker.Convert(selectedDatePicker, Format);
        PickerDateTime = selectedDatePicker;
        _displayMonth = _datePicker.GetMonthName(PickerDateTime);
    }
    public async void NextMonth_Onclick()
    {
        PickerDateTime = _datePicker.GetNextMonth(PickerDateTime);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    public async void PrevMonth_Onclick()
    {
        PickerDateTime = _datePicker.GetPrevMonth(PickerDateTime);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));

    }
    public async void SetToday_Onclick()
    {
        PickerDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PickerDateTime.Hour, PickerDateTime.Minute, 0);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }
    public async void Ok_Onclick()
    {
        InputDate = SelectedDateTime = await _datePicker.Convert(PickerDateTime, DATE_TIME_FORMAT);
        _datePickerCardDisplay = "none";
    }
}
