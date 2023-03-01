using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class UInt16Input : Input<UInt16>
{
    internal override UInt16 Convert(string value)
    {
        return System.Convert.ToUInt16(value);
    }

    internal override string Get(UInt16 value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out UInt16 result)
    {
        return UInt16.TryParse(value, out result);
    }
}

