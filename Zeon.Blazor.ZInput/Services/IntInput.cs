using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class IntInput : Input<int>
{
    internal override int Convert(int value)
    {
        return value;
    }

    internal override string Get(int value)
    {
        return value.ToString();
    }

    internal override bool TryParse(string value, out int result)
    {
        return int.TryParse(value, out result);
    }
}

