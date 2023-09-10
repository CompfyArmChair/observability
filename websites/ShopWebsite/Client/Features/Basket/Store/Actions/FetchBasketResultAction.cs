using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Basket.Store.Actions;

public class FetchBasketResultAction
{
	public BasketDto Basket { get; }

	public FetchBasketResultAction(BasketDto basket) 
		=> Basket = basket;
}
