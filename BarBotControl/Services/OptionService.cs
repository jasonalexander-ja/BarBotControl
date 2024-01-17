using BarBotControl.Models;


namespace BarBotControl.Services;

public class OptionService
{
    private readonly OptionDataService OptionDataService;

    public OptionService(OptionDataService optionDataService)
    {
        OptionDataService = optionDataService;
    }

    public async Task<OptionModel> AddOption(OptionModel opt)
    {
        return new OptionModel(await OptionDataService.AddOption(opt));
    }

    public async Task<OptionModel> GetOption(OptionModel opt)
    {
        return new OptionModel(await OptionDataService.GetOption(opt));
    }

    public async Task<List<OptionModel>> GetActiveOptions()
    {
        var models = await OptionDataService.GetActiveOptions();
        return models.Select(o => new OptionModel(o)).ToList();
    }

    public async Task<List<OptionModel>> GetOptionsForModule(int moduleId)
    {
        var models = await OptionDataService.GetOptionsForModule(moduleId);
        return models.Select(o => new OptionModel(o)).ToList();
    }

    public async Task<OptionModel> UpdateOption(OptionModel opt)
    {
        return new OptionModel(await OptionDataService.UpdateOption(opt));
    }

    public async Task DeleteOption(OptionModel opt)
    {
        await OptionDataService.DeleteOption(opt);
    }

    public async Task DeleteOptions(IEnumerable<OptionModel> opts)
    {
        await OptionDataService.DeleteOptions(opts);
    }
}
