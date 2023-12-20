namespace BarBotControl.Models;

public class Sequence
{
    public int SequenceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();
}
