using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Basket.Store.Actions;

public class AddToCurrentBasketRequestAction
{
	public AddToCurrentBasketRequestAction(ProductDto productToAdd)
	{		
		ProductToAdd = productToAdd;
	}

	public ProductDto ProductToAdd { get; }	
}
