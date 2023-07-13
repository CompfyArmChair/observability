using Fluxor;
using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Checkout.Store;

[FeatureState]
public class CheckoutState
{
	public bool IsLoading { get; }

	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Expiration { get; set; } = string.Empty;
	public string CreditCard { get; set; } = string.Empty;
	public string Cvc { get; set; } = string.Empty;

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
