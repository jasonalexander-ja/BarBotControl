namespace BarBotControl.Exceptions.SudoUser;

public class SudoUserNotFoundException : SignInBaseException
{
    public SudoUserNotFoundException(string msg) : base(msg) { }
}
