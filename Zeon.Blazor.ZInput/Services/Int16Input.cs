using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class Int16Input : Input<Int16>
{
    internal override Int16 Convert(string value)
    {
        return System.Convert.ToInt16(value);
    }

    internal override string Get(Int16 value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out Int16 result)
    {
        return Int16.TryParse(value, out result);
    }
}

