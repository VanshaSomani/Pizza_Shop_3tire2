using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IMenuRepository
{
    public Task<List<Category>> GetAllCategory();

    public Task<List<Modifiergroup>> GetAllModifierGroup();

    public Task<bool> AddCategory(Category obj);

    public Task<bool> AddModifierGroup(string name , string desc , List<int> mid, int userid);

    public Task<Category> getCategoryById(int categoryId);

    public Task<(Modifiergroup modifierGroup , bool status)> getMgById(int MGid , List<int> mid);

    public Task<(Modifier obj , bool status)> getMById(int mid);

    public Task <List<Modifier>> GetModifierListByid(List<int> mid);

    public Task<List<ModifierModifierGroupMapping>> GetModifierModifierGroupMappingsAsync(int MGid);

    public Task<Category> GetCategoryAsync(int categoryId);

    public Task<bool> EditCategory(Category obj);

    public Task<bool> EditMg(Modifiergroup obj);

    public Task<ModifierModifierGroupMapping> GetModifierGroupMappingForEditModifier(int modifierId , int modifierGroupId);

    public Task<bool> EditModifier(ModifierModifierGroupMapping mod , Modifier x);

    public Task<bool> DeleteCategory(Category obj);

    public Task<Modifiergroup> GetModifierGroupAsync(int ModifierGroupID);

    public Task<bool> DeleteMg(Modifiergroup obj); 

    public Task<(List<Item> obj , int totalRecord)> GetCategoryItems(int categoryId , string searchcriteria , int page , int pagesize);

    public Task<(IEnumerable<Modifier> modifiers , int modifierTotalRecord)> GetModifiersForMG(int mgid , int page , int pagesize , string searchcriteria = null );

    public Task<(IEnumerable<Modifier> modifiers , int totalRecord)> GetModifierList(int page , int pagesize , string searchcriteria);

    public Task<List<Category>> GetAllCategoryForAddItems();

    public Task<List<Modifiergroup>> GetAllModifierGroupForAddItems();

    public Task AddItemModifierGroupMapping(ItemModifierGroup newItemModifier);

    public Task<Item> AddItem(Item newItem);

    public Task<Modifier> ChekModiferForAddModifier(string Modifiername);

    public Task<bool> AddModifier(Modifier NewModifier , ModifierModifierGroupMapping newMofierMapping);

    public Task<Item> GetItemAsync(int ItemId);

    public Task<bool> DeleteItem(Item obj);

    public Task<List<ItemModifierGroup>> GetItemById(int itemid);

    public Task<ItemModifierGroup> GetItemModifierGroupForEditItem(int itemId , int modifierGroupId);

    public Task EditItemUpdateDataOfItemModifierGroupMapping(ItemModifierGroup exist);

    public Task<Item> EditItem(Item editedItem);

    public Task DeleteItemById(Item obj);

    public Task<bool> DeleteModifier(int mid , int mgid, int userid);

    public Task AddModifierInEditMG(int mid , int mgid , int userid);

    public Task<List<Modifier>> GetModifiresForMgId(int MgId);

    public Task deleteModifierById(int modifierId , int userid , int ModifierGroupId);

    public Task<bool> DeleteModifierGrounItemMapping(List<int> ModifierGroupIdList , int itemId);

    public Task<bool> AvailableItemStatus(int ItemId , bool Status , int userid);
}
