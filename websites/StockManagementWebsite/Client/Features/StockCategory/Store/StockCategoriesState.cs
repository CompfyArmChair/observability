using Fluxor;
using StockManagementWebsite.Shared.StockCategories;

namespace StockManagementWebsite.Client.Features.StockCategory.Store;

[FeatureState]
public class StockCategoriesState
{
    public bool IsLoading { get; }
    public List<StockCategoryDto> Categories { get; }

    private StockCategoriesState() 
    { 
        Categories = new List<StockCategoryDto>();
    }

    public StockCategoriesState(bool isLoading, List<StockCategoryDto> categories)
    {
        IsLoading = isLoading;
        Categories = categories ?? new List<StockCategoryDto>();
    }
}