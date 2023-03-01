using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class BoolInput : Input<bool>
{
    internal override bool Convert(string value)
    {
        return System.Convert.ToBoolean(value);
    }

    internal override string Get(bool value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool TryParse(string value, out bool result)
    {
        return bool.TryParse(value, out result);
    }
}

