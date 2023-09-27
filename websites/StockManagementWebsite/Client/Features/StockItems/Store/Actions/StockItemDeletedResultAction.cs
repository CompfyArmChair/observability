namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class StockItemDeletedResultAction
{
    public int Id { get; }

    public StockItemDeletedResultAction(int id)
    {
        Id = id;
    }
}