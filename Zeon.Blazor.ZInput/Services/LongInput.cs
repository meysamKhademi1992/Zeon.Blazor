using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class LongInput : Input<long>
{
    internal override long Convert(long value)
    {
        return value;
    }

    internal override string Get(long value)
    {
        return value.ToString();
    }

    internal override bool TryParse(string value, out long result)
    {
        return long.TryParse(value, out result);
    }
}
