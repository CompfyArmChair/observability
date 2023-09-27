namespace Shared.ServiceBus.Commands;

public class EditSaleItemCommand
{
    public string Sku { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}