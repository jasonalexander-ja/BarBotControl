using BarBotControl.Data;
using BarBotControl.Exceptions;
using BarBotControl.Models;
using Microsoft.EntityFrameworkCore;


namespace BarBotControl.Services;

public class SequenceItemDataService
{
    private readonly AppDbContext Context;

    public SequenceItemDataService(AppDbContext context)
    {
        Context = context;
    }

    public async Task<SequenceItem> AddSequenceItem(SequenceItemModel opt)
    {
        var seqItemModel = new SequenceItem(opt);
        Context.SequenceItem.Add(seqItemModel);
        await Context.SaveChangesAsync();
        return seqItemModel;
    }

    public async Task<SequenceItem> GetSequenceItem(int seqId)
    {
        var sequenceModel = await Context.SequenceItem.FirstOrDefaultAsync(o => o.SequenceItemId == seqId);
        if (sequenceModel is null) 
        {
            throw new ObjectNotFoundException($"Cannot find sequence `{seqId}`. ");
        }
        return sequenceModel;
    }

    public async Task<List<SequenceItem>> GetSequenceItems()
    {
        return await Context.SequenceItem.ToListAsync();
    }

    public async Task<List<SequenceItem>> GetItemsForSeq(int seqId)
    {
        return await Context.SequenceItem
            .Where(s => s.SequenceId == seqId)
            .ToListAsync();
    }

    public async Task<List<SequenceItem>> GetItemsForSeqWithModuleOpt(int seqId)
    {
        return await Context.SequenceItem
            .Include(s => s.Option)
            .Include(s => s.Module)
            .Where(s => s.SequenceId == seqId)
            .ToListAsync();
    }

    public async Task<SequenceItem> UpdateSequenceItem(SequenceItemModel seq)
    {
        var seqItemModel = await GetSequenceItem(seq.SequenceItemId);
        seqItemModel.Index = seq.Index;
        await Context.SaveChangesAsync();
        return seqItemModel;
    }

    public async Task DeleteSequenceItem(SequenceItemModel seq)
    {
        var seqItemModel = await GetSequenceItem(seq.SequenceItemId);
        Context.SequenceItem.Remove(seqItemModel);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteSequenceItems(IEnumerable<SequenceItemModel> SequenceItems)
    {
        foreach (var seq in SequenceItems)
        {
            var sequenceModel = await GetSequenceItem(seq.SequenceItemId);
            Context.SequenceItem.Remove(sequenceModel);
        }
        await Context.SaveChangesAsync();
    }
}
