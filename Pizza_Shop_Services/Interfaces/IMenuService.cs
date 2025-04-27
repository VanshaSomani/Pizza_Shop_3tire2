using Microsoft.AspNetCore.Mvc.Rendering;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IMenuService
{
    public Task<(ItemCategoryViewModel obj , int totalRecord , int modifierTotalRecord)> GetCategoryAsync(int categoryId , string searchcriteria , int page , int pagesize);

    public Task<bool> AddCategoryAsync(ItemCategoryViewModel obj);

    public Task<bool> AddModifierGroupAsync(string Name , string Desc , List<int> mid);

    public Task<ItemCategoryViewModel> EditCategoryAsync(int categoryId);

    public Task<(ItemCategoryViewModel obj , bool status)> EditMGAsync(int MGid , List<int> mid = null);

    public Task<(ItemCategoryViewModel obj , bool status)> EditModifierAsync(int mid);

    public Task<(ItemCategoryViewModel obj , int ModifierTotalRecord)> GetModifiers(int MGId , string searchcriteria , int page , int pagesize);

    public Task<List<Modifier>> GetModifiresAddItems(int MgId);

    public Task<bool> UpdateCategory(ItemCategoryViewModel obj);

    public Task<bool> UpdateMG(ItemCategoryViewModel obj);

    public Task<bool> UpdateModifier(ItemCategoryViewModel obj);

    public Task<bool> DeleteCategoryAsync(ItemCategoryViewModel obj);

    public Task<bool> DeleteMGAsync(ItemCategoryViewModel obj);

    public Task<SelectList> GetAllCategoryAsync();
    
    public Task<ItemCategoryViewModel> GetAllMG();

    public Task<(ItemCategoryViewModel obj , int totalRecord)> GetAllModifiers(int page , int pagesize , string searchcriteria);

    public Task<SelectList> GetAllModifierGroupAsync();

    public Task<bool> AddItemsAsync(ItemCategoryViewModel obj);

    public Task<bool> DeleteItemAsync(ItemCategoryViewModel obj);

    public Task<ItemCategoryViewModel> EditItemAsync(int itemid);

    public Task<bool> UpdateItem(ItemCategoryViewModel obj);

    public Task DeleteMassItem(int itemid);

    public Task ModifierDeleteMass(List<int> modifierIds , int modifierGroupId);

    public Task<bool> AddModifierAsync(ItemCategoryViewModel obj);

    public Task<bool> DeletModifierAsync(int mid , int mgid);

    public Task AddModifierToMG(List<int> mid, int mgid);

    public Task<bool> DeleteModifierGroupInEditItem(List<int> modifierGroupIdList , int itemId);

    public Task<bool> EditAvailableItem(int ItemId , bool Status);
}
