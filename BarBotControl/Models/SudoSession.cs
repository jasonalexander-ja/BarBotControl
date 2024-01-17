namespace BarBotControl.Models;

public class SudoSession
{
    public string SessionKey { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Expirey { get; set; }

    public SudoSession(string userName, string sessionKey, DateTime expirey) 
    { 
        SessionKey = sessionKey;
        UserName = userName;
        Expirey = expirey;
    }
}
