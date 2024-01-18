namespace BarBotControl.Data.Models;

public class SudoUser
{
    public int SudoUserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public byte[] PasswordSalt { get; set; } = new byte[] { };
}
