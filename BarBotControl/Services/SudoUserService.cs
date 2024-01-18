using BarBotControl.Config;
using BarBotControl.Exceptions;
using BarBotControl.Exceptions.SudoUser;
using BarBotControl.Models;
using BarBotControl.Services.Accessors;
using System.Security.Cryptography;
using System.Text;

namespace BarBotControl.Services;

public class SudoUserService
{
    private SessionService _sessionService;
    private SudoUserAccessor _sudoUserAccessor;
    private readonly SudoUserConfig _config;

    private const int keySize = 64;
    private const int iterations = 350000;
    private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public SudoUserService(SessionService sessionService, 
        SudoUserAccessor sudoUserAccessor, 
        SudoUserConfig config)
    {
        _sessionService = sessionService;
        _sudoUserAccessor = sudoUserAccessor;
        _config = config;
    }

    public async Task<bool> CanStartupSignIn()
    {
        return await _sudoUserAccessor.CanStartupSignIn();
    }

    public async Task<SudoSession> SignUserIn(string userName, string password)
    {
        try
        {
            var user = await _sudoUserAccessor.GetUser(userName);
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new WrongDetailsException("Username or password incorrect. ");
            }
            var sessionKey = _sessionService.AddSession(out var expirey);
            var session = new SudoSession(user.UserName, sessionKey, expirey);
            return session;
        }
        catch (ObjectNotFoundException ex)
        {
            throw new SudoUserNotFoundException(ex.Message);
        }
    }

    public async Task<SudoSession> StartupSignIn(string startupPasscode)
    {
        if (!(await CanStartupSignIn()))
        {
            throw new AppHasUsersException("Application already has users. ");
        }
        if (_config.StartupPasscode != startupPasscode)
        {
            throw new WrongDetailsException("Incorrect passcode. ");
        }
        var sessionKey = _sessionService.AddSession(out var expirey);
        var session = new SudoSession("Admin", sessionKey, expirey);
        return session;
    }

    public async Task UpdatePassword(string userName, string password)
    {
        try
        {
            var passwordHash = HashPasword(password, out var salt);
            await _sudoUserAccessor.UpdatePassword(userName, passwordHash, salt);
        }
        catch (ObjectNotFoundException ex) 
        {
            throw new SudoUserNotFoundException(ex.Message);
        }
    }

    public async Task<SudoUserModel> GetUser(string userName)
    {
        try
        {
            var user = await _sudoUserAccessor.GetUser(userName);
            return new SudoUserModel(user);
        }
        catch (ObjectNotFoundException ex) 
        { 
            throw new SudoUserNotFoundException(ex.Message);
        }
    }

    public async Task<List<SudoUserModel>> GetUsers()
    {
        var users = await _sudoUserAccessor.GetUsers();
        return users.Select(u => new SudoUserModel(u)).ToList();
    }

    public async Task DeleteUser(string userName)
    {
        try
        {
            await _sudoUserAccessor.DeleteUser(userName);
        }
        catch (ObjectNotFoundException ex) 
        {
            throw new SudoUserNotFoundException(ex.Message);
        }
    }

    public async Task DeleteUsers(IEnumerable<SudoUserModel> sudoUsers)
    {
        try
        {
            var userNames = sudoUsers.Select(u => u.UserName);
            await _sudoUserAccessor.DeleteUsers(userNames);
        }
        catch (ObjectNotFoundException ex)
        {
            throw new SudoUserNotFoundException(ex.Message);
        }
    }

    public async Task<SudoUserModel> AddUser(string userName, string password)
    {
        var passwordHash = HashPasword(password, out var salt);
        var user = await _sudoUserAccessor.CreateUser(userName, passwordHash, salt);
        return new SudoUserModel(user);
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
