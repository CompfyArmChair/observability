using Fluxor;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Products.Store
{
	[FeatureState]
	public class ProductsState
	{
		public bool IsLoading { get; }

		public IEnumerable<ProductDto> Products { get; }

		private ProductsState()
		{
			Products = Enumerable.Empty<ProductDto>();
		}

		public ProductsState(bool isLoading, IEnumerable<ProductDto> products)
		{
			IsLoading = isLoading;
			Products = products ?? Enumerable.Empty<ProductDto>();
		}
	}
}
