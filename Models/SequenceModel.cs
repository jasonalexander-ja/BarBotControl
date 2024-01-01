using System.ComponentModel.DataAnnotations;

namespace BarBotControl.Models;

public class SequenceModel
{
    public int SequenceId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public IEnumerable<SequenceItemModel> SequenceItems { get; set; } = new List<SequenceItemModel>();

    public SequenceModel() { }

    public SequenceModel(Sequence sequence, IEnumerable<SequenceItem>? sequenceItems = null)
    {
        SequenceId = sequence.SequenceId;
        Name = sequence.Name;
        Description = sequence.Description;
        SequenceItems = sequenceItems?.Select(i => new SequenceItemModel(i)) ?? new List<SequenceItemModel>();
    }
}
