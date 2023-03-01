using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class UInt32Input : Input<UInt32>
{
    internal override UInt32 Convert(string value)
    {
        return System.Convert.ToUInt32(value);
    }

    internal override string Get(UInt32 value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out UInt32 result)
    {
        return UInt32.TryParse(value, out result);
    }
}

