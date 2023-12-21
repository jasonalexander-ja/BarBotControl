namespace BarBotControl.Exceptions.SudoUser;

public class WrongDetailsException : Exception
{
    public WrongDetailsException(string msg) : base(msg) { }
}
