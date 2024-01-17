namespace BarBotControl.Exceptions.SudoUser;

public abstract class SignInBaseException : Exception
{
    public SignInBaseException(string message) : base(message) { }
}
