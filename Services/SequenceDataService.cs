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

    public async Task<Sequence> AddSequence(SequenceModel opt)
    {
        var seqModel = new Sequence(opt);
        Context.Sequences.Add(seqModel);
        await Context.SaveChangesAsync();
        return seqModel;
    }

    public async Task<Sequence> GetSequence(int seqId)
    {
        var optionModel = await Context.Sequences.FirstOrDefaultAsync(o => o.SequenceId == seqId);
        if (optionModel is null) 
        {
            throw new ObjectNotFoundException($"Cannot find option `{seqId}`. ");
        }
        return optionModel;
    }

    public async Task<List<Sequence>> GetSequences()
    {
        return await Context.Sequences.ToListAsync();
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
        Context.Sequences.Remove(seqModel);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteSequences(IEnumerable<SequenceModel> sequences)
    {
        foreach (var seq in sequences)
        {
            var optionModel = await GetSequence(seq.SequenceId);
            Context.Sequences.Remove(optionModel);
        }
        await Context.SaveChangesAsync();
    }
}
