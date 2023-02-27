using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class DecimalInput : Input<decimal>
{
    internal override decimal Convert(decimal value)
    {
        return Math.Round(value, 2);  
    }

    internal override string Get(decimal value)
    {
        return true ? string.Format("{0:n}", value) : string.Format("{0:n0}", value);
    }

    internal override bool TryParse(string value, out decimal result)
    {
        return decimal.TryParse(value, out result);
    }
}
