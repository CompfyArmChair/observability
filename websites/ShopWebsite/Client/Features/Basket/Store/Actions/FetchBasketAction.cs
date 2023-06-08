namespace ShopWebsite.Client.Features.Basket.Store.Actions;

public class FetchBasketAction
{
	public FetchBasketAction(int basketId)
    {
		BasketId = basketId;
	}

	public int BasketId { get; }
}
