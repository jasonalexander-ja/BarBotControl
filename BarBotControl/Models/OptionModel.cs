using System.ComponentModel.DataAnnotations.Schema;
using BarBotControl.Data.Models;

namespace BarBotControl.Models;

public class OptionModel
{
    public int OptionId { get; set; }
    public int ModuleId { get; set; }
    public int OptionValue { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public OptionModel() { }

    public OptionModel(Option opt)
    {
        OptionId = opt.OptionId;
        ModuleId = opt.ModuleId;
        OptionValue = opt.OptionValue;
        Name = opt.Name;
        IsActive = opt.IsActive;
    }
}
