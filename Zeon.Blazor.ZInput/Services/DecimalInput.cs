using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class DecimalInput : Input<decimal>
{
    public override decimal Convert(decimal value)
    {
        return Math.Round(value, 2);  
    }
}
