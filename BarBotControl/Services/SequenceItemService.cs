using BarBotControl.Models;
using BarBotControl.Services.Accessors;


namespace BarBotControl.Services;

public class SequenceItemService
{
    private readonly SequenceItemAccessor _sequenceItemAccessor;

    public SequenceItemService(SequenceItemAccessor sequenceItemAccessor)
    {
        _sequenceItemAccessor = sequenceItemAccessor;
    }

    public async Task<SequenceItemModel> AddSequence(SequenceItemModel seq)
    {
        return new SequenceItemModel(await _sequenceItemAccessor.AddSequenceItem(seq));
    }

    public async Task<SequenceItemModel> GetSequence(int seqId)
    {
        return new SequenceItemModel(await _sequenceItemAccessor.GetSequenceItem(seqId));
    }

    public async Task<List<SequenceItemModel>> GetSequenceItems()
    {
        var models = await _sequenceItemAccessor.GetSequenceItems();
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<List<SequenceItemModel>> GetItemsForSeq(int seqId)
    {
        var models = await _sequenceItemAccessor.GetItemsForSeq(seqId);
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<List<SequenceItemDescModel>> GetItemDescriptionsForSeq(int seqId)
    {
        var descriptions = await _sequenceItemAccessor.GetItemDescriptionsForSeq(seqId);
        return descriptions;
    }

    public async Task<List<SequenceItemModel>> GetItemsForSeqWithModuleOpt(int seqId)
    {
        var models = await _sequenceItemAccessor.GetItemsForSeqWithModuleOpt(seqId);
        return models.Select(s => new SequenceItemModel(s)).ToList();
    }

    public async Task<int> GetHighestIndexForSeq(int seqId)
    {
        var result = await _sequenceItemAccessor.GetHighestIndexForSeq(seqId);
        return result;
    }

    public async Task<SequenceItemModel> UpdateSequence(SequenceItemModel seq)
    {
        return new SequenceItemModel(await _sequenceItemAccessor.UpdateSequenceItem(seq));
    }

    public async Task SwapSeqItemIndexes(int item1Id, int item2Id)
    {
        var item1 = new SequenceItemModel(await _sequenceItemAccessor.GetSequenceItem(item1Id));
        var item2 = new SequenceItemModel(await _sequenceItemAccessor.GetSequenceItem(item2Id));
        var item1Index = item1.Index;
        var item2Index = item2.Index;
        item1.Index = item2Index;
        item2.Index = item1Index;
        await _sequenceItemAccessor.UpdateSequenceItem(item1);
        await _sequenceItemAccessor.UpdateSequenceItem(item2);
    }

    public async Task DeleteSequenceItem(int seqItemId)
    {
        await _sequenceItemAccessor.DeleteSequenceItem(seqItemId);
    }

    public async Task DeleteSequenceItems(IEnumerable<int> seqItemIds)
    {
        await _sequenceItemAccessor.DeleteSequenceItems(seqItemIds);
    }
}
