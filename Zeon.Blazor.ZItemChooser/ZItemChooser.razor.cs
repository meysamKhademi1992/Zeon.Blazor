using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Zeon.Blazor.ZItemChooser;

public partial class ZItemChooser<TModel, KeyType> : ComponentBase where TModel : class where KeyType : IEquatable<KeyType>
{
    private bool _isloading = false;
    private bool _showItems = false;
    private string _displayValue = "";
    private string _displayKey = "";
    private string _value = "";
    private string _inputId = "";
    private string _listId = "";
    private TimeSpan _span;
    private Dictionary<KeyType, string> _dataSource;
    private readonly Dictionary<BehaviorModeAfterItemSelection, SelectedItemTypesDelegate> _setSelectedItemTypes;
    private delegate Task SelectedItemTypesDelegate(int index);

    /// <summary>
    /// Label Or Set Property Name For Show Display Name Property Attribute
    /// </summary>
    [Parameter, EditorRequired]
    public string For { get; set; } = null!;
    /// <summary>
    /// Component Name 
    /// </summary>
    [Parameter, EditorRequired]
    public string Name { get; set; } = null!;

    [Parameter]
    public Func<string, Task<Dictionary<KeyType, string>>> FetchData { get; set; } = null!;
    /// <summary>
    /// Event Callback After Selected Item Or Changed
    /// </summary>
    [Parameter]
    public EventCallback<KeyType> OnKeyChanged { get; set; }

    [Parameter]
    public string KeyDisplayText { get; set; } = "شناسه";

    [Parameter]
    public string OddItemColor { get; set; } = "#f5f5f5";

    [Parameter]
    public string EvenItemColor { get; set; } = "#f1f1f1";

    [Parameter]
    public string NotFoundRecordText { get; set; } = "رکوردی یافت نشد!";

    [Parameter]
    public string NotFoundRecordBackgroundColor { get; set; } = "lightblue";
    /// <summary>
    /// Use Icon Classes Like fa fa-plus
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    ///  Waiting Time To Receive Data After Typing Default = 750 ms
    /// </summary>
    [Parameter]
    public int WaitingTimeTyping { get; set; } = 750;

    [Parameter]
    public bool ShowSelectedKey { get; set; } = false;

    [Parameter]
    public BehaviorModeAfterItemSelection BehaviorModeAfterItemSelection { get; set; } = BehaviorModeAfterItemSelection.Normal;

    [Inject]
    protected JSRuntime.ElementHelper ElementHelper { get; set; } = null!;

    public ZItemChooser()
    {
        _dataSource = new();
        _setSelectedItemTypes = new Dictionary<BehaviorModeAfterItemSelection, SelectedItemTypesDelegate>();
        _setSelectedItemTypes.Add(BehaviorModeAfterItemSelection.Normal, SetSelectedItemNormal);
        _setSelectedItemTypes.Add(BehaviorModeAfterItemSelection.Eventable, SetSelectedItemEventable);
    }

    protected override void OnInitialized()
    {
        _inputId = Name + "Input";
        _listId = Name + "ListItems";
        base.OnInitialized();
    }


    private async Task OnInput(ChangeEventArgs e)
    {
        string value = e.Value?.ToString()?.Trim() ?? "";
        bool hasValue = value?.ToString()?.Length > 0;
        if (hasValue)
        {
            _value = value?.ToString() ?? "";

            if (DateTime.Now.TimeOfDay >= _span)
            {
                _span = DateTime.Now.AddMilliseconds(WaitingTimeTyping).TimeOfDay;

                await Task.Factory.StartNew(() => WaitingCompleteTypeing());
                if (!string.IsNullOrEmpty(_displayKey))
                    await SetSelectedKey(default(KeyType));
            }
            else
            {
                _span = DateTime.Now.AddMilliseconds(WaitingTimeTyping).TimeOfDay;
            }

        }
        else
        {
            _showItems = false;

            if (!string.IsNullOrEmpty(_displayKey))
                await SetSelectedKey(default(KeyType));
        }
    }
    private async void WaitingCompleteTypeing()
    {
        bool isBreak = false;
        while (true)
        {
            if (DateTime.Now.TimeOfDay >= _span && !isBreak)
            {
                isBreak = true;
                await SendFetchDataRequestAsync();
                break;
            }
        }
    }

    private async Task SendFetchDataRequestAsync()
    {
        _isloading = true;
        _dataSource = await FetchData.Invoke(_value);
        _isloading = false;
        _showItems = true;
        await this.InvokeAsync(() => this.StateHasChanged());
    }

    private async Task SetSelectedKey(KeyType? key)
    {
        await OnKeyChanged.InvokeAsync(key);
        _displayKey = key?.ToString() ?? "";
    }
    private void SetDisplayValue(string value)
    {
        _displayValue = value;
    }

    private async Task ItemOnKeyPress(KeyboardEventArgs e, int index)
    {
        if (e.Key == "Enter")
        {
            await SetSelectedIndex(index);
        }
        else if (e.Key == "ArrowDown")
        {
            var elementId = Name + (_dataSource.Count > index ? index + 1 : 1);
            await ElementHelper.FocusElementById(elementId);
            await ElementHelper.ScrollToElementById(elementId, true);
        }
        else if (e.Key == "ArrowUp")
        {
            var elementId = Name + (index > 1 ? index - 1 : _dataSource.Count);
            await ElementHelper.FocusElementById(elementId);
            await ElementHelper.ScrollToElementById(elementId, false);
        }
    }
    private async Task InputOnKeyDown(KeyboardEventArgs e)
    {
        if (_showItems)
        {
            if (e.Key == "Enter")
            {
                await SetSelectedIndex(1);
            }
            else if (e.Key == "ArrowDown")
            {
                var elementId = Name + ("1");
                await ElementHelper.FocusElementById(elementId);
            }
            else if (e.Key == "ArrowUp")
            {
                var elementId = Name + (_dataSource.Count.ToString());
                await ElementHelper.FocusElementById(elementId);
            }
        }
    }
    private async Task ItemOnDblClick(MouseEventArgs e, int index)
    {
        await SetSelectedIndex(index);
    }

    private async Task SetSelectedIndex(int index)
    {
        await _setSelectedItemTypes.Single(q => q.Key == BehaviorModeAfterItemSelection).Value.Invoke(index);
    }
    private async Task SetSelectedItemNormal(int index)
    {
        var key = default(KeyType);
        string value = "";
        if (_dataSource.Count > 0 && index > 0 && _dataSource.Count >= index)
        {
            var item = _dataSource.ElementAt(index - 1);
            key = item.Key;
            value = item.Value.Trim();
        }
        await SetSelectedKey(key);
        SetDisplayValue(value);
        _showItems = false;
    }
    private async Task SetSelectedItemEventable(int index)
    {
        var key = default(KeyType);
        string value = "";
        if (_dataSource.Count > 0 && index > 0 && _dataSource.Count >= index)
        {
            var item = _dataSource.ElementAt(index - 1);
            key = item.Key;
            value = item.Value.Trim();
        }
        await SetSelectedKey(key);
        SetDisplayValue(value);
        _showItems = false;

        Clear();
        await ElementHelper.FocusElementById(_inputId);

    }
    private void Clear()
    {
        _displayValue = "";
        _displayKey = "";
    }
}

public enum BehaviorModeAfterItemSelection
{
    Normal = 1,
    Eventable = 2,
}