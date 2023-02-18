﻿using Microsoft.Extensions.DependencyInjection;

namespace Zeon.Blazor.Services
{
    public static class ZeonServices
    {
        public static void AddZeonServices(this IServiceCollection services)
        {
            services.AddScoped<JSRuntime.ElementHelper>();
        }
    }
}
