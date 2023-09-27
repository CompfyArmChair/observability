using StockManagementWebsite.Shared.StockCategories;

namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class EditStockCategoryAction
{
    public EditStockCategoryDto Category { get; }

    public EditStockCategoryAction(EditStockCategoryDto category)
    {
        Category = category;
    }
}
