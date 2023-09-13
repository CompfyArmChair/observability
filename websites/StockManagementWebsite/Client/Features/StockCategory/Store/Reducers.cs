using Fluxor;
using StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

namespace StockManagementWebsite.Client.Features.StockCategory.Store;

public class StockCategoryReducers
{
    [ReducerMethod]
    public static StockCategoriesState ReduceStockCategoryAddedResultAction(
        StockCategoriesState state,
        StockCategoryAddedResultAction action)
    {
        var newList = state.Categories.ToList();
        newList.Add(action.AddedCategory);
        return new StockCategoriesState(false, newList);
    }

    [ReducerMethod]
    public static StockCategoriesState ReduceStockCategoryEditedResultAction(
        StockCategoriesState state,
        StockCategoryEditedResultAction action)
    {
        var newList = state.Categories.ToList();
        var category = newList.FirstOrDefault(c => c.Id == action.EditedCategory.Id);
        if (category != null)
        {
            category = action.EditedCategory;
        }
        return new StockCategoriesState(false, newList);
    }

    [ReducerMethod]
    public static StockCategoriesState ReduceStockCategoryRemovedResultAction(
    StockCategoriesState state,
    StockCategoryRemovedResultAction action)
    {
        var newList = state.Categories.ToList();
        var categoryToRemove = newList.FirstOrDefault(c => c.Id == action.RemovedCategoryId);
        if (categoryToRemove != null)
        {
            newList.Remove(categoryToRemove);
        }
        return new StockCategoriesState(false, newList);
    }

}

