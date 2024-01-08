using BarBotControl.Data;
using BarBotControl.Exceptions;
using BarBotControl.Models;
using Microsoft.EntityFrameworkCore;


namespace BarBotControl.Services;

public class SequenceDataService
{
    private readonly AppDbContext Context;

    public SequenceDataService(AppDbContext context)
    {
        Context = context;
    }

    public async Task<Sequence> AddSequence(SequenceModel seq)
    {
        var seqModel = new Sequence(seq);
        Context.Sequences.Add(seqModel);
        await Context.SaveChangesAsync();
        return seqModel;
    }

    public async Task<Sequence> GetSequence(int seqId)
    {
        var sequenceModel = await Context.Sequences.FirstOrDefaultAsync(o => o.SequenceId == seqId);
        if (sequenceModel is null) 
        {
            throw new ObjectNotFoundException($"Cannot find sequence `{seqId}`. ");
        }
        return sequenceModel;
    }

    public async Task<Sequence> GetSequenceWithItems(int seqId)
    {
        var sequenceModel = await Context.Sequences.FirstOrDefaultAsync(o => o.SequenceId == seqId);
        if (sequenceModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find sequence `{seqId}`. ");
        }
        sequenceModel.SequenceItems = await Context.Entry(sequenceModel)
            .Collection(i => i.SequenceItems)
            .Query()
            .Include(i => i.Module)
            .Include(i => i.Option)
            .ToListAsync();
        return sequenceModel;
    }

    public async Task<List<Sequence>> GetActiveSequences()
    {
        return await Context.Sequences
            .Where(s => s.SequenceItems.All(i => 
                i.Module.IsActive && i.Option.IsActive
            ))
            .ToListAsync();
    }

    public async Task<List<Sequence>> GetInactiveSequences()
    {
        return await Context.Sequences
            .Where(s => s.SequenceItems.Any(i => 
                !i.Module.IsActive || !i.Option.IsActive
            ))
            .ToListAsync();
    }

    public async Task<List<Sequence>> GetSequencesWithItems()
    {
        return await Context.Sequences
            .Include(s => s.SequenceItems)
            .ToListAsync();
    }

    public async Task<Sequence> UpdateSequence(SequenceModel seq)
    {
        var seqModel = await GetSequence(seq.SequenceId);
        seqModel.Name = seq.Name;
        seqModel.Description = seq.Description;
        await Context.SaveChangesAsync();
        return seqModel;
    }

    public async Task DeleteSequence(SequenceModel seq)
    {
        var seqModel = await GetSequence(seq.SequenceId);
        var seqItems = await Context.SequenceItem.Where(i => i.SequenceId == seqModel.SequenceId)
            .ToListAsync();
        if (seqItems.Any())
        {
            Context.SequenceItem.RemoveRange(seqItems);
            await Context.SaveChangesAsync();
        }
        Context.Sequences.Remove(seqModel);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteSequences(IEnumerable<SequenceModel> sequences)
    {
        foreach (var seq in sequences)
        {
            var sequenceModel = await GetSequence(seq.SequenceId);
            Context.Sequences.Remove(sequenceModel);
        }
        await Context.SaveChangesAsync();
    }
}
