
using Microsoft.AspNetCore.Components;
using Zeon.Blazor.ZInput.Abstraction;
using Zeon.Blazor.ZInput.Services;

namespace Zeon.Blazor.ZInput;

public partial class ZInput<Type> : ComponentBase where Type : IEquatable<Type>
{
    private const string DEFAULT_INPUT_TYPE = "string";
    
    private readonly Input<Type> _input;
    private Type _number;
    private string _eventChange = "onchange";

    [Parameter]
    public string Id { get; set; } = null!;

    [Parameter]
    public Type DefaultValue
    {
        get => _number;
        set
        {
            int Types = WithType ? 2 : 0;
            _number = _input.Convert(value);
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
    }

    private Input<Type> GetInputInstance() =>

         nameof(Type) switch
         {
             "decimal" => new DecimalInput() as Input<Type>,
             "int" => new IntInput() as Input<Type>,
             "long" => new DecimalInput() as Input<Type>,
             "string" => new DecimalInput() as Input<Type>,
             "String" => new DecimalInput() as Input<Type>,
             _ => throw new NullReferenceException(string.Concat(nameof(Type), "Type Not Found"))
         };

    public string FormatValue
    {
        get => WithType ? string.Format("{0:n}", _number) : string.Format("{0:n0}", _number);
        set
        {
            Type num = 0;
            if (Type.TryParse(value, out num) || value == "")
            {
                if (_number == num) return;

                _number = num;
                DefaultValue = (_number);
                OnValueChanged.InvokeAsync(_number);
            }
        }
    }

}
