using Fluxor;
using ShopWebsite.Client.Features.Basket.Store.Actions;
using ShopWebsite.Client.Features.Checkout.Store.Actions;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Checkout.Store;

public class Reducers
{
	[ReducerMethod(typeof(CheckoutRequestAction))]
	public static CheckoutState CheckoutRequestAction(
		CheckoutState state) => new(
			isLoading: true,
			state.Email,
			state.FirstName,
			state.LastName,
			state.Address,
			state.Expiration,			
			state.CreditCard,
			state.Cvc,
			state.Basket);
	

	[ReducerMethod]
	public static CheckoutState CheckoutRequestResultAction(
		CheckoutState state,
		CheckoutRequestResultAction action) => new(
			isLoading: false,
			action.Purchase.Email,
			action.Purchase.FirstName,
			action.Purchase.LastName,
			action.Purchase.Address,
			action.Purchase.Expiration,			
			action.Purchase.CreditCard,
			action.Purchase.Cvc,
			action.Purchase.Basket);
}
