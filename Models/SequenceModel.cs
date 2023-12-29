namespace BarBotControl.Models;

public class SequenceModel
{
    public int SequenceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();

    public SequenceModel() { }

    public SequenceModel(Sequence sequence)
    {
        SequenceId = sequence.SequenceId;
        Name = sequence.Name;
        Description = sequence.Description;
        SequenceItems = sequence.SequenceItems;
    }
}
