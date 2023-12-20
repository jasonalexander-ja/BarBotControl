using System.ComponentModel.DataAnnotations.Schema;

namespace BarBotControl.Models;

public class Option
{
    public int OptionId { get; set; }
    public int ModuleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Module Module { get; set; } = null!;
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();
}
