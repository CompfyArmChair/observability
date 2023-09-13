using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class AddStockCategoryAction
{
    public AddStockCategoryDto Category { get; }

    public AddStockCategoryAction(AddStockCategoryDto category)
    {
        Category = category;
    }
}
