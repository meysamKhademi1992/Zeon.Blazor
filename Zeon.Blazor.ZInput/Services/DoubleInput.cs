using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class DoubleInput : Input<double>
{
    internal override double Convert(string value)
    {
        return System.Convert.ToDouble(value);
    }

    internal override string Get(double value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out double result)
    {
        return double.TryParse(value, out result);
    }
}
