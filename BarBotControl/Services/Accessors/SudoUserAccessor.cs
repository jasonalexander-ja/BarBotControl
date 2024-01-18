using System.Security.Cryptography;
using BarBotControl.Data;
using BarBotControl.Config;
using BarBotControl.Exceptions.SudoUser;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BarBotControl.Exceptions;
using BarBotControl.Data.Models;

namespace BarBotControl.Services.Accessors;

public class SudoUserAccessor
{
    private readonly AppDbContext _context;

    public SudoUserAccessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SudoUser> CreateUser(string userName, string passwordHash, byte[] salt)
    {
        bool exists = await _context.SudoUsers.AnyAsync(u => u.UserName == userName);
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
        await _context.SudoUsers.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<SudoUser> UpdatePassword(string userName, string passwordHash, byte[] salt)
    {
        var user = await GetUser(userName);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> CanStartupSignIn()
    {
        return !await _context.SudoUsers.AnyAsync();
    }

    public async Task<SudoUser> GetUser(string userName)
    {
        var result = await _context.SudoUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        if (result == null)
        {
            throw new ObjectNotFoundException($"No user by {userName}. ");
        }
        return result;
    }

    public async Task<List<SudoUser>> GetUsers()
    {
        return await _context.SudoUsers.ToListAsync();
    }

    public async Task DeleteUser(string userName)
    {
        var user = await GetUser(userName);
        _context.SudoUsers.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsers(IEnumerable<string> userNames)
    {
        foreach (var userName in userNames)
        {
            var user = await GetUser(userName);
            _context.SudoUsers.Remove(user);
        }
        await _context.SaveChangesAsync();
    }
}
