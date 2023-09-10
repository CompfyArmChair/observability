using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Checkout.Store.Actions;

public class CheckoutRequestAction
{
	public CheckoutRequestAction(PurchaseDto purchase)
	{		
		Purchase = purchase;
	}

	public PurchaseDto Purchase { get; }	
}
