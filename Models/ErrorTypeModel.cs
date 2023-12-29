using Microsoft.EntityFrameworkCore;

namespace BarBotControl.Models;

public class ErrorTypeModel
{
    public int ErrorTypeId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool Recoverable { get; set; }
    public int ModuleId { get; set; }
    [Comment("Recovery Sequence Id")]
    public int? SequenceId { get; set; }

    public ErrorTypeModel(ErrorType err)
    {
        ErrorTypeId = err.ErrorTypeId;
        ErrorMessage = err.ErrorMessage;
        Recoverable = err.Recoverable;
        ModuleId = err.ModuleId;
        SequenceId = err.SequenceId;
    }
}
