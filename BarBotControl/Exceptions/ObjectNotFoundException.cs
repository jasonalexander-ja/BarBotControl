namespace BarBotControl.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string msg) : base(msg) { }
}
