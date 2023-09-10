using Fluxor;
using ShopWebsite.Client.Features.Products.Store.Actions;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Products.Store;

public class Reducers
{
	[ReducerMethod(typeof(FetchProductsAction))]
	public static ProductsState ReduceFetchDataAction(ProductsState _) =>
	  new(
		isLoading: true,
		products: Enumerable.Empty<ProductDto>());

	[ReducerMethod]
	public static ProductsState ReduceFetchProductsResultAction(
		ProductsState _,
		FetchProductsResultAction action) =>
		  new(
			isLoading: false,
			products: action.Products);
}
