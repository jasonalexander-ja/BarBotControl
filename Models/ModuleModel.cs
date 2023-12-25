using System.ComponentModel.DataAnnotations;

namespace BarBotControl.Models;

public class ModuleModel
{
    public int ModuleId { get; set; }
    public int Channel { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
