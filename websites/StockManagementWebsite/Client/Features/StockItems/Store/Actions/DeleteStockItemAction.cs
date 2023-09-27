namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class DeleteStockItemAction
{
    public int Id { get; }

    public DeleteStockItemAction(int id)
    {
        Id = id;
    }
}
