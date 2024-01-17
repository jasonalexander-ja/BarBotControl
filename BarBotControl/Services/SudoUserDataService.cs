using System.Security.Cryptography;
using BarBotControl.Data;
using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions.SudoUser;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BarBotControl.Exceptions;

namespace BarBotControl.Services;

public class SudoUserDataService
{
    private readonly AppDbContext Context;

    public SudoUserDataService(AppDbContext context)
    {
        Context = context;
    }

    public async Task<SudoUser> CreateUser(string userName, string passwordHash, byte[] salt)
    {
        bool exists = await Context.SudoUsers.AnyAsync(u => u.UserName == userName);
        if (exists)
        {
            throw new SudoUserExistsException("Already user with this name. ");
        }
        SudoUser user = new SudoUser
        {
            UserName = userName,
            PasswordHash = passwordHash,
            PasswordSalt = salt
        };
        await Context.SudoUsers.AddAsync(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<SudoUser> UpdatePassword(string userName, string passwordHash, byte[] salt)
    {
        var user = await GetUser(userName);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> CanStartupSignIn()
    {
        return !(await Context.SudoUsers.AnyAsync());
    }

    public async Task<SudoUser> GetUser(string userName)
    {
        var result = await Context.SudoUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        if (result == null)
        {
            throw new ObjectNotFoundException($"No user by {userName}. ");
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

    public async Task DeleteUsers(IEnumerable<string> userNames)
    {
        foreach (var userName in userNames)
        {
            var user = await GetUser(userName);
            Context.SudoUsers.Remove(user);
        }
        await Context.SaveChangesAsync();
    }
}
