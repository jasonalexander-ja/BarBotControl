using BarBotControl.Models;


namespace BarBotControl.Services;

public class ErrorTypeService
{
    private readonly ErrorTypeDataService ErrorTypeDataService;

    public ErrorTypeService(ErrorTypeDataService errorTypeDataService)
    {
        ErrorTypeDataService = errorTypeDataService;
    }

    public async Task<ErrorTypeModel> AddErrorType(ErrorTypeModel errType)
    {
        return new ErrorTypeModel(await ErrorTypeDataService.AddErrorType(errType));
    }

    public async Task<ErrorTypeModel> GetErrorType(ErrorTypeModel errType)
    {
        return new ErrorTypeModel(await ErrorTypeDataService.GetErrorType(errType));
    }

    public async Task<List<ErrorTypeModel>> GetErrorTypesForModule(int moduleId)
    {
        var models = await ErrorTypeDataService.GetErrorTypesForModule(moduleId);
        return models.Select(o => new ErrorTypeModel(o)).ToList();
    }

    public async Task<ErrorTypeModel> UpdateErrorType(ErrorTypeModel errType)
    {
        return new ErrorTypeModel(await ErrorTypeDataService.UpdateErrorType(errType));
    }

    public async Task DeleteErrorType(ErrorTypeModel errType)
    {
        await ErrorTypeDataService.DeleteErrorType(errType);
    }

    public async Task DeleteErrorTypes(IEnumerable<ErrorTypeModel> errTypes)
    {
        await ErrorTypeDataService.DeleteErrorTypes(errTypes);
    }
}
