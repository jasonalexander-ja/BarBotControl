using System.ComponentModel.DataAnnotations;
using BarBotControl.Data.Models;

namespace BarBotControl.Models;

public class ModuleModel
{
    public int ModuleId { get; set; }
    [Range(8, 127, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Channel { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ModuleModel() { }
    public ModuleModel(Module model)
    {
        ModuleId = model.ModuleId;
        Channel = model.Channel;
        Name = model.Name;
        IsActive = model.IsActive;
    }
}
