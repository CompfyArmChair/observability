using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Checkout.Store.Actions;

public class CheckoutRequestResultAction
{
	public PurchaseDto Purchase { get; }

	public CheckoutRequestResultAction(PurchaseDto purchase) 
		=> Purchase = purchase;
}
