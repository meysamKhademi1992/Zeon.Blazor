using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class Int32Input : Input<int>
{
    internal override int Convert(string value)
    {
        return System.Convert.ToInt32(value);
    }

    internal override string Get(int value, string? format)
    {
        return value.ToString(format);
    }

    internal override bool TryParse(string value, out int result)
    {
        return int.TryParse(value, out result);
    }
}

