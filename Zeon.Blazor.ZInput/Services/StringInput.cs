using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class StringInput : Input<string>
{
    private const string INPUT_TYPE = "text";
    internal override string InputType { get; set; } = INPUT_TYPE;
    internal override string Convert(string value)
    {
        return value;
    }

    internal override string Get(string? value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool IsValid(string value, out string result)
    {
        result = value is not null ? value : "";
        return true;
    }
    internal async override Task<(bool isValid, string message)> Validate(Func<string, Task<(bool isValid, string message)>>? validate, string value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}
