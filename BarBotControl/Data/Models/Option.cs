using System.ComponentModel.DataAnnotations.Schema;
using BarBotControl.Models;

namespace BarBotControl.Data.Models;

public class Option
{
    public int OptionId { get; set; }
    public int ModuleId { get; set; }
    public int OptionValue { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Module Module { get; set; } = null!;
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();

    public Option() { }
    public Option(OptionModel opt)
    {
        OptionId = opt.OptionId;
        ModuleId = opt.ModuleId;
        OptionValue = opt.OptionValue;
        Name = opt.Name;
        IsActive = opt.IsActive;
    }
}
