using Fluxor;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Checkout.Store;

[FeatureState]
public class CheckoutState
{
	public bool IsLoading { get; }

	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address { get; set; }
	public string Expiration { get; set; }
	public string CreditCard { get; set; }
	public string Cvc { get; set; }

	public BasketDto Basket { get; set; }


	private CheckoutState() { }

	public CheckoutState(
		bool isLoading,		
		string email,
		string firstName,
		string lastName,
		string address,
		string expiration,
		string creditcard,
		string cvc,
		BasketDto basket)
	{
		IsLoading = isLoading;

		Email = email;
		FirstName = firstName;
		LastName = lastName;
		Address = address;
		Expiration = expiration;
		CreditCard = creditcard;
		Cvc = cvc;
		Basket = basket;
	}
}
