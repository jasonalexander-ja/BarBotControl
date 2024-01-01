using System.ComponentModel.DataAnnotations;

namespace BarBotControl.Models;

public class ModuleModel
{
    public int ModuleId { get; set; }
    [Range(8, 127, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Channel { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public IEnumerable<OptionModel> Options { get; set; } = new List<OptionModel>();
    public IEnumerable<SequenceItemModel> SequenceItems { get; set; } = new List<SequenceItemModel>();
    public IEnumerable<ErrorTypeModel> ErrorTypes { get; set; } = new List<ErrorTypeModel>();

    public ModuleModel() { }
    public ModuleModel(
        Module model, 
        IEnumerable<Option>? options = null, 
        IEnumerable<ErrorType>? errTypes = null,
        IEnumerable<SequenceItem>? seqItems = null)
    {
        ModuleId = model.ModuleId;
        Channel = model.Channel;
        Name = model.Name;
        IsActive = model.IsActive;
        Options = options?.Select(s => new OptionModel(s)) ?? new List<OptionModel>();
        SequenceItems = seqItems?.Select(s => new SequenceItemModel(s)) ?? new List<SequenceItemModel>();
        ErrorTypes = errTypes?.Select(s => new ErrorTypeModel(s)) ?? new List<ErrorTypeModel>();
    }
}
