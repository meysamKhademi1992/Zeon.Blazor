using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class DecimalInput : Input<decimal>
{
    private const string INPUT_TYPE = "text";
    internal override string InputType { get; set; } = INPUT_TYPE;
    internal override decimal Convert(string value)
    {
        return System.Convert.ToDecimal(value);
    }

    internal override string Get(decimal value, string? format)
    {
        return string.Format(format ?? "{0}", System.Convert.ToDecimal(value));
    }

    internal override bool IsValid(string value, out decimal result)
    {
        return decimal.TryParse(value, out result);
    }

    internal async override Task<(bool isValid, string message)> Validate(Func<decimal, Task<(bool isValid, string message)>>? validate, decimal value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}
