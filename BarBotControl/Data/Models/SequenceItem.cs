using BarBotControl.Models;

namespace BarBotControl.Data.Models;

public class SequenceItem
{
    public int SequenceItemId { get; set; }
    public int SequenceId { get; set; }
    public int Index { get; set; }
    public int ModuleId { get; set; }
    public int OptionId { get; set; }
    public Sequence Sequence { get; set; } = null!;
    public Module Module { get; set; } = null!;
    public Option Option { get; set; } = null!;

    public SequenceItem() { }
    public SequenceItem(SequenceItemModel model)
    {
        SequenceItemId = model.SequenceItemId;
        SequenceId = model.SequenceId;
        Index = model.Index;
        ModuleId = model.ModuleId;
        OptionId = model.OptionId;
    }
}
