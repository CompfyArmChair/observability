using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class EditStockCategoryAction
{
    public EditStockCategoryDto Category { get; }

    public EditStockCategoryAction(EditStockCategoryDto category)
    {
        Category = category;
    }
}
