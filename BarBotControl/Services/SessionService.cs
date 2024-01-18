using BarBotControl.Exceptions.SudoUser;
using BarBotControl.Config;

namespace BarBotControl.Services;

public class SessionService
{
    private Dictionary<string, DateTime> Sessions = new Dictionary<string, DateTime>();

    private readonly SudoUserConfig _config;

    public SessionService(SudoUserConfig config)
    {
        _config = config;
    }

    public string AddSession(out DateTime expirey)
    {
        string id = Guid.NewGuid().ToString();
        DateTime timeout = DateTime.Now.AddMinutes(_config.SessionMinutes);
        Sessions.Add(id, timeout);
        expirey = timeout;
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

    public void RemoveSession(string id) 
    { 
        Sessions.Remove(id); 
    }
}