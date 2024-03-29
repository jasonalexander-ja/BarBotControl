﻿using System.ComponentModel.DataAnnotations.Schema;
using BarBotControl.Models;

namespace BarBotControl.Data.Models;

public class Module
{
    public int ModuleId { get; set; }
    public int Channel { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Option> Options { get; } = new List<Option>();
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();
    public ICollection<ErrorType> ErrorTypes { get; set; } = new List<ErrorType>();

    public Module() { }
    public Module(ModuleModel model)
    {
        ModuleId = model.ModuleId;
        Channel = model.Channel;
        Name = model.Name;
        IsActive = model.IsActive;
    }
}
