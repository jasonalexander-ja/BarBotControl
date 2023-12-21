using BarBotControl.Config;

namespace BarBotControl.Services;

public class SessionService
{
    private Dictionary<string, DateTime> Sessions = new Dictionary<string, DateTime>();

    private readonly SudoUserConfig Config;

    public SessionService(SudoUserConfig config)
    {
        Config = config;
    }

    public string AddSession()
    {
        string id = Guid.NewGuid().ToString();
        DateTime timeout = DateTime.Now.AddMinutes(Config.SessionMinutes);
        Sessions.Add(id, timeout);
        return id;
    }

    public bool CheckSession(string id)
    {
        if (!Sessions.TryGetValue(id, out var timeout)) 
            return false;

        if (DateTime.Now > timeout)
        {
            Sessions.Remove(id);
            throw new TimedOutException("Session timed out.");
        }

        return true;
    }
}

class TimedOutException : Exception
{
    public TimedOutException(string msg) : base(msg) { }
}
