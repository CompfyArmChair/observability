using ShopWebsite.Shared;

namespace ShopWebsite.Client.Features.Products.Store.Actions;

public class FetchProductsResultAction
{
    public IEnumerable<ProductDto> Products { get; }

    public FetchProductsResultAction(IEnumerable<ProductDto> products)
    {
        Products = products;
    }
}