using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class StringInput : Input<string>
{
    internal override string Convert(string value)
    {
        return value;
    }

    internal override string Get(string? value)
    {
        return value?.Trim() ?? "";
    }

    internal override bool TryParse(string value, out string result)
    {
        result = value is not null ? value : "";
        return true;
    }
}
