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

    public async Task<List<SequenceItemDescModel>> GetItemDescriptionsForSeq(int seqId)
    {
        var descriptions = await SequenceItemDataService.GetItemDescriptionsForSeq(seqId);
        return descriptions;
    }

    public async Task<List<SequenceItemModel>> GetItemsForSeqWithModuleOpt(int seqId)
    {
        var models = await SequenceItemDataService.GetItemsForSeqWithModuleOpt(seqId);
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<int> GetHighestIndexForSeq(int seqId)
    {
        var result = await SequenceItemDataService.GetHighestIndexForSeq(seqId);
        return result;
    }

    public async Task<SequenceItemModel> UpdateSequence(SequenceItemModel seq)
    {
        return new SequenceItemModel(await SequenceItemDataService.UpdateSequenceItem(seq));
    }

    public async Task SwapSeqItemIndexes(int item1Id, int item2Id)
    {
        var item1 = new SequenceItemModel(await SequenceItemDataService.GetSequenceItem(item1Id));
        var item2 = new SequenceItemModel(await SequenceItemDataService.GetSequenceItem(item2Id));
        var item1Index = item1.Index;
        var item2Index = item2.Index;
        item1.Index = item2Index;
        item2.Index = item1Index;
        await SequenceItemDataService.UpdateSequenceItem(item1);
        await SequenceItemDataService.UpdateSequenceItem(item2);
    }

    public async Task DeleteSequenceItem(int seqItemId)
    {
        await SequenceItemDataService.DeleteSequenceItem(seqItemId);
    }

    public async Task DeleteSequenceItems(IEnumerable<int> seqItemIds)
    {
        await SequenceItemDataService.DeleteSequenceItems(seqItemIds);
    }
}
