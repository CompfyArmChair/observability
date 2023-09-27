namespace Shared.ServiceBus.Commands;

public class AddNewCatalogueItemCommand
{	
	public string Sku { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
}
