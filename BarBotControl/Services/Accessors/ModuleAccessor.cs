using System.Security.Cryptography;
using BarBotControl.Data;
using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using BarBotControl.Data.Models;


namespace BarBotControl.Services.Accessors;

public class ModuleAccessor
{
    private readonly AppDbContext _context;

    public ModuleAccessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Module> CreateModule(ModuleModel module)
    {
        var model = new Module()
        {
            Name = module.Name,
            Channel = module.Channel,
            IsActive = module.IsActive,
        };
        _context.Modules.Add(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<Module> GetModule(int id)
    {
        var result = await _context.Modules.FirstOrDefaultAsync(u => u.ModuleId == id);
        if (result == null)
        {
            throw new ObjectNotFoundException($"Can't find module with ID {id}");
        }
        return result;
    }

    public async Task<List<Module>> GetActiveModulesWithOptions()
    {
        return await _context.Modules
            .Where(m => m.IsActive == true)
            .Include(m => m.Options.Where(o => o.IsActive == true))
            .ToListAsync();
    }

    public async Task<Module> GetModuleWithRelatedData(int id)
    {
        var result = await _context.Modules
            .Include(m => m.Options)
            .Include(m => m.ErrorTypes)
            .FirstOrDefaultAsync(u => u.ModuleId == id);
        if (result == null)
        {
            throw new ObjectNotFoundException($"Can't find module with ID {id}");
        }
        return result;
    }

    public async Task<List<Module>> GetModules()
    {
        return await _context.Modules.ToListAsync();
    }

    public async Task<List<Module>> GetActiveModules()
    {
        return await _context.Modules.Where(m => m.IsActive).ToListAsync();
    }

    public async Task<Module> UpdateModule(ModuleModel module)
    {
        var model = await GetModule(module.ModuleId);
        model.Name = module.Name;
        model.Channel = module.Channel;
        model.IsActive = module.IsActive;
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task DeleteModule(ModuleModel module)
    {
        var model = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == module.ModuleId);
        if (model == null)
        {
            throw new ObjectNotFoundException($"Can't find module with ID {module.ModuleId}");
        }
        _context.Modules.Remove(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteModules(IEnumerable<ModuleModel> modules)
    {
        foreach (var m in modules)
        {
            var user = await GetModule(m.ModuleId);
            _context.Modules.Remove(user);
        }
        await _context.SaveChangesAsync();
    }
}
