using Microsoft.EntityFrameworkCore;

namespace BarBotControl.Models;

public class ErrorType
{
    public int ErrorTypeId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool Recoverable { get; set; }
    public int ModuleId { get; set; }
    [Comment("Recovery Sequence Id")]
    public int? SequenceId { get; set; }
    public Module Module { get; set; } = null!;
    [Comment("Recovery Sequence")]
    public Sequence? Sequence { get; set; }
}
