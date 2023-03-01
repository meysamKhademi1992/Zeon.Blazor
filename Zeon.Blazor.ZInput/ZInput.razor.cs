
using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZInput.Abstraction;
using Zeon.Blazor.ZInput.Services;

namespace Zeon.Blazor.ZInput;

public partial class ZInput<Type> : ComponentBase where Type : IEquatable<Type>
{
    private readonly Input<Type> _input;
    private Type? _value;

    [Parameter]
    public string Id { get; set; } = null!;

    [Parameter]
    public string? Format { get; set; }

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
            "double" => (new DoubleInput() as Input<Type>)!,
            "int16" => (new Int16Input() as Input<Type>)!,
            "int32" => (new Int32Input() as Input<Type>)!,
            "int64" => (new Int64Input() as Input<Type>)!,
            "uint16" => (new UInt16Input() as Input<Type>)!,
            "uint32" => (new UInt32Input() as Input<Type>)!,
            "uint64" => (new UInt64Input() as Input<Type>)!,
            "string" => (new StringInput() as Input<Type>)!,
            "boolean" => (new BoolInput() as Input<Type>)!,
            _ => throw new NullReferenceException(string.Concat(typeName, " Type Not Found ")),
        };
    }

    public string OnInput
    {
        get => _input.Get(_value, Format);
        set
        {
            if (_input.TryParse(value, out Type result))
            {
                var valueConverted = _input.Convert(value);
                var valueFormated = _input.Convert(_input.Get(valueConverted, Format));
                if (_value?.Equals(valueConverted) ?? false) return;
                _value = valueConverted;
                this.InvokeAsync(async () => await OnValueChanged.InvokeAsync(valueFormated));
            }
        }
    }

}
