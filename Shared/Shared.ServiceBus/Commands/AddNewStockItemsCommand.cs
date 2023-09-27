namespace Shared.ServiceBus.Commands;

public class AddNewStockItemsCommand
{
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
