using System.ComponentModel.DataAnnotations.Schema;

namespace BarBotControl.Models;

public class Module
{
    public int ModuleId { get; set; }
    public int Channel { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Option> Options { get; } = new List<Option>();
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();
}
