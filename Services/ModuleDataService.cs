using System.Security.Cryptography;
using BarBotControl.Data;
using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;


namespace BarBotControl.Services;

public class ModuleDataService
{
    private readonly AppDbContext Context;

    public ModuleDataService(AppDbContext context)
    {
        Context = context;
    }

    public async Task<Module> CreateModule(ModuleModel module)
    {
        var model = new Module()
        {
            Name = module.Name,
            Channel = module.Channel,
            IsActive = module.IsActive,
        };
        Context.Modules.Add(model);
        await Context.SaveChangesAsync();
        return model;
    }

    public async Task<Module> GetModule(int id)
    {
        var result = await Context.Modules.FirstOrDefaultAsync(u => u.ModuleId == id);
        if (result == null) 
        {
            throw new ObjectNotFoundException($"Can't find module with ID {id}");
        }
        return result;
    }

    public async Task<Module> GetModuleWithRelatedData(int id)
    {
        var result = await Context.Modules
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
        return await Context.Modules.ToListAsync();
    }

    public async Task<Module> UpdateModule(ModuleModel module)
    {
        var model = await GetModule(module.ModuleId);
        model.Name = module.Name;
        model.Channel = module.Channel;
        model.IsActive = module.IsActive;
        await Context.SaveChangesAsync();
        return model;
    }

    public async Task DeleteModule(ModuleModel module)
    {
        var model = await Context.Modules.FirstOrDefaultAsync(m => m.ModuleId == module.ModuleId);
        if (model == null)
        {
            throw new ObjectNotFoundException($"Can't find module with ID {module.ModuleId}");
        }
        Context.Modules.Remove(model);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteModules(IEnumerable<ModuleModel> modules)
    {
        foreach (var m in modules)
        {
            var user = await GetModule(m.ModuleId);
            Context.Modules.Remove(user);
        }
        await Context.SaveChangesAsync();
    }
}
