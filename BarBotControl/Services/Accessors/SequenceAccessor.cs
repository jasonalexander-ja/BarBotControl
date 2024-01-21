using BarBotControl.Data;
using BarBotControl.Data.Models;
using BarBotControl.Exceptions;
using BarBotControl.Models;
using Microsoft.EntityFrameworkCore;


namespace BarBotControl.Services.Accessors;

public class SequenceAccessor
{
    private readonly AppDbContext _context;

    public SequenceAccessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Sequence> AddSequence(SequenceModel seq)
    {
        var seqModel = new Sequence(seq);
        _context.Sequences.Add(seqModel);
        await _context.SaveChangesAsync();
        return seqModel;
    }

    public async Task<Sequence> GetSequence(int seqId)
    {
        var sequenceModel = await _context.Sequences.FirstOrDefaultAsync(o => o.SequenceId == seqId);
        if (sequenceModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find sequence `{seqId}`. ");
        }
        return sequenceModel;
    }

    public async Task<Sequence> GetSequenceWithItems(int seqId)
    {
        var sequenceModel = await _context.Sequences.FirstOrDefaultAsync(o => o.SequenceId == seqId);
        if (sequenceModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find sequence `{seqId}`. ");
        }
        sequenceModel.SequenceItems = await _context.Entry(sequenceModel)
            .Collection(i => i.SequenceItems)
            .Query()
            .Include(i => i.Module)
            .Include(i => i.Option)
            .ToListAsync();
        return sequenceModel;
    }

    public async Task<List<Sequence>> GetActiveSequences()
    {
        return await _context.Sequences
            .Where(s => s.SequenceItems.All(i =>
                i.Module.IsActive && i.Option.IsActive
            ))
            .ToListAsync();
    }

    public async Task<List<Sequence>> GetInactiveSequences()
    {
        return await _context.Sequences
            .Where(s => s.SequenceItems.Any(i =>
                !i.Module.IsActive || !i.Option.IsActive
            ))
            .ToListAsync();
    }

    public async Task<Sequence> GetSequenceForErrType(int moduleId, int value)
    {
        var sequenceModel = await _context.Sequences
            .Where(s => s.ErrorTypes.Any(e => 
                e.ModuleId == moduleId && e.ErrorValue == value))
            .FirstOrDefaultAsync();
        if (sequenceModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find sequence for error value `{value}` on module id `{moduleId}`. ");
        }
        return sequenceModel;
    }

    public async Task<Sequence> UpdateSequence(SequenceModel seq)
    {
        var seqModel = await GetSequence(seq.SequenceId);
        seqModel.Name = seq.Name;
        seqModel.Description = seq.Description;
        await _context.SaveChangesAsync();
        return seqModel;
    }

    public async Task DeleteSequence(SequenceModel seq)
    {
        var seqModel = await GetSequence(seq.SequenceId);
        var seqItems = await _context.SequenceItem.Where(i => i.SequenceId == seqModel.SequenceId)
            .ToListAsync();
        if (seqItems.Any())
        {
            _context.SequenceItem.RemoveRange(seqItems);
            await _context.SaveChangesAsync();
        }
        _context.Sequences.Remove(seqModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSequences(IEnumerable<SequenceModel> sequences)
    {
        foreach (var seq in sequences)
        {
            var sequenceModel = await GetSequence(seq.SequenceId);
            _context.Sequences.Remove(sequenceModel);
        }
        await _context.SaveChangesAsync();
    }
}
