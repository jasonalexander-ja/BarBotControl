namespace BarBotControl.Exceptions.SudoUser;

public class WrongDetailsException : SignInBaseException
{
    public WrongDetailsException(string msg) : base(msg) { }
}
