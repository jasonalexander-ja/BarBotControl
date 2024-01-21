using BarBotControl.Comms.Exceptions;
using BarBotControl.Worker.Services;
using BarBotControl.Worker.Models;
using BarBotControl.Comms.Models;
using System.Threading.Channels;
using BarBotControl.Config;
using Response = BarBotControl.Worker.Models.Response<BarBotControl.Comms.Models.ResponseType>;
using BarBotControl.Data.Models;
using BarBotControl.Models;
using MudBlazor;

namespace BarBotControl.Services;

public class ErrorHandlingService
{
    private readonly IDialogService _dialogService;
    private readonly SequenceItemService _sequenceItemService;
    private readonly ErrorTypeService _errorTypeService;


    public ErrorHandlingService(IDialogService dialogService, 
        SequenceItemService sequenceItemService,
        ErrorTypeService errorTypeService)
    {
        _dialogService = dialogService;
        _sequenceItemService = sequenceItemService;
        _errorTypeService = errorTypeService;
    }

    public async Task<bool> HandleError(ResponseType.WorkerException response, 
        List<SequenceItemModel> sequenceItems)
    {
        return response.WorkerExceptionBase switch
        {
            OpeningI2cException e => await I2COpenError(e, response.ExceptionResponseSender),
            I2cCommunicationException e => 
                await ModuleCommunicationErr(e, sequenceItems, response.ExceptionResponseSender),
            ModuleReturnedException e => 
                await ModuleReturnedErr(e, sequenceItems, response.ExceptionResponseSender),
            _ => true
        };
    }

    public async Task<bool> ModuleReturnedErr(ModuleReturnedException err,
        List<SequenceItemModel> sequenceItem, 
        ChannelWriter<ExceptionResponse> response)
    {
        var moduleId = sequenceItem.Where(i => i.Index == err.SequenceIndex)
            .Select(i => i.SequenceId)
            .FirstOrDefault(0);
        var errType = await TryGetErrorType(moduleId, err.ReturnedValue);
        return errType switch
        {
            ErrorTypeModel { Recoverable: false } => await ErrorTypeUnrecoverable(errType, response),
            ErrorTypeModel { Recoverable: true } => await ErrorTypeRecoverable(errType, response),
            _ => await UnknownErrorType(err, response),
        };
    }

    public async Task<bool> UnknownErrorType(ModuleReturnedException err,
        ChannelWriter<ExceptionResponse> response)
    {
        await _dialogService.ShowMessageBox(
            "Error",
            $"A module on I2C address `{err.ModuleAddress}` returned an unknown error code `{err.ReturnedValue}`, "
            + $" with the option `{err.Option}`, please contact an admin, your request is now cancelled,"
        );
        await response.WriteAsync(ExceptionResponse.Halt());
        return false;
    }

    public async Task<bool> ErrorTypeRecoverable(ErrorTypeModel errType,
        ChannelWriter<ExceptionResponse> response)
    {
        var res = await _dialogService.ShowMessageBox(
            "Error",
            $"An error `{errType.ErrorMessage}` was returned but is recoverable, would you like to proceed? ",
            yesText: "Continue",
            noText: "Cancel Request"
        ) ?? false;
        if (errType.SequenceId is null)
        {
            var exRes = res ? ExceptionResponse.Continue() : ExceptionResponse.Halt();
            await response.WriteAsync(exRes);
            return false;
        }
        await PerformRecoverySequence(res, errType.SequenceId.Value, response);
        return res;
    }

    public async Task<bool> ErrorTypeUnrecoverable(ErrorTypeModel errType,
        ChannelWriter<ExceptionResponse> response)
    {
        await _dialogService.ShowMessageBox(
            "Error",
            $"An error was returned {errType.ErrorMessage}, "
            + "this is unrecoverable and your we have cancelled your request. "
        );
        if (errType.SequenceId is null)
        {
            await response.WriteAsync(ExceptionResponse.Halt());
            return false;
        }
        await PerformRecoverySequence(false, errType.SequenceId.Value, response);
        return false;
    }

    public async Task<bool> ModuleCommunicationErr(I2cCommunicationException err,
        List<SequenceItemModel> sequenceItem,
        ChannelWriter<ExceptionResponse> response)
    {
        var moduleName = sequenceItem.Where(i => i.Index == err.SequenceIndex)
            .Select(i => i.Module.Name)
            .FirstOrDefault("");
        bool res = await _dialogService.ShowMessageBox(
            "Module Communication Error",
            $"There was an error accessing module `{moduleName}` set to I2C address `{err.I2cAddress}` "
            + $"on I2C Channel {err.I2cChannel}. Do you wish to cancel? "
            + $"\nReturned `{err.InnerException?.Message}`",
            yesText: "Skip",
            noText: "Cancel Request"
        ) ?? false;
        var exResponse = res ? ExceptionResponse.Continue() : ExceptionResponse.Halt();
        await response.WriteAsync(exResponse);
        return res;
    }

    public async Task<bool> I2COpenError(OpeningI2cException err,
        ChannelWriter<ExceptionResponse> response)
    {
        await _dialogService.ShowMessageBox(
            "Communication Error",
            $"There was an exception opening I2C Channel {err.I2cChannel}, "
            + "please contact an admin, your request is now cancelled,"
            + $" returned `{err.InnerException?.Message}`"
        );
        await response.WriteAsync(ExceptionResponse.Halt());
        return false;
    }

    public async Task PerformRecoverySequence(bool shouldContinue,
        int seqId,
        ChannelWriter<ExceptionResponse> response)
    {
        var recoverySequenceModel = await _sequenceItemService.GetItemsForSeq(seqId);
        var recoverySequence = recoverySequenceModel.Select(s => new WorkerSequenceItem()
        {
            Index = s.Index,
            ModuleAddress = s.Module.Channel,
            Option = s.Option.OptionValue
        }).ToList();
        var exResponse = shouldContinue ?
            ExceptionResponse.PerformSequenceContinue(recoverySequence)
            : ExceptionResponse.PerformSequenceHalt(recoverySequence);
        await response.WriteAsync(exResponse);
    }

    public async Task<ErrorTypeModel?> TryGetErrorType(int moduleId, int value)
    {
        try
        {
            return await _errorTypeService.GetErrorTypeForModuleValue(moduleId, value);
        }
        catch
        {
            return null;
        }
    }
}
