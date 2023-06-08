using Fluxor;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Basket.Store;

[FeatureState]
public class BasketState
{
	public bool IsLoading { get; }

	public int BasketId { get; } = -1;

	public IEnumerable<ProductDto> Products { get; } = Enumerable.Empty<ProductDto>();

    private BasketState() { }

    public BasketState(bool isLoading, int basketId, IEnumerable<ProductDto> basket)
    {
		IsLoading = isLoading;
		Products = basket;
		BasketId = basketId;
    }
}
