using Microsoft.AspNetCore.Components;


namespace Zeon.Blazor.ZCheckBoxButton;

public partial class ZCheckBoxButton : ComponentBase
{
    private bool _value = false;


    protected override void OnInitialized()
    {
        Value = DefaultValue;
        base.OnInitialized();
    }
    [Parameter, EditorRequired]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Default : btn-outline-primary
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = "btn-outline-primary";

    [Parameter]
    public string? CssIcon { get; set; }

    [Parameter]
    public string Text { get; set; } = null!;

    [Parameter]
    public string Width { get; set; } = "100%";

    [Parameter]
    public string Height { get; set; } = "auto";

    private bool Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value == value) return;
            _value = value;
            InvokeAsync(async () => await OnValueChanged.InvokeAsync(_value));
        }
    }

    [Parameter]
    public bool DefaultValue { get; set; }

    [Parameter]
    public EventCallback<bool> OnValueChanged { get; set; }



}

