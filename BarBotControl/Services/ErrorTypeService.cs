using BarBotControl.Models;
using BarBotControl.Services.Accessors;


namespace BarBotControl.Services;

public class ErrorTypeService
{
    private readonly ErrorTypeAccessor _errorTypeAccessor;

    public ErrorTypeService(ErrorTypeAccessor errorTypeAccessor)
    {
        _errorTypeAccessor = errorTypeAccessor;
    }

    public async Task<ErrorTypeModel> AddErrorType(ErrorTypeModel errType)
    {
        return new ErrorTypeModel(await _errorTypeAccessor.AddErrorType(errType));
    }

    public async Task<ErrorTypeModel> GetErrorType(ErrorTypeModel errType)
    {
        return new ErrorTypeModel(await _errorTypeAccessor.GetErrorType(errType));
    }

    public async Task<ErrorTypeModel> GetErrorTypeForModuleValue(int moduleId, int value)
    {
        return new ErrorTypeModel(await _errorTypeAccessor.GetErrorTypeForModuleValue(moduleId, value));
    }

    public async Task<List<ErrorTypeModel>> GetErrorTypesForModule(int moduleId)
    {
        var models = await _errorTypeAccessor.GetErrorTypesForModule(moduleId);
        return models.Select(o => new ErrorTypeModel(o)).ToList();
    }

    public async Task<ErrorTypeModel> UpdateErrorType(ErrorTypeModel errType)
    {
        return new ErrorTypeModel(await _errorTypeAccessor.UpdateErrorType(errType));
    }

    public async Task DeleteErrorType(ErrorTypeModel errType)
    {
        await _errorTypeAccessor.DeleteErrorType(errType);
    }

    public async Task DeleteErrorTypes(IEnumerable<ErrorTypeModel> errTypes)
    {
        await _errorTypeAccessor.DeleteErrorTypes(errTypes);
    }
}
