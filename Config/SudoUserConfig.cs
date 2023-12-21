namespace BarBotControl.Config;


public class SudoUserConfig
{
    public string StartupPasscode { get; set; } = string.Empty;
    public int SessionMinutes { get; set; } = 30;
}
