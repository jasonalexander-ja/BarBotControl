using System.ComponentModel.DataAnnotations;
using BarBotControl.Data.Models;

namespace BarBotControl.Models;

public class SequenceItemModel
{
    public int SequenceItemId { get; set; }
    public int SequenceId { get; set; }
    public int Index { get; set; }
    public int ModuleId { get; set; }
    public int OptionId { get; set; }
    public SequenceModel Sequence { get; set; } = null!;
    [Required]
    public ModuleModel Module { get; set; } = null!;
    [Required]
    public OptionModel Option { get; set; } = null!;

    public SequenceItemModel() { }
    public SequenceItemModel(SequenceItem model)
    {
        SequenceItemId = model.SequenceItemId;
        SequenceId = model.SequenceId;
        Index = model.Index;
        ModuleId = model.ModuleId;
        OptionId = model.OptionId;
        Sequence = new SequenceModel(model.Sequence);
        Module = new ModuleModel(model.Module);
        Option = new OptionModel(model.Option);
    }
}
