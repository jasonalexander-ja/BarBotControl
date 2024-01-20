using BarBotControl.Config;
using BarBotControl.Services;

namespace BarBotControl.Extensions;

public static class SudoUserConfigExtensions
{
    public static WebApplicationBuilder AddSudoUserService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(
            builder.Configuration.GetRequiredSection("SudoUserConfig")
                .Get<SudoUserConfig>());
        builder.Services.AddSingleton<SessionService>();

        return builder;
    }
}
