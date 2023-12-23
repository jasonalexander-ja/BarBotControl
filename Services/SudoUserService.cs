using BarBotControl.Config;
using BarBotControl.Exceptions.SudoUser;
using BarBotControl.Models;
using System.Security.Cryptography;
using System.Text;

namespace BarBotControl.Services;

public class SudoUserService
{
    private SessionService SessionService;
    private SudoUserDataService SudoUserAccessService;
    private readonly SudoUserConfig Config;

    private const int keySize = 64;
    private const int iterations = 350000;
    private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public SudoUserService(SessionService sessionService, SudoUserDataService sudoUserAccessService, SudoUserConfig config)
    {
        SessionService = sessionService;
        SudoUserAccessService = sudoUserAccessService;
        Config = config;
    }

    public async Task<bool> CanStartupSignIn()
    {
        return await SudoUserAccessService.CanStartupSignIn();
    }

    public async Task<SudoSession> SignUserIn(string userName, string password)
    {
        var user = await SudoUserAccessService.GetUser(userName);
        if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt)) 
        {
            throw new WrongDetailsException("Username or password incorrect. ");
        }
        var sessionKey = SessionService.AddSession(out var expirey);
        var session = new SudoSession(user.UserName, sessionKey, expirey);
        return session;
    }

    public async Task<SudoSession> StartupSignIn(string startupPasscode)
    {
        if (!(await CanStartupSignIn()))
        {
            throw new AppHasUsersException("Application already has users. ");
        }
        if (Config.StartupPasscode != startupPasscode)
        {
            throw new WrongDetailsException("Incorrect passcode. ");
        }
        var sessionKey = SessionService.AddSession(out var expirey);
        var session = new SudoSession("Admin", sessionKey, expirey);
        return session;
    }

    public async Task UpdatePassword(string userName, string password)
    {
        var passwordHash = HashPasword(password, out var salt);
        await SudoUserAccessService.UpdatePassword(userName, passwordHash, salt);
    }

    public async Task<SudoUserModel> GetUser(string userName)
    {
        var user = await SudoUserAccessService.GetUser(userName);
        return new SudoUserModel(user);
    }

    public async Task<SudoSession> AddUser(string userName, string password)
    {
        var passwordHash = HashPasword(password, out var salt);
        var user = await SudoUserAccessService.CreateUser(userName, passwordHash, salt);
        var sessionKey = SessionService.AddSession(out var expirey);
        var session = new SudoSession(user.UserName, sessionKey, expirey);
        return session;
    }

    private string HashPasword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }

    private bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}
