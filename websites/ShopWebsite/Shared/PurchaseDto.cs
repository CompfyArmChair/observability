namespace ShopWebsite.Shared;

public record PurchaseDto(
	string Email,
	string FirstName,
	string LastName,
	string Address,
	string Expiration,
	string CreditCard,
	string Cvc,
	BasketDto Basket);
