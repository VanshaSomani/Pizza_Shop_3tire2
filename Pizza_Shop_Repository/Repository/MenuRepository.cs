using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class MenuRepository : IMenuRepository
{
    private readonly RmsdemoContext _db;
    public MenuRepository(RmsdemoContext db){
        _db = db;
    }

    public async Task<bool> AddCategory(Category obj)
    {
        try
        {
            _db.Categories.Add(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    
    public async Task<bool> AddModifierGroup(string name , string desc , List<int> mid , int userid){
        try
        {
            Modifiergroup newModifiergroup = new Modifiergroup();
            newModifiergroup.Mgname = name;
            newModifiergroup.Mgdesc = desc;
            newModifiergroup.Createdby = userid;
            newModifiergroup.Createdat = DateTime.Now;
            _db.Modifiergroups.Add(newModifiergroup);
            await _db.SaveChangesAsync();
            for(int i = 0 ; i < mid.Count ; i++){
                await AddModifierInEditMG(mid[i] , newModifiergroup.Modifiergroupid , userid);
            }
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task deleteModifierById(int modifierId , int userid , int ModifierGroupId){
        ModifierModifierGroupMapping mm = await _db.ModifierModifierGroupMappings.FirstOrDefaultAsync(mm => mm.ModifierGroupId == ModifierGroupId && mm.ModifierId == modifierId && mm.IsDeleted != true);
        mm.IsDeleted = true;
        await _db.SaveChangesAsync();
    }

    public async Task<List<Category>> GetAllCategory()
    {
        List<Category> item_Category = await _db.Categories.Where(c => c.Isdeleted != true).ToListAsync();
        return item_Category;
    }

    public async Task<List<Modifiergroup>> GetAllModifierGroup(){
        List<Modifiergroup> modifierGroup_list = await _db.Modifiergroups.Where(m => m.Isdeleted != true).ToListAsync();
        return modifierGroup_list;
    }

    public async Task<(List<Item> obj , int totalRecord)> GetCategoryItems(int categoryId , string searchcriteria , int page , int pagesize)
    {
        IQueryable<Item> itemsList = _db.Items
        .Where(i => i.Categoryid == categoryId && i.Isdeleted != true);
        
        if(!string.IsNullOrEmpty(searchcriteria)){
            itemsList = itemsList.Where(i => i.Itemname.ToLower().Contains(searchcriteria.ToLower()));
        }

        int totalRecord = await itemsList.CountAsync();

        List<Item> reducedItem2 = await itemsList
        .Skip((page-1)*pagesize)
        .Take(pagesize)
        .ToListAsync();
        
        return (reducedItem2 , totalRecord);
    }

    public async Task<(IEnumerable<Modifier> modifiers , int modifierTotalRecord)> GetModifiersForMG(int mgid , int page , int pagesize , string searchcriteria = null){
        IQueryable<Modifier> modifier_list = _db.ModifierModifierGroupMappings
        .Where(mm => mm.ModifierGroupId == mgid && mm.IsDeleted!= true && mm.Modifier.Isdeleted != true)
        .Include(m => m.Modifier)
        .ThenInclude(m => m.ModifierModifierGroupMappings)
        .Select(x=>x.Modifier);
        if(!string.IsNullOrEmpty(searchcriteria)){
            modifier_list = modifier_list.Where(m => m.Modifiername.ToLower().Contains(searchcriteria.ToLower()));
        }
        int totalRecord = await modifier_list.CountAsync();
        List<Modifier> reducesmodifire2 = await modifier_list.Skip((page-1)*pagesize).Take(pagesize).ToListAsync();
        return (reducesmodifire2 , totalRecord);
    }

    public async Task<(IEnumerable<Modifier> modifiers , int totalRecord)> GetModifierList(int page , int pagesize , string searchcriteria){
        IQueryable<Modifier> modifier_list = _db.Modifiers.Where(m => m.Isdeleted != true);
        if(!string.IsNullOrEmpty(searchcriteria)){
            modifier_list = modifier_list.Where(m => m.Modifiername.ToLower().Contains(searchcriteria.ToLower()));
        }
        int totalRecord = await modifier_list.CountAsync();
        List<Modifier> reduced_modifier_list = await modifier_list.Skip((page-1)*pagesize).Take(pagesize).ToListAsync();
        return (reduced_modifier_list , modifier_list.Count());
    }

    public async Task<Modifier> ChekModiferForAddModifier(string Modifiername){
        Modifier check = await _db.ModifierModifierGroupMappings
                    .Include(m => m.Modifier)
                    .Include(m => m.ModifierGroup)
                    .Where(mm => mm.IsDeleted!=true)
                    .Select(x => x.Modifier)
                    .FirstOrDefaultAsync(m => m.Modifiername == Modifiername);
        return check;
    }

    public async Task<bool> AddModifier(Modifier NewModifier , ModifierModifierGroupMapping newMofierMapping){
        try
        {
            _db.Modifiers.Add(NewModifier);
            await _db.SaveChangesAsync();
                
            newMofierMapping.ModifierId = NewModifier.Modifierid;
            _db.ModifierModifierGroupMappings.Add(newMofierMapping);
            await _db.SaveChangesAsync();  

            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task<(Modifier obj , bool status)> getMById(int mid){
        try
        {
            Modifier mo = await _db.Modifiers
            .Include(m => m.ModifierModifierGroupMappings)
            .FirstOrDefaultAsync(m => m.Modifierid == mid);
            return (mo , true);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);            
            return (null , false);
        }
    }

    public async Task<Category> getCategoryById(int categoryId){
        Category category = await _db.Categories
        .FirstOrDefaultAsync(c => c.Categoryid == categoryId);
        return category;
    }

    public async Task<(Modifiergroup modifierGroup , bool status)> getMgById(int MGid , List<int> mid){
        Modifiergroup modifierGroup = await _db.Modifiergroups
        .FirstOrDefaultAsync(mg => mg.Modifiergroupid == MGid);
        return (modifierGroup , true);
    }

    public async Task<List<ModifierModifierGroupMapping>> GetModifierModifierGroupMappingsAsync(int MGid){
        List<ModifierModifierGroupMapping> modifier_list = await _db.ModifierModifierGroupMappings
        .Include(m => m.Modifier).Where(mm => mm.ModifierGroupId == MGid && mm.IsDeleted != true)
        .ToListAsync();
        return modifier_list;
    }

    public async Task <List<Modifier>> GetModifierListByid(List<int> mid){
        List<Modifier> obj = new List<Modifier>();
        for(int i = 0 ; i < mid.Count() ; i++){
            Modifier mod = await _db.Modifiers.FirstOrDefaultAsync(m => m.Modifierid == mid[i]);
            if(mod != null){
                obj.Add(mod);
            }
        }
        return obj;
    }

    public async Task<List<ItemModifierGroup>> GetItemById(int itemid){
        List<ItemModifierGroup> im = await _db.ItemModifierGroups
        .Where(im => im.ItemId == itemid && im.Isdeleted!=true)
        .Include(mg => mg.ModifierGroup)
        .ThenInclude(mm => mm.ModifierModifierGroupMappings.Where(mm => mm.IsDeleted != true))
        .ThenInclude(m => m.Modifier)
        .Where(m => m.Isdeleted != true)
        .ToListAsync();
        
        return im;
    }

    public async Task<Category> GetCategoryAsync(int categoryId){
        return await _db.Categories.FirstOrDefaultAsync(c => c.Categoryid == categoryId && c.Isdeleted != true);
    }

    public async Task<bool> EditCategory(Category obj){
        try
        {
            _db.Categories.Update(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> EditMg(Modifiergroup obj){
        try
        {
            _db.Modifiergroups.Update(obj);
            _db.SaveChanges();
            return true;
        }
        catch (Exception )
        {
            // Console.WriteLine(e);
            return false;            
        }
    }

    public async Task<ModifierModifierGroupMapping> GetModifierGroupMappingForEditModifier(int modifierId , int modifierGroupId){
        ModifierModifierGroupMapping mod = await _db.ModifierModifierGroupMappings
        .Include(m => m.Modifier)
        .FirstOrDefaultAsync(m => m.ModifierGroupId == modifierGroupId && 
        m.ModifierId == modifierId && m.IsDeleted != true);
        return mod;
    }

    public async Task<bool> EditModifier(ModifierModifierGroupMapping mod , Modifier x){
        try
        {
            _db.ModifierModifierGroupMappings.Update(mod);
            _db.Modifiers.Update(x);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception )
        {
            // Console.WriteLine(e);            
            return false;
        }
    }

    public async Task<bool> DeleteCategory(Category obj){
        try
        {
             _db.Categories.Update(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task<Modifiergroup> GetModifierGroupAsync(int ModifierGroupID){
        return await _db.Modifiergroups.FirstOrDefaultAsync(m => m.Modifiergroupid == ModifierGroupID && m.Isdeleted!= true);
    }

    public async Task<bool> DeleteMg(Modifiergroup obj){
        try
        {
            _db.Modifiergroups.Update(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task<Item> GetItemAsync(int ItemId){
        return await _db.Items.FirstOrDefaultAsync(i => i.Itemid == ItemId && i.Isdeleted != true);
    }

    public async Task<bool> DeleteItem(Item obj){
        try
        {
            _db.Items.Update(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task<List<Category>> GetAllCategoryForAddItems(){
        List<Category> categories = _db.Categories.Where(c => c.Isdeleted != true).ToList();
        return categories;
    }

    public async Task<List<Modifiergroup>> GetAllModifierGroupForAddItems(){
        List<Modifiergroup> modifiergroups = _db.Modifiergroups.Where(m => m.Isdeleted != true).ToList();
        return modifiergroups;
    }

    public async Task AddItemModifierGroupMapping(ItemModifierGroup newItemModifier){    
        await _db.ItemModifierGroups.AddAsync(newItemModifier);
        await _db.SaveChangesAsync();
    }

    public async Task<Item> AddItem(Item newItem){
        _db.Items.AddAsync(newItem);
        await _db.SaveChangesAsync();
        return newItem;
    }

    public async Task EditItemUpdateDataOfItemModifierGroupMapping(ItemModifierGroup exist){
        _db.ItemModifierGroups.Update(exist);
        await _db.SaveChangesAsync();
    }

    public async Task<ItemModifierGroup> GetItemModifierGroupForEditItem(int itemId , int modifierGroupId){
        ItemModifierGroup exist = await _db.ItemModifierGroups
                .FirstOrDefaultAsync(im => im.ItemId == itemId && 
                im.ModifierGroupId == modifierGroupId && im.Isdeleted != true);

        return exist;
    }



    public async Task<Item> EditItem(Item editedItem){
        
            _db.Items.Update(editedItem);
            await _db.SaveChangesAsync();
            return editedItem;
            
    }

    public async Task<bool> DeleteModifierGrounItemMapping(List<int> ModifierGroupIdList , int itemId){
        try
        {
            for(int i = 0 ; i < ModifierGroupIdList.Count() ; i++){
                ItemModifierGroup exist = await _db.ItemModifierGroups
                .FirstOrDefaultAsync(im => im.ItemId == itemId && 
                im.ModifierGroupId == ModifierGroupIdList[i] && 
                im.Isdeleted != true);
                if(exist != null){
                    exist.Isdeleted = true;
                    await _db.SaveChangesAsync();
                }
            }
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task DeleteItemById(Item obj){
        try
        {
            _db.Items.Update(obj);
            await _db.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
        }
    }

    public async Task<bool> DeleteModifier(int mid , int mgid , int userid){
        try
        {
            ModifierModifierGroupMapping ModifierModifierMapping = await _db.ModifierModifierGroupMappings
            .FirstOrDefaultAsync(m => m.ModifierGroupId == mgid && 
            m.ModifierId == mid && m.IsDeleted != true);
            if(ModifierModifierMapping!=null){
                ModifierModifierMapping.IsDeleted = true;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    public async Task AddModifierInEditMG(int mid , int mgid , int userid){
        ModifierModifierGroupMapping check = await _db.ModifierModifierGroupMappings
        .FirstOrDefaultAsync(mm => mm.ModifierGroupId == mgid && 
        mm.ModifierId == mid && 
        mm.IsDeleted != true);
        if(check == null){
            ModifierModifierGroupMapping new_mm = new ModifierModifierGroupMapping();
            new_mm.ModifierId = mid;
            new_mm.ModifierGroupId = mgid;
            new_mm.IsDeleted = false;
            _db.ModifierModifierGroupMappings.Add(new_mm);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Modifier>> GetModifiresForMgId(int MgId){

        List<ModifierModifierGroupMapping> modifier_list = await _db.ModifierModifierGroupMappings
        .Include(m => m.Modifier)
        .Where(m => m.ModifierGroupId == MgId && 
        m.IsDeleted != true)
        .ToListAsync();
        List<Modifier> modifiers = modifier_list.Select(x => x.Modifier).Where(m => m.Isdeleted != true).ToList();
        return modifiers;
    }

    public async Task<bool> AvailableItemStatus(int ItemId , bool Status , int userid){
        try
        {
            Item obj = await _db.Items.FirstOrDefaultAsync(i => i.Itemid == ItemId && i.Isdeleted != true);
            obj.Availlable = Status;
            if(Status == false){
                obj.Quantity = 0;
            }
            obj.Updatedby = userid;
            obj.Updatedat = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);   
            return false;
        }
    }

}