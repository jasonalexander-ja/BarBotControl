namespace BarBotControl.Exceptions.SudoUser;

public class AppHasUsersException : Exception
{
    public AppHasUsersException(string msg) : base(msg) { }
}
