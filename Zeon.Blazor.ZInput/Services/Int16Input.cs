using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class Int16Input : Input<Int16>
{
    internal override Int16 Convert(Int16 value)
    {
        return value;
    }

    internal override string Get(Int16 value)
    {
        return value.ToString();
    }

    internal override bool TryParse(string value, out Int16 result)
    {
        return Int16.TryParse(value, out result);
    }
}

