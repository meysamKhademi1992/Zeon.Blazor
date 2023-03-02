using Zeon.Blazor.ZInput.Abstraction;

namespace Zeon.Blazor.ZInput.Services;

public class Int64Input : Input<long>
{
    internal override Int64 Convert(string value)
    {
        return System.Convert.ToInt64(value);
    }

    internal override string Get(long value, string? format)
    {
        return string.Format(format ?? "{0}", value);
    }

    internal override bool IsValid(string value, out long result)
    {
        return long.TryParse(value, out result);
    }
    internal async override Task<(bool isValid, string message)> Validate(Func<Int64, Task<(bool isValid, string message)>>? validate, Int64 value)
    {
        if (validate is not null)
            return await validate.Invoke(value);

        return (true, string.Empty);
    }
}
