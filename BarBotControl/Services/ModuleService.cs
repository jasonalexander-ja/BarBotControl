using BarBotControl.Config;
using BarBotControl.Exceptions;
using BarBotControl.Exceptions.SudoUser;
using BarBotControl.Models;
using BarBotControl.Services.Accessors;
using System.Security.Cryptography;
using System.Text;

namespace BarBotControl.Services;

public class ModuleService
{
    private ModuleAccessor _moduleAccessor;

    public ModuleService(ModuleAccessor Accessor) 
    { 
        _moduleAccessor = Accessor;
    }

    public async Task<ModuleModel> CreateModule(ModuleModel model)
    {
        var result = await _moduleAccessor.CreateModule(model);
        var resultModel = new ModuleModel()
        {
            ModuleId = result.ModuleId,
            Channel = result.Channel,
            Name = result.Name,
            IsActive = result.IsActive,
        };
        return resultModel;
    }

    public async Task<ModuleModel> UpdateModule(ModuleModel model)
    {
        var result = await _moduleAccessor.UpdateModule(model);
        var resultModel = new ModuleModel()
        {
            ModuleId = result.ModuleId,
            Channel = result.Channel,
            Name = result.Name,
            IsActive = result.IsActive,
        };
        return resultModel;
    }

    public async Task<ModuleModel> GetModule(int id)
    {
        var result = await _moduleAccessor.GetModule(id);
        var resultModel = new ModuleModel(result);
        return resultModel;
    }

    public async Task<List<ModuleModel>> GetModules()
    {
        var result = await _moduleAccessor.GetModules();
        return result.Select(m => new ModuleModel(m)).ToList();
    }

    public async Task<List<ModuleModel>> GetActiveModules()
    {
        var result = await _moduleAccessor.GetModules();
        return result.Select(m => new ModuleModel(m)).ToList();
    }

    public async Task DeleteModule(ModuleModel module)
    {
        await _moduleAccessor.DeleteModule(module);
    }

    public async Task DeleteModules(IEnumerable<ModuleModel> module)
    {
        await _moduleAccessor.DeleteModules(module);
    }
}
