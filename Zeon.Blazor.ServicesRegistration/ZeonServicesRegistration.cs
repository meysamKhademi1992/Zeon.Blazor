using Microsoft.Extensions.DependencyInjection;
using Zeon.Blazor.ZDateTimePicker.Abstractions;
using Zeon.Blazor.ZDateTimePicker.Services;

namespace Zeon.Blazor.Services;

public static class ZeonServices
{
    public static void AddZeonServices(this IServiceCollection services)
    {
        services.AddScoped<JSRuntime.ElementHelper>();
    }
    public static void AddZeonDatePickerServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeParser, DateTimeParserService>();
    }
}

