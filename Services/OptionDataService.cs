using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using BarBotControl.Data;


namespace BarBotControl.Services;

public class OptionDataService
{
    private readonly AppDbContext Context;

    public OptionDataService(AppDbContext context)
    {
        Context = context;
    }

    public async Task<Option> AddOption(OptionModel opt)
    {
        var optModel = new Option(opt);
        Context.Options.Add(optModel);
        await Context.SaveChangesAsync();
        return optModel;
    }

    public async Task<Option> GetOption(OptionModel opt)
    {
        var optionModel = await Context.Options.FirstOrDefaultAsync(
            o => o.OptionId == opt.OptionId && o.ModuleId == opt.ModuleId);
        if (optionModel is null) 
        {
            throw new ObjectNotFoundException($"Cannot find option `{opt.Name}` (Id {opt.OptionId}) on module. ");
        }
        return optionModel;
    }

    public async Task<List<Option>> GetOptionsForModule(int moduleId)
    {
        return await Context.Options.Where(opt => opt.ModuleId == moduleId).ToListAsync();
    }

    public async Task<Option> UpdateOption(OptionModel opt)
    {
        var optModel = await GetOption(opt);
        optModel.OptionValue = opt.OptionValue;
        optModel.IsActive = opt.IsActive;
        optModel.Name = opt.Name;
        await Context.SaveChangesAsync();
        return optModel;
    }

    public async Task DeleteOption(OptionModel opt)
    {
        var optionModel = await GetOption(opt);
        Context.Options.Remove(optionModel);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteOptions(IEnumerable<OptionModel> options)
    {
        foreach (var opt in options)
        {
            var optionModel = await GetOption(opt);
            Context.Options.Remove(optionModel);
        }
        await Context.SaveChangesAsync();
    }
}
