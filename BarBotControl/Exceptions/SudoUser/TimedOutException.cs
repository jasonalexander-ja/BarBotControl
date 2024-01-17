namespace BarBotControl.Exceptions.SudoUser;

class TimedOutException : Exception
{
    public TimedOutException(string msg) : base(msg) { }
}
