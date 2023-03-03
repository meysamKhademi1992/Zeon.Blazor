using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class Int16Input : Input<Int16>
{
    private const string INPUT_TYPE = "number";
    internal override string InputType { get; set; } = INPUT_TYPE;
    internal override Int16 Convert(string value)
    {
        return System.Convert.ToInt16(value);
    }

    internal override string Get(Int16 value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool IsValid(string value, out Int16 result)
    {
        return Int16.TryParse(value, out result);
    }
    internal async override Task<(bool isValid, string message)> Validate(Func<Int16, Task<(bool isValid, string message)>>? validate, Int16 value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}

