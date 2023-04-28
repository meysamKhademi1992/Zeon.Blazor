using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class DoubleInput : Input<double>
{
    private const string INPUT_TYPE = "text";
    internal override string InputType { get; set; } = INPUT_TYPE;
    internal override double Convert(string value)
    {
        return System.Convert.ToDouble(value);
    }

    internal override string Get(double value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool IsValid(string value, out double result)
    {
        return double.TryParse(value, out result);
    }
    internal async override Task<(bool isValid, string message)> Validate(Func<double, Task<(bool isValid, string message)>>? validate, double value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}
