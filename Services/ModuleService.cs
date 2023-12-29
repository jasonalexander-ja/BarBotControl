using BarBotControl.Config;
using BarBotControl.Exceptions;
using BarBotControl.Exceptions.SudoUser;
using BarBotControl.Models;
using System.Security.Cryptography;
using System.Text;

namespace BarBotControl.Services;

public class ModuleService
{
    private ModuleDataService ModuleDataService;

    public ModuleService(ModuleDataService moduleDataService) 
    { 
        ModuleDataService = moduleDataService;
    }

    public async Task<ModuleModel> CreateModule(ModuleModel model)
    {
        var result = await ModuleDataService.CreateModule(model);
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
        var result = await ModuleDataService.UpdateModule(model);
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
        var result = await ModuleDataService.GetModule(id);
        var resultModel = new ModuleModel(result);
        return resultModel;
    }

    public async Task<ModuleModel> GetModuleWithRelatedData(int id)
    {
        var result = await ModuleDataService.GetModuleWithRelatedData(id);
        var resultModel = new ModuleModel(result);
        return resultModel;
    }

    public async Task<List<ModuleModel>> GetModules()
    {
        var result = await ModuleDataService.GetModules();
        return result.Select(m => new ModuleModel(m)).ToList();
    }

    public async Task DeleteModule(ModuleModel module)
    {
        await ModuleDataService.DeleteModule(module);
    }

    public async Task DeleteModules(IEnumerable<ModuleModel> module)
    {
        await ModuleDataService.DeleteModules(module);
    }
}
