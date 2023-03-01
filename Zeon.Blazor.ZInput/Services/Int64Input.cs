using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class Int64Input : Input<long>
{
    internal override Int64 Convert(string value)
    {
        return System.Convert.ToInt64(value);
    }

    internal override string Get(long value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out long result)
    {
        return long.TryParse(value, out result);
    }
}
