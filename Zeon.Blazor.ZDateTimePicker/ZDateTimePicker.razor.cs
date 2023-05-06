using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Constants;
using Zeon.Blazor.ZDateTimePicker.Services;

namespace Zeon.Blazor.ZDateTimePicker;

public partial class ZDateTimePicker : ComponentBase
{
    private const DatePickerType DEFAULT_DATE_PICKER_TYPE = DatePickerType.Jalali;
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
    private const string ALLOW_CHARACTERS = "yMdHhmsf-/ : . t";

    private readonly Dictionary<DatePickerType, DatePicker> _datePickerTypes;

    private DatePickerType _datePickerType;
    private bool _isLiveTime = false;
    private DatePicker _datePicker;
    private string _datePickerCardDisplay = "none";
    private string _panelDisplay = "none";
    private string _currentHourPicker = string.Empty;
    private string _currentMinutePicker = string.Empty;
    private string _dateTimeDisplay = string.Empty;
    private string _displayMonth = string.Empty;
    private string _format = "yyyy-MM-dd";

    private DateTime CurrentDateTime { get; set; }
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

    private string SelectedDateTime { get; set; } = string.Empty;
    private string CurrentHourPicker
    {
        get
        {
            return _datePicker.GetHourDisplayItem(int.Parse(_currentHourPicker));
        }
        set
        {
            _currentHourPicker = value;
            var hour = 0;
            int.TryParse(value, out hour);
            if (hour >= 0 && hour <= 23)
            {
                PickerDateTime = _datePicker.ChangeHour(PickerDateTime, hour);
                InvokeAsync(async () => await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT)));
            }

        }
    }
    private string CurrentMinutePicker
    {
        get
        {
            return _datePicker.GetMinuteDisplayItem(int.Parse(_currentMinutePicker));
        }
        set
        {
            _currentMinutePicker = value;

            if (int.TryParse(value, out var minute) && minute >= 0 && minute <= 59)
            {
                PickerDateTime = _datePicker.ChangeMinute(PickerDateTime, minute);
                InvokeAsync(async () => await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT)));
            }
        }
    }
    private string FontFamily { get => GetFontFamily(); }


    [Parameter]
    public string FontFamilyRTL { get; set; } = "inherit";

    [Parameter]
    public string FontFamilyLTR { get; set; } = "inherit";

    [Parameter]
    public int CreateNumberOfYears { get; set; } = 200;

    [Parameter]
    public string Format { get => _format; set => SetFormat(value); }

    [Parameter]
    public DatePickerType DatePickerType { get => _datePickerType; set => InvokeAsync(() => SetDatePickerType(value)); }

    [Parameter]
    public InputType InputPickerType { get; set; } = InputType.Date;

    [Parameter]
    public DateTime? DefaultDateTime { get; set; }

    [Parameter]
    public EventCallback<DateTime> ChangedDateTime { get; set; }

    [Parameter]
    public EventCallback<bool> DateTimeIsValid { get; set; }

    [Parameter]
    public bool IsLiveTime { get; set; } = false;

    [Parameter]
    public int LiveTimeDelay { get; set; } = 500;



    public ZDateTimePicker()
    {
        _datePickerType = DEFAULT_DATE_PICKER_TYPE;
        _datePickerTypes = new()
        {
            { DatePickerType.Jalali, new JalaliDatePicker() },
            { DatePickerType.Gregorian, new GregorianDatePicker() }
        };
        _datePicker = GetDatePickerInstance(_datePickerType);
    }
    protected async override void OnInitialized()
    {
        DefaultDateTime ??= GetDateTimeNow(InputPickerType);
        _isLiveTime = IsLiveTime && LiveTimeDelay > 0;
        var dateTimeFormat = await _datePicker.Convert((DateTime)DefaultDateTime!, Format);
        var dateTime = await _datePicker.Convert((DateTime)DefaultDateTime!, DATE_TIME_FORMAT);
        await InputDate(dateTime, dateTimeFormat);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (_isLiveTime)
            {
                await InvokeAsync(SetLiveTime);
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }

    private string GetFontFamily()
    {
        return _datePicker.Direction switch
        {
            "rtl" => FontFamilyRTL,
            "ltr" => FontFamilyLTR,
            _ => "",
        };
    }

    private DateTime GetDateTimeNow(InputType inputType)
    {
        return inputType == InputType.DateTime ? DateTime.Now : DateTime.Now.Date;
    }

    private async Task SetLiveTime()
    {
        while (_isLiveTime)
        {
            var now = GetDateTimeNow(InputPickerType);
            var dateTimeFormat = await _datePicker.Convert(now, Format);
            var dateTime = await _datePicker.Convert(now, DATE_TIME_FORMAT);
            await InputDate(dateTime, dateTimeFormat);
            this.StateHasChanged();
            await Task.Delay(LiveTimeDelay);
        }
    }

    private void SetFormat(string format)
    {
        var isValid = format.All(q => ALLOW_CHARACTERS.Contains(q));
        if (!isValid)
            throw new FormatException(format);
        _format = format;
    }

    private void SetDatePickerType(DatePickerType value)
    {
        if (_datePickerType != value)
        {
            _datePickerType = value;
            _datePicker = GetDatePickerInstance(value);
            this.InvokeAsync(async () => await Refresh());
        }
    }

    private DatePicker GetDatePickerInstance(DatePickerType value)
    {
        return _datePickerTypes.Single(q => q.Key == value).Value;
    }

    private async Task InputDate(string dateTime, string dateTimeFormat)
    {
        var result = await _datePicker.Convert(dateTime);
        IsValid = result.isValid;

        if (IsValid)
        {
            if (ChangedDateTime.HasDelegate && !CurrentDateTime.Equals(result.dateTime))
                await ChangedDateTime.InvokeAsync(_datePicker.GetResult(result.dateTime, InputPickerType));

            CurrentDateTime = result.dateTime;
            PickerDateTime = CurrentDateTime;
            _dateTimeDisplay = dateTimeFormat;
            _displayMonth = _datePicker.GetMonthName(PickerDateTime);
            CurrentHourPicker = CurrentDateTime.Hour.ToString().PadLeft(2, '0');
            CurrentMinutePicker = CurrentDateTime.Minute.ToString().PadLeft(2, '0');
        }
    }

    private void OpenClose_Onclick()
    {
        _isLiveTime = false;
        _datePickerCardDisplay = _datePickerCardDisplay == "block" ? "none" : "block";
    }

    private void OpenPanel_Onclick()
    {
        _panelDisplay = "block";
    }

    private async Task Year_Onclick(int value)
    {
        PickerDateTime = _datePicker.ChangeYear(PickerDateTime, value);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }

    private async Task Month_Onclick(int value)
    {
        PickerDateTime = _datePicker.ChangeMonth(PickerDateTime, value);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }

    private void ChangeDateInPanel_Onclick()
    {
        _panelDisplay = "none";
    }

    private async Task ChangeDateTimePicker_Onclick(string value)
    {
        var selectedDatePicker = DateTime.Parse(value);
        SelectedDateTime = await _datePicker.Convert(selectedDatePicker, Format);
        PickerDateTime = selectedDatePicker;
        _displayMonth = _datePicker.GetMonthName(PickerDateTime);
    }

    private async Task NextMonth_Onclick()
    {
        PickerDateTime = _datePicker.GetNextMonth(PickerDateTime);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }

    private async Task PrevMonth_Onclick()
    {
        PickerDateTime = _datePicker.GetPrevMonth(PickerDateTime);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }

    private async Task SetToday_Onclick()
    {
        PickerDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PickerDateTime.Hour, PickerDateTime.Minute, 0);
        await ChangeDateTimePicker_Onclick(PickerDateTime.ToString(DATE_TIME_FORMAT));
    }

    private async Task Ok_Onclick()
    {
        var dateTimeFormat = await _datePicker.Convert(PickerDateTime, Format);
        var dateTime = await _datePicker.Convert(PickerDateTime, DATE_TIME_FORMAT);
        await InputDate(dateTime, dateTimeFormat);
        _datePickerCardDisplay = "none";
    }

    public async Task Refresh()
    {
        var dateTimeFormat = await _datePicker.Convert(CurrentDateTime, Format);
        var dateTime = await _datePicker.Convert(CurrentDateTime, DATE_TIME_FORMAT);
        await InputDate(dateTime, dateTimeFormat);
        this.StateHasChanged();
    }
}
