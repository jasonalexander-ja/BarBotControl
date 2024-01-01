using BarBotControl.Models;


namespace BarBotControl.Services;

public class SequenceItemService
{
    private readonly SequenceItemDataService SequenceItemDataService;

    public SequenceItemService(SequenceItemDataService sequenceItemDataService)
    {
        SequenceItemDataService = sequenceItemDataService;
    }

    public async Task<SequenceItemModel> AddSequence(SequenceItemModel seq)
    {
        return new SequenceItemModel(await SequenceItemDataService.AddSequenceItem(seq));
    }

    public async Task<SequenceItemModel> GetSequence(int seqId)
    {
        return new SequenceItemModel(await SequenceItemDataService.GetSequenceItem(seqId));
    }

    public async Task<List<SequenceItemModel>> GetSequenceItems()
    {
        var models = await SequenceItemDataService.GetSequenceItems();
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<List<SequenceItemModel>> GetItemsForSeq(int seqId)
    {
        var models = await SequenceItemDataService.GetItemsForSeq(seqId);
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<List<SequenceItemModel>> GetItemsForSeqWithModuleOpt(int seqId)
    {
        var models = await SequenceItemDataService.GetItemsForSeqWithModuleOpt(seqId);
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<SequenceItemModel> UpdateSequence(SequenceItemModel seq)
    {
        return new SequenceItemModel(await SequenceItemDataService.UpdateSequenceItem(seq));
    }

    public async Task DeleteSequence(SequenceItemModel seq)
    {
        await SequenceItemDataService.DeleteSequenceItem(seq);
    }

    public async Task DeleteSequence(IEnumerable<SequenceItemModel> seqs)
    {
        await SequenceItemDataService.DeleteSequenceItems(seqs);
    }
}
