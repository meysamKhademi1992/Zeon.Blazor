using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class UInt32Input : Input<UInt32>
{
    private const string INPUT_TYPE = "text";
    internal override string InputType { get; set; } = INPUT_TYPE;
    internal override UInt32 Convert(string value)
    {
        return System.Convert.ToUInt32(value);
    }

    internal override string Get(UInt32 value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool IsValid(string value, out UInt32 result)
    {
        return UInt32.TryParse(value, out result);
    }
    internal async override Task<(bool isValid, string message)> Validate(Func<UInt32, Task<(bool isValid, string message)>>? validate, UInt32 value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}

