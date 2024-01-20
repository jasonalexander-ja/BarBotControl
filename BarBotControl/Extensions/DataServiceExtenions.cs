using BarBotControl.Services.Accessors;
using BarBotControl.Services;

namespace BarBotControl.Extensions;

public static class DataServiceExtenions
{
    public static WebApplicationBuilder AddDataServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SudoUserAccessor>();
        builder.Services.AddScoped<SudoUserService>();

        builder.Services.AddScoped<ModuleAccessor>();
        builder.Services.AddScoped<ModuleService>();

        builder.Services.AddScoped<OptionAccessor>();
        builder.Services.AddScoped<OptionService>();

        builder.Services.AddScoped<ErrorTypeAccessor>();
        builder.Services.AddScoped<ErrorTypeService>();

        builder.Services.AddScoped<SequenceAccessor>();
        builder.Services.AddScoped<SequenceService>();

        builder.Services.AddScoped<SequenceItemAccessor>();
        builder.Services.AddScoped<SequenceItemService>();

        return builder;
    }
}
