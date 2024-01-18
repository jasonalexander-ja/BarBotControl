using BarBotControl.Models;
using Microsoft.EntityFrameworkCore;

namespace BarBotControl.Data.Models;

public class ErrorType
{
    public int ErrorTypeId { get; set; }
    public int ModuleId { get; set; }
    public int ErrorValue { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool Recoverable { get; set; }
    [Comment("Recovery Sequence Id")]
    public int? SequenceId { get; set; }
    public Module Module { get; set; } = null!;
    [Comment("Recovery Sequence")]
    public Sequence? Sequence { get; set; }

    public ErrorType() { }
    public ErrorType(ErrorTypeModel err)
    {
        ErrorTypeId = err.ErrorTypeId;
        ErrorMessage = err.ErrorMessage;
        ErrorValue = err.ErrorValue;
        Recoverable = err.Recoverable;
        ModuleId = err.ModuleId;
        SequenceId = err.SequenceId;
    }
}
