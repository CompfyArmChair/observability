using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Basket.Store.Actions;

public class AddToCurrentBasketRequestResultAction
{
	public IEnumerable<ProductDto> Basket { get; }

	public AddToCurrentBasketRequestResultAction(IEnumerable<ProductDto> basket) 
		=> Basket = basket;
}
