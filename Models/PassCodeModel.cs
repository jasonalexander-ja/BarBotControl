using System.ComponentModel.DataAnnotations;

namespace BarBotControl.Models;

public class PassCodeModel
{
    [Required]
    public string Passcode { get; set; } = string.Empty;
}
