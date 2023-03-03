

namespace Zeon.Blazor.ZInput.Abstraction;

public abstract class Input<Type> where Type : IEquatable<Type>
{
    internal abstract string InputType { get; set; }
    internal abstract Type Convert(string value);
    internal abstract bool IsValid(string value, out Type result);
    internal abstract string Get(Type? value, string? format);
    internal abstract Task<(bool isValid, string message)> Validate(Func<Type, Task<(bool isValid, string message)>>? validate, Type? value);
}

