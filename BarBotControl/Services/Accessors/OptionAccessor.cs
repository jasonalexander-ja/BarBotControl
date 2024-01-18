using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using BarBotControl.Data;
using BarBotControl.Data.Models;


namespace BarBotControl.Services.Accessors;

public class OptionAccessor
{
    private readonly AppDbContext _context;

    public OptionAccessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Option> AddOption(OptionModel opt)
    {
        var optModel = new Option(opt);
        _context.Options.Add(optModel);
        await _context.SaveChangesAsync();
        return optModel;
    }

    public async Task<Option> GetOption(OptionModel opt)
    {
        var optionModel = await _context.Options.FirstOrDefaultAsync(
            o => o.OptionId == opt.OptionId && o.ModuleId == opt.ModuleId);
        if (optionModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find option `{opt.Name}` (Id {opt.OptionId}) on module. ");
        }
        return optionModel;
    }

    public async Task<List<Option>> GetActiveOptions()
    {
        return await _context.Options
            .Where(opt => opt.IsActive && opt.Module.IsActive)
            .ToListAsync();
    }

    public async Task<List<Option>> GetOptionsForModule(int moduleId)
    {
        return await _context.Options.Where(opt => opt.ModuleId == moduleId).ToListAsync();
    }

    public async Task<Option> UpdateOption(OptionModel opt)
    {
        var optModel = await GetOption(opt);
        optModel.OptionValue = opt.OptionValue;
        optModel.IsActive = opt.IsActive;
        optModel.Name = opt.Name;
        await _context.SaveChangesAsync();
        return optModel;
    }

    public async Task DeleteOption(OptionModel opt)
    {
        var optionModel = await GetOption(opt);
        _context.Options.Remove(optionModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOptions(IEnumerable<OptionModel> options)
    {
        foreach (var opt in options)
        {
            var optionModel = await GetOption(opt);
            _context.Options.Remove(optionModel);
        }
        await _context.SaveChangesAsync();
    }
}
