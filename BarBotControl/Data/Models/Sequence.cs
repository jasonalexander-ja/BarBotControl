﻿using BarBotControl.Models;

namespace BarBotControl.Data.Models;

public class Sequence
{
    public int SequenceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<SequenceItem> SequenceItems { get; set; } = new List<SequenceItem>();
    public ICollection<ErrorType> ErrorTypes { get; set; } = new List<ErrorType>();

    public Sequence() { }

    public Sequence(SequenceModel sequence)
    {
        SequenceId = sequence.SequenceId;
        Name = sequence.Name;
        Description = sequence.Description;
    }
}
