using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using BarBotControl.Data;


namespace BarBotControl.Services;

public class ErrorTypeDataService
{
    private readonly AppDbContext Context;

    public ErrorTypeDataService(AppDbContext context)
    {
        Context = context;
    }

    public async Task<ErrorType> AddErrorType(ErrorTypeModel errType)
    {
        var errorTypeModel = new ErrorType(errType);
        Context.ErrorTypes.Add(errorTypeModel);
        await Context.SaveChangesAsync();
        return errorTypeModel;
    }

    public async Task<ErrorType> GetErrorType(ErrorTypeModel errType)
    {
        var errorTypeModel = await Context.ErrorTypes.FirstOrDefaultAsync(
            o => o.ErrorTypeId == errType.ErrorTypeId && o.ModuleId == errType.ModuleId);
        if (errorTypeModel is null) 
        {
            throw new ObjectNotFoundException($"Cannot find module `{errType.ErrorMessage}` on module. ");
        }
        return errorTypeModel;
    }

    public async Task<List<ErrorType>> GetErrorTypesForModule(int moduleId)
    {
        return await Context.ErrorTypes.Where(errorType => errorType.ModuleId == moduleId).ToListAsync();
    }

    public async Task<ErrorType> UpdateErrorType(ErrorTypeModel errType)
    {
        var optModel = await GetErrorType(errType);
        optModel.ErrorMessage = errType.ErrorMessage;
        optModel.Recoverable = errType.Recoverable;
        optModel.SequenceId = errType.SequenceId;
        await Context.SaveChangesAsync();
        return optModel;
    }

    public async Task DeleteErrorType(ErrorTypeModel errType)
    {
        var errTypeModel = await GetErrorType(errType);
        Context.ErrorTypes.Remove(errTypeModel);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteErrorTypes(IEnumerable<ErrorTypeModel> errTypes)
    {
        foreach (var err in errTypes)
        {
            var errTypeModel = await GetErrorType(err);
            Context.ErrorTypes.Remove(errTypeModel);
        }
        await Context.SaveChangesAsync();
    }
}
