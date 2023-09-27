namespace Shared.ServiceBus.Commands;

public class AddNewSaleItemCommand
{    
    public string Sku { get; set; } = string.Empty; 
    public decimal Cost { get; set; }    
}