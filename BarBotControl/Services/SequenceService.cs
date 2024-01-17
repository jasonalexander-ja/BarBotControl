using BarBotControl.Models;


namespace BarBotControl.Services;

public class SequenceService
{
    private readonly SequenceDataService SequenceDataService;
    private readonly SequenceItemDataService SequenceItemDataService;

    public SequenceService(SequenceDataService sequenceDataService, SequenceItemDataService sequenceItemDataService)
    {
        SequenceDataService = sequenceDataService;
        SequenceItemDataService = sequenceItemDataService;
    }

    public async Task<SequenceModel> AddSequence(SequenceModel seq)
    {
        return new SequenceModel(await SequenceDataService.AddSequence(seq));
    }

    public async Task<SequenceModel> GetSequence(int seqId)
    {
        return new SequenceModel(await SequenceDataService.GetSequence(seqId));
    }

    public async Task<SequenceModel> GetSequenceWithItems(int seqId)
    {
        var model = await SequenceDataService.GetSequenceWithItems(seqId);
        return new SequenceModel(model);
    }

    public async Task<List<SequenceModel>> GetActiveSequences()
    {
        var models = await SequenceDataService.GetActiveSequences();
        return models.Select(s => new SequenceModel(s)).ToList();
    }

    public async Task<List<SequenceModel>> GetInactiveSequences()
    {
        var models = await SequenceDataService.GetInactiveSequences();
        return models.Select(s => new SequenceModel(s)).ToList();
    }

    public async Task<List<SequenceModel>> GetSequencesWithItems()
    {
        var models = await SequenceDataService.GetSequencesWithItems();
        return models.Select(s => new SequenceModel(s)).ToList();
    }

    public async Task<SequenceModel> UpdateSequence(SequenceModel seq)
    {
        return new SequenceModel(await SequenceDataService.UpdateSequence(seq));
    }

    public async Task DeleteSequence(SequenceModel seq)
    {
        await SequenceDataService.DeleteSequence(seq);
    }

    public async Task DeleteSequence(IEnumerable<SequenceModel> seqs)
    {
        await SequenceDataService.DeleteSequences(seqs);
    }
}
