using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class DecimalInput : Input<decimal>
{
    internal override decimal Convert(string value)
    {
        return System.Convert.ToDecimal(value);
    }

    internal override string Get(decimal value, string? format)
    {
        return string.Format(format ?? "{0}", System.Convert.ToDecimal(value));
    }

    internal override bool TryParse(string value, out decimal result)
    {
        return decimal.TryParse(value, out result);
    }
}
