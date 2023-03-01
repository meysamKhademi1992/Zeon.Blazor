using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class UInt64Input : Input<UInt64>
{
    internal override UInt64 Convert(string value)
    {
        return System.Convert.ToUInt64(value);
    }

    internal override string Get(UInt64 value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out UInt64 result)
    {
        return UInt64.TryParse(value, out result);
    }
}

