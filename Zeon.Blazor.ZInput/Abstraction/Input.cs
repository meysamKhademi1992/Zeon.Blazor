

namespace Zeon.Blazor.ZInput.Abstraction;

public abstract class Input<Type> where Type : IEquatable<Type>
{
    internal abstract Type Convert(string value);
    internal abstract bool TryParse(string value, out Type result);
    internal abstract string Get(Type? value, string? format);
}

