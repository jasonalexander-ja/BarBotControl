using BarBotControl.Models;
using BarBotControl.Services.Accessors;


namespace BarBotControl.Services;

public class SequenceService
{
    private readonly SequenceAccessor _sequenceAccessor;

    public SequenceService(SequenceAccessor sequenceAccessor)
    {
        _sequenceAccessor = sequenceAccessor;
    }

    public async Task<SequenceModel> AddSequence(SequenceModel seq)
    {
        return new SequenceModel(await _sequenceAccessor.AddSequence(seq));
    }

    public async Task<SequenceModel> GetSequence(int seqId)
    {
        return new SequenceModel(await _sequenceAccessor.GetSequence(seqId));
    }

    public async Task<SequenceModel> GetSequenceWithItems(int seqId)
    {
        var model = await _sequenceAccessor.GetSequenceWithItems(seqId);
        return new SequenceModel(model);
    }

    public async Task<List<SequenceModel>> GetActiveSequences()
    {
        var models = await _sequenceAccessor.GetActiveSequences();
        return models.Select(s => new SequenceModel(s)).ToList();
    }

    public async Task<List<SequenceModel>> GetInactiveSequences()
    {
        var models = await _sequenceAccessor.GetInactiveSequences();
        return models.Select(s => new SequenceModel(s)).ToList();
    }

    public async Task<List<SequenceModel>> GetSequencesWithItems()
    {
        var models = await _sequenceAccessor.GetSequencesWithItems();
        return models.Select(s => new SequenceModel(s)).ToList();
    }

    public async Task<SequenceModel> UpdateSequence(SequenceModel seq)
    {
        return new SequenceModel(await _sequenceAccessor.UpdateSequence(seq));
    }

    public async Task DeleteSequence(SequenceModel seq)
    {
        await _sequenceAccessor.DeleteSequence(seq);
    }

    public async Task DeleteSequence(IEnumerable<SequenceModel> seqs)
    {
        await _sequenceAccessor.DeleteSequences(seqs);
    }
}
