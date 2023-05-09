
using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZInput.Abstraction;
using Zeon.Blazor.ZInput.Services;

namespace Zeon.Blazor.ZInput;

public partial class ZInput<Type> : ComponentBase where Type : IEquatable<Type>
{
    private readonly Input<Type> _input;
    private Type? _value;
    private (bool isValid, string message) _validation;
    protected override void OnInitialized()
    {
        if (DefaultValue is not null)
        {
            OnInput = DefaultValue.ToString()!;
        }
        base.OnInitialized();
    }

    public (bool isValid, string message) IsValid
    {
        get
        {
            this.InvokeAsync(async () => await CheckValidation(_value));
            return _validation;
        }
    }

    [Parameter]
    public string? Id { get; set; } = null;

    [Parameter]
    public string? Format { get; set; }

    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public Type? DefaultValue { get; set; }

    [Parameter]
    public EventCallback<Type> OnValueChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; } = false;

    [Parameter]
    public Func<Type, Task<(bool isValid, string message)>>? Validate { get; set; }

    public ZInput()
    {
        _validation = (isValid: true, message: string.Empty);
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
            _ => throw new NullReferenceException(string.Concat(typeName, " Type Not Found ")),
        };
    }

    private string OnInput
    {
        get => _input.Get(_value, Format);

        set
        {
            if (_input.IsValid(value, out Type result))
            {
                var valueConverted = _input.Convert(value);
                this.InvokeAsync(async () => await CheckValidation(valueConverted));
                if (_validation.isValid)
                {
                    var valueFormated = _input.Convert(_input.Get(valueConverted, Format));
                    if (_value?.Equals(valueConverted) ?? false) return;
                    _value = valueConverted;
                    this.InvokeAsync(async () => await OnValueChanged.InvokeAsync(valueFormated));
                }
            }
        }
    }

    private async Task CheckValidation(Type? value)
    {
        _validation = await _input.Validate(Validate, value);
    }

    public void SetValue(string value)
    {
        OnInput = value;
        Refresh();
    }
    public void Refresh()
    {
        this.StateHasChanged();
    }
}
