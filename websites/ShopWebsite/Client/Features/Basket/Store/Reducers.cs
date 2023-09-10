using Fluxor;
using ShopWebsite.Client.Features.Basket.Store.Actions;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Basket.Store;

public class Reducers
{
	[ReducerMethod(typeof(AddToCurrentBasketRequestAction))]
	public static BasketState ReduceAddToCurrentBasketRequestAction(
		BasketState state) => new(
			basketId: state.BasketId,
			isLoading: true,
			basket: state.Products);
	

	[ReducerMethod]
	public static BasketState ReduceAddToBasketRequestResultAction(
		BasketState state,
		AddToCurrentBasketRequestResultAction action) => new(
			basketId: state.BasketId,
			isLoading: false,
			basket: action.Basket);
	

	[ReducerMethod(typeof(FetchBasketAction))]
	public static BasketState ReduceFetchBasketAction(
		BasketState state) => new(
			basketId: state.BasketId,
			isLoading: true,
			basket: state.Products);
	

	[ReducerMethod]
	public static BasketState ReduceFetchBasketResultAction(
		BasketState state,
		FetchBasketResultAction action) => new(
			basketId: action.Basket.Id,
			isLoading: false,
			basket: action.Basket.Products);
}
