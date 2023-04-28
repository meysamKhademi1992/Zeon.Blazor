using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

internal class Int32Input : Input<int>
{
    private const string INPUT_TYPE = "text";
    internal override string InputType { get; set; } = INPUT_TYPE;
    internal override int Convert(string value)
    {
        return System.Convert.ToInt32(value);
    }

    internal override string Get(int value, string? format)
    {
        return value.ToString(format);
    }

    internal override bool IsValid(string value, out int result)
    {
        return int.TryParse(value, out result);
    }
    internal async override Task<(bool isValid, string message)> Validate(Func<Int32, Task<(bool isValid, string message)>>? validate, Int32 value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}

