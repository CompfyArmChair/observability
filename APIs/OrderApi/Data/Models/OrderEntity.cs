﻿using OrderApi.Enums;

namespace OrderApi.Data.Models;

public class OrderEntity
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Expiration { get; set; } = string.Empty;
	public string Creditcard { get; set; } = string.Empty;
	public string Cvc { get; set; } = string.Empty;
	public List<ProductEntity> ProductEntities { get; set; } = new();
    public Status Status { get; set; }
    public string CustomerReference { get; set; } = string.Empty;
}
