using Microsoft.AspNetCore.Components;


namespace Zeon.Blazor.ZCheckBoxButton;

public partial class ZCheckBoxButton : ComponentBase
{
    private bool _value = false;

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


    [Parameter]
    public bool Value
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
    public EventCallback<bool> OnValueChanged { get; set; }



}

