using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class IntInput : Input<int>
{
    public override int Convert(int value)
    {
        return value;
    }
}

