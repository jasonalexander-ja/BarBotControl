using BarBotControl.Data;
using BarBotControl.Data.Models;
using BarBotControl.Exceptions;
using BarBotControl.Models;
using Microsoft.EntityFrameworkCore;


namespace BarBotControl.Services.Accessors;

public class SequenceItemAccessor
{
    private readonly AppDbContext _context;

    public SequenceItemAccessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SequenceItem> AddSequenceItem(SequenceItemModel opt)
    {
        var seqItemModel = new SequenceItem(opt);
        _context.SequenceItem.Add(seqItemModel);
        await _context.SaveChangesAsync();
        return seqItemModel;
    }

    public async Task<SequenceItem> GetSequenceItem(int seqId)
    {
        var sequenceModel = await _context.SequenceItem.FirstOrDefaultAsync(o => o.SequenceItemId == seqId);
        if (sequenceModel is null)
        {
            throw new ObjectNotFoundException($"Cannot find sequence `{seqId}`. ");
        }
        return sequenceModel;
    }

    public async Task<List<SequenceItem>> GetSequenceItems()
    {
        return await _context.SequenceItem.ToListAsync();
    }

    public async Task<List<SequenceItem>> GetItemsForSeq(int seqId)
    {
        return await _context.SequenceItem
            .Where(s => s.SequenceId == seqId)
            .ToListAsync();
    }

    public async Task<List<SequenceItem>> GetItemsForSeqWithModuleOpt(int seqId)
    {
        return await _context.SequenceItem
            .Include(s => s.Option)
            .Include(s => s.Module)
            .Where(s => s.SequenceId == seqId)
            .ToListAsync();
    }

    public async Task<List<SequenceItemDescModel>> GetItemDescriptionsForSeq(int seqId)
    {
        return await _context.SequenceItem
            .Where(s => s.SequenceId == seqId)
            .Select(s => new SequenceItemDescModel(s.SequenceItemId, s.Index, $"{s.Module.Name}: {s.Option.Name}"))
            .ToListAsync();
    }

    public async Task<int> GetHighestIndexForSeq(int seqId)
    {
        if (!await _context.SequenceItem
                .Where(s => s.SequenceId == seqId).AnyAsync())
            return 0;
        return await _context.SequenceItem
            .Where(s => s.SequenceId == seqId)
            .MaxAsync(x => x.Index);
    }

    public async Task<SequenceItem> UpdateSequenceItem(SequenceItemModel seq)
    {
        var seqItemModel = await GetSequenceItem(seq.SequenceItemId);
        seqItemModel.Index = seq.Index;
        await _context.SaveChangesAsync();
        return seqItemModel;
    }

    public async Task DeleteSequenceItem(int seqItemId)
    {
        var seqItemModel = await GetSequenceItem(seqItemId);
        _context.SequenceItem.Remove(seqItemModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSequenceItems(IEnumerable<int> seqItemIds)
    {
        foreach (var id in seqItemIds)
        {
            var sequenceModel = await GetSequenceItem(id);
            _context.SequenceItem.Remove(sequenceModel);
        }
        await _context.SaveChangesAsync();
    }
}
