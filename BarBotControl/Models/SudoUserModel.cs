using BarBotControl.Data.Models;

namespace BarBotControl.Models;

public class SudoUserModel
{
    public string UserName { get; set; } = string.Empty;

    public SudoUserModel() 
    {
        UserName = string.Empty;
    }
    public SudoUserModel(SudoUser user)
    {
        UserName = user.UserName;
    }
}
