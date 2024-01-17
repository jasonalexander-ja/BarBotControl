namespace BarBotControl.Exceptions.SudoUser;

public class SudoUserExistsException : Exception
{
    public SudoUserExistsException(string msg) : base(msg) { }
}
