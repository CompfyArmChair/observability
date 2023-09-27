using StockManagementWebsite.Shared.StockCategories;

namespace ShopWebsite.Client.Features.Products.Store.Actions;

public class FetchStockCategoriesResultAction
{
    public IEnumerable<StockCategoryDto> Stock { get; }

    public FetchStockCategoriesResultAction(IEnumerable<StockCategoryDto> stock)
    {
        Stock = stock;
    }
}