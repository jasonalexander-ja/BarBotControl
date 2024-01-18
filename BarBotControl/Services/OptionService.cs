using BarBotControl.Models;
using BarBotControl.Services.Accessors;


namespace BarBotControl.Services;

public class OptionService
{
    private readonly OptionAccessor _optionAccessor;

    public OptionService(OptionAccessor optionDataService)
    {
        _optionAccessor = optionDataService;
    }

    public async Task<OptionModel> AddOption(OptionModel opt)
    {
        return new OptionModel(await _optionAccessor.AddOption(opt));
    }

    public async Task<OptionModel> GetOption(OptionModel opt)
    {
        return new OptionModel(await _optionAccessor.GetOption(opt));
    }

    public async Task<List<OptionModel>> GetActiveOptions()
    {
        var models = await _optionAccessor.GetActiveOptions();
        return models.Select(o => new OptionModel(o)).ToList();
    }

    public async Task<List<OptionModel>> GetOptionsForModule(int moduleId)
    {
        var models = await _optionAccessor.GetOptionsForModule(moduleId);
        return models.Select(o => new OptionModel(o)).ToList();
    }

    public async Task<OptionModel> UpdateOption(OptionModel opt)
    {
        return new OptionModel(await _optionAccessor.UpdateOption(opt));
    }

    public async Task DeleteOption(OptionModel opt)
    {
        await _optionAccessor.DeleteOption(opt);
    }

    public async Task DeleteOptions(IEnumerable<OptionModel> opts)
    {
        await _optionAccessor.DeleteOptions(opts);
    }
}
