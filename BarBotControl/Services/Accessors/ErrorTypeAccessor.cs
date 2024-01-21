using BarBotControl.Config;
using BarBotControl.Models;
using BarBotControl.Exceptions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using BarBotControl.Data;
using BarBotControl.Data.Models;


namespace BarBotControl.Services.Accessors;

public class ErrorTypeAccessor
{
    private readonly AppDbContext _context;

    public ErrorTypeAccessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorType> AddErrorType(ErrorTypeModel errType)
    {
        var errorTypeModel = new ErrorType(errType);
        _context.ErrorTypes.Add(errorTypeModel);
        await _context.SaveChangesAsync();
        return errorTypeModel;
    }

    public async Task<ErrorType> GetErrorType(ErrorTypeModel errType)
    {
        var errorTypeModel = await _context.ErrorTypes.FirstOrDefaultAsync(
            o => o.ErrorTypeId == errType.ErrorTypeId && o.ModuleId == errType.ModuleId);
        if (errorTypeModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find error type `{errType.ErrorMessage}` on module. ");
        }
        return errorTypeModel;
    }

    public async Task<ErrorType> GetErrorTypeForModuleValue(int moduleId, int value)
    {
        var errorTypeModel = await _context.ErrorTypes.FirstOrDefaultAsync(
            o => o.ModuleId == moduleId && o.ErrorValue == value);
        if (errorTypeModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find error value `{value}` on module id `{moduleId}`. ");
        }
        return errorTypeModel;
    }

    public async Task<List<ErrorType>> GetErrorTypesForModule(int moduleId)
    {
        return await _context.ErrorTypes.Where(errorType => errorType.ModuleId == moduleId).ToListAsync();
    }

    public async Task<ErrorType> UpdateErrorType(ErrorTypeModel errType)
    {
        var optModel = await GetErrorType(errType);
        optModel.ErrorMessage = errType.ErrorMessage;
        optModel.Recoverable = errType.Recoverable;
        optModel.SequenceId = errType.SequenceId;
        await _context.SaveChangesAsync();
        return optModel;
    }

    public async Task DeleteErrorType(ErrorTypeModel errType)
    {
        var errTypeModel = await GetErrorType(errType);
        _context.ErrorTypes.Remove(errTypeModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteErrorTypes(IEnumerable<ErrorTypeModel> errTypes)
    {
        foreach (var err in errTypes)
        {
            var errTypeModel = await GetErrorType(err);
            _context.ErrorTypes.Remove(errTypeModel);
        }
        await _context.SaveChangesAsync();
    }
}
