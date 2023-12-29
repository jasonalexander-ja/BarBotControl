namespace BarBotControl.Models;

public class SequenceItemModel
{
    public int SequenceItemId { get; set; }
    public int SequenceId { get; set; }
    public int Index { get; set; }
    public int ModuleId { get; set; }
    public int OptionId { get; set; }

    public SequenceItemModel() { }
    public SequenceItemModel(SequenceItem model)
    {
        SequenceItemId = model.SequenceItemId;
        SequenceId = model.SequenceId;
        Index = model.Index;
        ModuleId = model.ModuleId;
        OptionId = model.OptionId;
    }
}
