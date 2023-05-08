using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Zeon.Blazor.ZItemChooser.Constants;

namespace Zeon.Blazor.ZItemChooser;

public partial class ZItemChooser<KeyType> : ComponentBase where KeyType : IEquatable<KeyType>
{
    private readonly Dictionary<BehaviorModeAfterItemSelection, SelectedItemTypesDelegate> _setSelectedItemTypes;

    private bool _typingStarted = false;
    private bool _showItems = false;
    private bool _isWaiting = false;
    private int _requestCount = 0;
    private string _displayValue = string.Empty;
    private string _value = string.Empty;
    private string _inputId = string.Empty;
    private string _listId = string.Empty;
    private TimeSpan _span;
    private Dictionary<KeyType, string> _dataSource;

    public bool SearchByEnter { get => WaitingTimeTyping <= 0; }

    /// <summary>
    /// Component Name 
    /// </summary>
    [Parameter, EditorRequired]
    public string Name { get; set; } = null!;

    [Parameter, EditorRequired]
    public Func<string, Task<Dictionary<KeyType, string>>> FetchData { get; set; } = null!;

    /// <summary>
    /// Event Callback After Selected Item Or Changed
    /// </summary>
    [Parameter, EditorRequired]
    public EventCallback<KeyType> OnKeyChanged { get; set; }

    [Parameter]
    public string NotFoundRecordText { get; set; } = "رکوردی یافت نشد!";

    /// <summary>
    ///  Waiting Time To Receive Data After Typing Default = 600 ms
    /// </summary>
    [Parameter]
    public UInt16 WaitingTimeTyping { get; set; } = 600;

    [Parameter]
    public BehaviorModeAfterItemSelection BehaviorModeAfterItemSelection { get; set; } = BehaviorModeAfterItemSelection.Normal;

    [Inject]
    protected JSRuntime.ElementHelper ElementHelper { get; set; } = null!;


    private delegate Task SelectedItemTypesDelegate(int index);

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
            _span = DateTime.Now.TimeOfDay.Add(new TimeSpan(0, 0, 0, 0, WaitingTimeTyping));
        }
        else
        {
            _value = string.Empty;
            _showItems = false;
            await SetSelectedKey(default(KeyType));
        }
        if (!_typingStarted && SearchByEnter == false)
        {
            await new TaskFactory().StartNew(async () => await WaitingCompleteTypeing());
        }

    }

    private async Task WaitingCompleteTypeing()
    {
        _typingStarted = true;

        while (true)
        {
            if (DateTime.Now.TimeOfDay >= _span && _typingStarted)
            {
                _typingStarted = false;
                await SendFetchDataRequestAsync();
                break;
            }
        }
    }

    private async Task SendFetchDataRequestAsync()
    {
        if (!string.IsNullOrWhiteSpace(_value))
        {
            _showItems = true;
            _isWaiting = true;
            _requestCount++;
            _dataSource = await FetchData.Invoke(_value);
            _requestCount--;
            _isWaiting = _requestCount > 0;
            await this.InvokeAsync(() => this.StateHasChanged());
        }
    }

    private async Task SetSelectedKey(KeyType? key)
    {
        await OnKeyChanged.InvokeAsync(key);
    }
    private async Task ItemOnKeyPress(KeyboardEventArgs e, int index)
    {
        if (e.Key == "Enter")
        {
            await SetSelectedIndex(index);
        }
        else if (e.Key == "ArrowDown")
        {
            var itemId = Name + (_dataSource.Count > index ? index + 1 : 1);
            await ElementHelper.FocusElementById(itemId);
            await ElementHelper.ScrollToElementById(_listId, itemId);
        }
        else if (e.Key == "ArrowUp")
        {
            var itemId = Name + (index > 1 ? index - 1 : _dataSource.Count);
            await ElementHelper.FocusElementById(itemId);
            await ElementHelper.ScrollToElementById(_listId, itemId);
        }
        else if (e.Key == "Escape")
            _showItems = false;

    }

    private async Task InputOnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            if (SearchByEnter)
                await SendFetchDataRequestAsync();
            else if (_showItems)
                await SetSelectedIndex(1);
        }
        if (_showItems)
        {
            if (e.Key == "ArrowDown")
            {
                var elementId = Name + ("1");
                await ElementHelper.FocusElementById(elementId);
            }
            else if (e.Key == "ArrowUp")
            {
                var elementId = Name + (_dataSource.Count.ToString());
                await ElementHelper.FocusElementById(elementId);
            }
            else if (e.Key == "Escape")
                _showItems = false;

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
    }
    public void SetDisplayValue(string value)
    {
        _displayValue = value;
    }

}
