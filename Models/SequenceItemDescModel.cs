namespace BarBotControl.Models;

public class SequenceItemDescModel
{
    public int SequenceItemId { get; set; }
    public int Index { get; set; }
    public string Description { get; set; } = string.Empty;

    public SequenceItemDescModel(int sequenceItemId, int index, string description)
    {
        SequenceItemId = sequenceItemId;
        Index = index;
        Description = description;
    }
}