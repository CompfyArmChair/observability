using Fluxor;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Checkout.Store;

[FeatureState]
public class CheckoutState
{
	public bool IsLoading { get; }

	public string Email { get; set; } = "email@address.com";
	public string FirstName { get; set; } = "Sue";
	public string LastName { get; set; } = "Lou";
	public string Address { get; set; } = "Road";
	public string Expiration { get; set; } = "05/05";
	public string CreditCard { get; set; } = "1111222233334444";
	public string Cvc { get; set; } = "123";

	public BasketDto Basket { get; set; } = default!;


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
