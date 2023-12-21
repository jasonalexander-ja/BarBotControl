using System.Security.Cryptography;
using BarBotControl.Data;
using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions.SudoUser;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BarBotControl.Services;

public class SudoUserAccessService
{
    private readonly AppDbContext Context;
    private readonly SudoUserConfig Config;

    private const int keySize = 64;
    private const int iterations = 350000;
    private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public SudoUserAccessService(AppDbContext context, SudoUserConfig config)
    {
        Context = context;
        Config = config;
    }

    public async Task<SudoUser> CreateUser(string userName, string password)
    {
        bool exists = await Context.SudoUsers.AnyAsync(u => u.UserName == userName);
        if (exists)
        {
            throw new SudoUserExistsException("Already user with this name. ");
        }
        var hashed = HashPasword(password, out var salt);
        SudoUser user = new SudoUser
        {
            UserName = userName,
            PasswordHash = hashed,
            PasswordSalt = salt
        };
        await Context.SudoUsers.AddAsync(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> CheckSignIn(string userName, string password)
    {
        var sudoUser = await GetUser(userName);
        return VerifyPassword(password, sudoUser.PasswordHash, sudoUser.PasswordSalt);
    }

    public async Task<SudoUser> UpdatePassword(string userName, string password)
    {
        var user = await GetUser(userName);
        var hashed = HashPasword(password, out var salt);
        user.PasswordHash = hashed;
        user.PasswordSalt = salt;
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> CanStartupSignIn()
    {
        return await Context.SudoUsers.AnyAsync();
    }

    public async Task<SudoUser> GetUser(string userName)
    {
        var result = await Context.SudoUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        if (result == null)
        {
            throw new SudoUserNotFoundException($"No user by {userName}. ");
        }
        return result;
    }

    public async Task<List<SudoUser>> GetUsers()
    {
        return await Context.SudoUsers.ToListAsync();
    }

    public async Task DeleteUser(string userName)
    {
        var user = await GetUser(userName);
        Context.SudoUsers.Remove(user);
        await Context.SaveChangesAsync();
    }

    public async Task<List<SudoUser>> SearchByUserName(string userName)
    {
        return await Context.SudoUsers
            .Where(u => u.UserName.ToLower().Contains(userName.ToLower()))
            .ToListAsync();
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
