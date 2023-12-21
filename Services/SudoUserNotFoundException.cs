namespace BarBotControl.Services;

public class SudoUserNotFoundException : Exception
{
    public SudoUserNotFoundException(string msg) : base(msg) { }
}
