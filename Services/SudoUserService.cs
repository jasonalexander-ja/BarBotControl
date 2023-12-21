using BarBotControl.Config;
using BarBotControl.Exceptions.SudoUser;
using BarBotControl.Models;

namespace BarBotControl.Services;

public class SudoUserService
{
    private SessionService SessionService;
    private SudoUserAccessService SudoUserAccessService;
    private readonly SudoUserConfig Config;

    public SudoUserService(SessionService sessionService, SudoUserAccessService sudoUserAccessService, SudoUserConfig config)
    {
        SessionService = sessionService;
        SudoUserAccessService = sudoUserAccessService;
        Config = config;
    }

    public async Task<bool> CanStartupSignIn()
    {
        return await SudoUserAccessService.CanStartupSignIn();
    }

    public async Task<string> SignUserIn(string userName, string password)
    {
        var result = await SudoUserAccessService.CheckSignIn(userName, password);
        if (!result) 
        {
            throw new WrongDetailsException("Username or password incorrect. ");
        }
        return SessionService.AddSession();
    }

    public async Task<string> StartupSignIn(string startupPasscode)
    {
        if (!(await CanStartupSignIn()))
        {
            throw new AppHasUsersException("Application already ahs users. ");
        }
        if (Config.StartupPasscode == startupPasscode)
        {
            throw new WrongDetailsException("Incorrect passcode. ");
        }
        return SessionService.AddSession();
    }

    public async Task UpdatePassword(string userName, string password)
    {
        await SudoUserAccessService.UpdatePassword(userName, password);
    }

    public async Task<SudoUserModel> GetUser(string userName)
    {
        var user = await SudoUserAccessService.GetUser(userName);
        return new SudoUserModel(user);
    }

    public async Task<string> AddUser(string userName, string password)
    {
        await SudoUserAccessService.CreateUser(userName, password);
        return SessionService.AddSession();
    }
}
