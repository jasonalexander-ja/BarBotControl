using BarBotControl.Models;


namespace BarBotControl.Services;

public class SequenceService
{
    private readonly SequenceDataService SequenceDataService;

    public SequenceService(SequenceDataService sequenceDataService)
    {
        SequenceDataService = sequenceDataService;
    }

    public async Task<SequenceModel> AddSequence(SequenceModel seq)
    {
        return new SequenceModel(await SequenceDataService.AddSequence(seq));
    }

    public async Task<SequenceModel> GetSequence(int seqId)
    {
        return new SequenceModel(await SequenceDataService.GetSequence(seqId));
    }

    public async Task<List<SequenceModel>> GetSequences()
    {
        var models = await SequenceDataService.GetSequences();
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
