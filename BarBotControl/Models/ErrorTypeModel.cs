using BarBotControl.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BarBotControl.Models;

public class ErrorTypeModel
{
    public int ErrorTypeId { get; set; }
    public int ModuleId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "{0} must be greater than {1}.")]
    public int ErrorValue { get; set; }
    [Required]
    public string ErrorMessage { get; set; } = string.Empty;
    public bool Recoverable { get; set; }
    public int? SequenceId { get; set; }

    public ErrorTypeModel() { }

    public ErrorTypeModel(ErrorType err)
    {
        ErrorTypeId = err.ErrorTypeId;
        ErrorMessage = err.ErrorMessage;
        ErrorValue = err.ErrorValue;
        Recoverable = err.Recoverable;
        ModuleId = err.ModuleId;
        SequenceId = err.SequenceId;
    }
}
