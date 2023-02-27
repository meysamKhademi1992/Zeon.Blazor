

namespace Zeon.Blazor.ZInput.Abstraction;

public abstract class Input<Type>
{
    public abstract Type Convert(Type value);
}

