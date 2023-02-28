
using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZInput.Abstraction;
using Zeon.Blazor.ZInput.Services;

namespace Zeon.Blazor.ZInput;

public partial class ZInput<Type> : ComponentBase where Type : IEquatable<Type>
{
    private const string DEFAULT_INPUT_TYPE = "decimal";

    private readonly Input<Type> _input;
    private Type? _value;
    private string _eventChange = "onchange";

    [Parameter]
    public string Id { get; set; } = null!;

    [Parameter]
    public Type? DefaultValue
    {
        get => _value;
        set
        {
            _value = value;
        }
    }

    [Parameter]
    public EventCallback<Type> OnValueChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; } = false;

    [Parameter]
    public bool WithType { get; set; } = true;

    protected override void OnInitialized()
    {
        _eventChange = WithType ? "onchange" : "oninput";
        base.OnInitialized();
    }
    public ZInput()
    {
        _input = GetInputInstance();
    }

    private Input<Type> GetInputInstance()
    {
        string typeName = typeof(Type).Name.ToLower();
        return typeName switch
        {
            "decimal" => (new DecimalInput() as Input<Type>)!,
            "int16" => (new Int16Input() as Input<Type>)!,
            "int32" => (new IntInput() as Input<Type>)!,
            "int64" => (new LongInput() as Input<Type>)!,
            "string" => (new StringInput() as Input<Type>)!,
            _ => throw new NullReferenceException(string.Concat(typeName, " Type Not Found ")),
        };
    }

    public string OnInput
    {
        get => _input.Get(_value);
        set
        {
            if (_input.TryParse(value, out Type result))
            {
                if (_value?.Equals(result) ?? false) return;

                _value = result;
                this.InvokeAsync(async () => await OnValueChanged.InvokeAsync(_value));
            }
        }
    }

}
