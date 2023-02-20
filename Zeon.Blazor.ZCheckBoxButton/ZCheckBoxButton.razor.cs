using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Zeon.Blazor.ZCheckBoxButton;

public partial class ZCheckBoxButton<TModel> : ComponentBase
{
    private bool _value = false;
    private string DisplayName => GetDisplayName(For);


    [Parameter, EditorRequired]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Default : btn-outline-primary
    /// </summary>
    [Parameter]
    public string CheckBoxButtonClass { get; set; } = "btn-outline-primary";

    [Parameter]
    public string? Icon { get; set; }

    [Parameter, EditorRequired]
    public string For { get; set; } = null!;

    [Parameter]
    public string Width { get; set; } = "100%";

    [Parameter]
    public string Height { get; set; } = "100%";


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


    private string GetDisplayName(string propertyName)
    {
        try
        {
            MemberInfo? myProperty = typeof(TModel).GetProperty(propertyName) as MemberInfo;
            var displayNameAttribute = myProperty?.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) as System.ComponentModel.DisplayNameAttribute;
            return displayNameAttribute?.DisplayName ?? propertyName;
        }
        catch
        {
            return propertyName;
        }

    }

}

