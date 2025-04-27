using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IHttpContextAccessor _httpaccessor;
    private readonly IWebHostEnvironment _env;

    public MenuService(IMenuRepository menuRepository, IHttpContextAccessor httpaccessor, IWebHostEnvironment env)
    {
        _menuRepository = menuRepository;
        _httpaccessor = httpaccessor;
        _env = env;
    }

    #region AddCategoryAsync    
    public async Task<bool> AddCategoryAsync(ItemCategoryViewModel obj)
    {
        try
        {
            Category newCategory = new Category();
            newCategory.Categoryname = obj.Categoryname;
            newCategory.Categorydesc = obj.CategoryDesc;
            newCategory.Isdeleted = false;
            newCategory.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            newCategory.Createdat = DateTime.Now;
            return await _menuRepository.AddCategory(newCategory);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region AddModifierGroupAsync
    public async Task<bool> AddModifierGroupAsync(string Name, string Desc, List<int> mid)
    {
        try
        {
            return await _menuRepository.AddModifierGroup(Name, Desc, mid, Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region ModifierDeleteMass
    public async Task ModifierDeleteMass(List<int> modifierIds, int modifierGroupId)
    {
        for (int i = 0; i < modifierIds.Count; i++)
        {
            await _menuRepository.deleteModifierById(modifierIds[i], Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")), modifierGroupId);
        }
    }
    #endregion

    #region GetCategoryAsync 
    public async Task<(ItemCategoryViewModel obj, int totalRecord, int modifierTotalRecord)> GetCategoryAsync(int categoryId, string searchcriteria, int page, int pagesize)
    {
        ItemCategoryViewModel obj =  new ItemCategoryViewModel();
        obj.CategoryList = await _menuRepository.GetAllCategory();
        ItemCategoryViewModel obj2 = obj;
        obj2.ModifiergroupList = await _menuRepository.GetAllModifierGroup();
        if (categoryId != 0)
        {
            ItemCategoryViewModel itemobj1 = obj2;
            (itemobj1.ItemList , int totalRecord1) = await _menuRepository.GetCategoryItems(categoryId, searchcriteria, page, pagesize);
            obj.ItemPaggination = new PagginationViewModel
            {
                CurrentPage = page,
                PageSize = pagesize,
                TotalRecord = totalRecord1,
                TotalPage = (int)Math.Ceiling((double)totalRecord1 / pagesize),
                MinRow = ((page - 1) * pagesize) + 1,
                MaxRow = ((page - 1) * pagesize) + obj.ItemList.Count()
            };
            (itemobj1.ModifiersList, int ModifierTotalRecord) = await _menuRepository.GetModifiersForMG(itemobj1.ModifiergroupList.First().Modifiergroupid, page, pagesize);
            obj.ModifierPaggination = new PagginationViewModel
            {
                CurrentPage = page,
                PageSize = pagesize,
                TotalRecord = ModifierTotalRecord,
                TotalPage = (int)Math.Ceiling((double)ModifierTotalRecord / pagesize),
                MinRow = ((page - 1) * pagesize) + 1,
                MaxRow = ((page - 1) * pagesize) + obj.ModifiersList.Count()
            };
            return (itemobj1, totalRecord1, ModifierTotalRecord);
        }
        obj2.Categoryid = obj2.CategoryList.First().Categoryid;
        obj2.Modifiergroupid = obj2.ModifiergroupList.First().Modifiergroupid;
        ItemCategoryViewModel itemobj2 = obj2;
        (itemobj2.ItemList , int totalRecord2) = await _menuRepository.GetCategoryItems(obj2.CategoryList.First().Categoryid, searchcriteria, page, pagesize);
        obj.ItemPaggination = new PagginationViewModel
        {
            CurrentPage = page,
            PageSize = pagesize,
            TotalRecord = totalRecord2,
            TotalPage = (int)Math.Ceiling((double)totalRecord2 / pagesize),
            MinRow = ((page - 1) * pagesize) + 1,
            MaxRow = ((page - 1) * pagesize) + obj.ItemList.Count()
        };
        (itemobj2.ModifiersList, int modifireTotalRecord) = await _menuRepository.GetModifiersForMG(itemobj2.ModifiergroupList.First().Modifiergroupid, page, pagesize);
        obj.ModifierPaggination = new PagginationViewModel
        {
            CurrentPage = page,
            PageSize = pagesize,
            TotalRecord = modifireTotalRecord,
            TotalPage = (int)Math.Ceiling((double)modifireTotalRecord / pagesize),
            MinRow = ((page - 1) * pagesize) + 1,
            MaxRow = ((page - 1) * pagesize) + obj.ModifiersList.Count()
        };
        return (itemobj2, totalRecord2, modifireTotalRecord);
    }
    #endregion

    #region GetAllMG
    public async Task<ItemCategoryViewModel> GetAllMG()
    {
        ItemCategoryViewModel obj = new ItemCategoryViewModel();
        obj.ModifiergroupList = await _menuRepository.GetAllModifierGroup();
        return obj;
    }
    #endregion

    #region GetModifiers
    public async Task<(ItemCategoryViewModel obj, int ModifierTotalRecord)> GetModifiers(int MGId, string searchcriteria, int page, int pagesize)
    {
        ItemCategoryViewModel obj = new ItemCategoryViewModel();
        (obj.ModifiersList, int modifireTotalRecord) = await _menuRepository.GetModifiersForMG(MGId, page, pagesize, searchcriteria);
        obj.ModifierPaggination = new PagginationViewModel
        {
            CurrentPage = page,
            PageSize = pagesize,
            TotalRecord = modifireTotalRecord,
            TotalPage = (int)Math.Ceiling((double)modifireTotalRecord / pagesize),
            MinRow = ((page - 1) * pagesize) + 1,
            MaxRow = ((page - 1) * pagesize) + obj.ModifiersList.Count()
        };
        return (obj, modifireTotalRecord);
    }
    #endregion

    #region GetModifiresAddItems
    public async Task<List<Modifier>> GetModifiresAddItems(int MgId)
    {
        return await _menuRepository.GetModifiresForMgId(MgId);
    }
    #endregion

    #region GetAllModifiers
    public async Task<(ItemCategoryViewModel obj, int totalRecord)> GetAllModifiers(int page, int pagesize, string searchcriteria)
    {
        ItemCategoryViewModel obj = new ItemCategoryViewModel();
        (obj.ModifiersList, int totalRecord) = await _menuRepository.GetModifierList(page, pagesize, searchcriteria);
        obj.ModifierPaggination = new PagginationViewModel
        {
            CurrentPage = page,
            PageSize = pagesize,
            TotalRecord = totalRecord,
            TotalPage = (int)Math.Ceiling((double)totalRecord / pagesize),
            MinRow = ((page - 1) * pagesize) + 1,
            MaxRow = ((page - 1) * pagesize) + obj.ModifiersList.Count()
        };
        return (obj, totalRecord);
    }
    #endregion

    #region EditCategoryAsync
    public async Task<ItemCategoryViewModel> EditCategoryAsync(int categoryId)
    {
        Category category = await _menuRepository.getCategoryById(categoryId);
        ItemCategoryViewModel obj = new ItemCategoryViewModel();
        obj.Categoryid = category.Categoryid;
        obj.Categoryname = category.Categoryname;
        obj.CategoryDesc = category.Categorydesc;
        return obj;
    }
    #endregion

    #region EditMGAsync
    public async Task<(ItemCategoryViewModel obj, bool status)> EditMGAsync(int MGid, List<int> mid)
    {
        (Modifiergroup modifierGroup ,bool status) = await _menuRepository.getMgById(MGid, mid);
        ItemCategoryViewModel obj = new ItemCategoryViewModel();
        obj.Modifiergroupid = modifierGroup.Modifiergroupid;
        obj.ModifierGroupName = modifierGroup.Mgname;
        obj.ModifierGroupDesc = modifierGroup.Mgdesc;

        List<ModifierModifierGroupMapping> modifier_list = await _menuRepository.GetModifierModifierGroupMappingsAsync(MGid);
        obj.ModifiersList = modifier_list.Select(x => x.Modifier);
        if(mid.Count != 0){
            obj.ModifiersList = obj.ModifiersList.Concat(await  _menuRepository.GetModifierListByid(mid));
        }
        
        return (obj, status);
    }
    #endregion

    #region EditModifierAsync
    public async Task<(ItemCategoryViewModel obj, bool status)> EditModifierAsync(int mid)
    {
        (Modifier mo , bool status) = await _menuRepository.getMById(mid);
        ItemCategoryViewModel obj = new ItemCategoryViewModel();
        obj.Modifiergroupid = mo.ModifierModifierGroupMappings.First().ModifierGroupId;
        obj.ModifierId = mo.Modifierid;
        obj.Modifiername = mo.Modifiername;
        obj.Rate = mo.Rate;
        obj.Quantity = mo.Quantity;
        obj.Unit = mo.Unit;
        obj.Modifierdesc = mo.Modifierdesc;
        return (obj , status);
    }
    #endregion

    #region UpdateModifier
    public async Task<bool> UpdateModifier(ItemCategoryViewModel obj)
    {
        try
        {
            ModifierModifierGroupMapping mod = await _menuRepository.GetModifierGroupMappingForEditModifier(obj.ModifierId , obj.Modifiergroupid);
            if(mod != null){
                mod.ModifierGroupId = obj.Modifiergroupid;
                (Modifier x , bool status) = await _menuRepository.getMById(obj.ModifierId);
                x.Modifiername = obj.Modifiername;
                x.Rate = obj.Rate;
                x.Quantity = obj.Quantity;
                x.Unit = obj.Unit;
                x.Modifierdesc = obj.Modifierdesc;
                x.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                x.Updatedat = DateTime.Now;
                return await _menuRepository.EditModifier(mod,x);
            }
            return false;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region AddModifierToMG
    public async Task AddModifierToMG(List<int> mid, int mgid)
    {
        for (int i = 0; i < mid.Count; i++)
        {
            await _menuRepository.AddModifierInEditMG(mid[i], mgid, Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
        }
    }
    #endregion

    #region EditItemAsync
    public async Task<ItemCategoryViewModel> EditItemAsync(int itemid)
    {
            Item item = await _menuRepository.GetItemAsync(itemid);
            ItemCategoryViewModel obj = new ItemCategoryViewModel();
            obj.Itemid = item.Itemid;
            obj.Categoryid = item.Categoryid;
            obj.Categoryid = item.Categoryid;
            obj.Itemname = item.Itemname;
            obj.ItemType = item.ItemType;
            obj.Rate = item.Rate;
            obj.Quantity = item.Quantity;
            obj.Unit = item.Unit;
            obj.Availlable = item.Availlable;
            obj.Defaulttax = item.Defaulttax;
            obj.Taxpercentage = (double)item.Taxpercentage;
            obj.Code = item.Code;
            obj.Itemdesc = item.Itemdesc;
            obj.ExistingFile = item.Imgfile;

            obj.itemModifierGroupsList = await _menuRepository.GetItemById(itemid);
            
            return obj;
    }
    #endregion

    #region UpdateCategory
    public async Task<bool> UpdateCategory(ItemCategoryViewModel obj)
    {
        try
        {
            Category category = await _menuRepository.GetCategoryAsync(obj.Categoryid);
            category.Categoryname = obj.Categoryname;
            category.Categorydesc = obj.CategoryDesc;
            category.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            category.Updatedat = DateTime.UtcNow;
            return await _menuRepository.EditCategory(category);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region UpdateMG
    public async Task<bool> UpdateMG(ItemCategoryViewModel obj)
    {
        try
        {
            Modifiergroup mg = await _menuRepository.GetModifierGroupAsync(obj.Modifiergroupid);
            mg.Mgname = obj.ModifierGroupName;
            mg.Mgdesc = obj.ModifierGroupDesc;
            mg.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            mg.Updatedat = DateTime.Now;
            return await _menuRepository.EditMg(mg);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region DeleteCategoryAsync
    public async Task<bool> DeleteCategoryAsync(ItemCategoryViewModel obj)
    {
        try
        {
            Category deleteCategory = await _menuRepository.GetCategoryAsync(obj.Categoryid);
            deleteCategory.Isdeleted = true;
            deleteCategory.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            deleteCategory.Updatedat = DateTime.UtcNow;
            return await _menuRepository.DeleteCategory(deleteCategory);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region DeleteMGAsync
    public async Task<bool> DeleteMGAsync(ItemCategoryViewModel obj)
    {
        try
        {
            Modifiergroup mg = await _menuRepository.GetModifierGroupAsync(obj.Modifiergroupid);
            mg.Isdeleted = true;
            mg.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            mg.Updatedat = DateTime.Now;
            return await _menuRepository.DeleteMg(mg);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region DeleteItemAsync
    public async Task<bool> DeleteItemAsync(ItemCategoryViewModel obj)
    {
        try
        {
            Item DeleteItem = await _menuRepository.GetItemAsync(obj.Itemid);
            DeleteItem.Isdeleted = true;
            DeleteItem.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            DeleteItem.Updatedat = DateTime.Now;    
            return await _menuRepository.DeleteItem(DeleteItem);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region GetAllCategoryAsync
    public async Task<SelectList> GetAllCategoryAsync()
    {
        SelectList category = new SelectList(await _menuRepository.GetAllCategoryForAddItems(), "Categoryid", "Categoryname");
        return category;
    }
    #endregion

    #region GetAllModifierGroupAsync
    public async Task<SelectList> GetAllModifierGroupAsync()
    {
        SelectList modifierGroup = new SelectList(await _menuRepository.GetAllModifierGroupForAddItems(), "Modifiergroupid", "Mgname");
        return modifierGroup;
    }
    #endregion

    #region AddItems
    public async Task<bool> AddItemsAsync(ItemCategoryViewModel obj)
    {
        try
        {
            var filepath = "";
            var filename = "";
            if (obj.Imgfile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "ItemImages");
                filename = Guid.NewGuid().ToString() + "_" + obj.Imgfile.FileName;
                filepath = Path.Combine(folder, filename);
                obj.Imgfile.CopyTo(new FileStream(filepath, FileMode.Create));
            }

            //add data to items
            Item newItem = new Item();
            newItem.Categoryid = obj.Categoryid;
            newItem.Itemname = obj.Itemname;
            newItem.ItemType = obj.ItemType;
            newItem.Rate = obj.Rate;
            newItem.Quantity = obj.Quantity;
            newItem.Unit = obj.Unit;
            newItem.Availlable = obj.Availlable;
            newItem.Defaulttax = obj.Defaulttax;
            newItem.Taxpercentage = obj.Taxpercentage;
            newItem.Code = obj.Code;
            newItem.Itemdesc = obj.Itemdesc;
            newItem.Imgfile = filename;
            //newItem.Modifiergroupid = obj.Modifiergroupid;
            newItem.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            newItem.Createdat = DateTime.Now;

            Item i = await _menuRepository.AddItem(newItem);         

            //add item to itemmodifiergroupmapping
            foreach(var n in obj.ItemModifierGroupMapList){
                ItemModifierGroup newItemModifier = new ItemModifierGroup();
                newItemModifier.ItemId = newItem.Itemid;
                newItemModifier.ModifierGroupId = n.ModifierGroupId;
                newItemModifier.Min = n.ModifierGroupMinVal;
                newItemModifier.Max = n.ModifierGroupMaxVal;
                newItemModifier.Isdeleted = false;
                await _menuRepository.AddItemModifierGroupMapping(newItemModifier);
            }

            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    public async Task<bool> AddModifierAsync(ItemCategoryViewModel obj)
    {
        try
        {
            Modifier check = await _menuRepository.ChekModiferForAddModifier(obj.Modifiername);
            if(check == null){
                Modifier NewModifier = new Modifier();
                NewModifier.Modifiername = obj.Modifiername;
                NewModifier.Rate = obj.Rate;
                NewModifier.Quantity = obj.Quantity;
                NewModifier.Unit = obj.Unit;
                NewModifier.Modifierdesc = obj.Modifierdesc;
                NewModifier.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                NewModifier.Createdat = DateTime.Now;

                ModifierModifierGroupMapping newMofierMapping = new ModifierModifierGroupMapping();
                newMofierMapping.ModifierGroupId = obj.Modifiergroupid;
                newMofierMapping.IsDeleted = false;

                return await _menuRepository.AddModifier(NewModifier, newMofierMapping);
            }
            return false;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }

    #region DeletModifierAsync
    public async Task<bool> DeletModifierAsync(int mid, int mgid)
    {
        return await _menuRepository.DeleteModifier(mid, mgid, Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
    }
    #endregion

    #region DeleteModifierGroupInEditItem
    public async Task<bool> DeleteModifierGroupInEditItem(List<int> modifierGroupIdList, int itemId)
    {
        return await _menuRepository.DeleteModifierGrounItemMapping(modifierGroupIdList, itemId);
    }
    #endregion

    #region UpdateItem
    public async Task<bool> UpdateItem(ItemCategoryViewModel obj)
    {
        try
        {
            var filepath = "";
            var filename = "";
            if (obj.Imgfile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "ItemImages");
                filename = Guid.NewGuid().ToString() + "_" + obj.Imgfile.FileName;
                filepath = Path.Combine(folder, filename);
                obj.Imgfile.CopyTo(new FileStream(filepath, FileMode.Create));
            }

            //update item in items table
            Item editedItem = await _menuRepository.GetItemAsync(obj.Itemid);
            editedItem.Categoryid = obj.Categoryid;
            editedItem.Itemname = obj.Itemname;
            editedItem.ItemType = obj.ItemType;
            editedItem.Rate = obj.Rate;
            editedItem.Quantity = obj.Quantity;
            editedItem.Unit = obj.Unit;
            editedItem.Availlable = obj.Availlable;
            editedItem.Defaulttax = obj.Defaulttax;
            editedItem.Taxpercentage = obj.Taxpercentage;
            editedItem.Code = obj.Code;
            editedItem.Itemdesc = obj.Itemdesc;
            if(filename != ""){
                editedItem.Imgfile = filename;
            }
            // editedItem.Modifiergroupid = obj.Modifiergroupid;
            editedItem.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            editedItem.Updatedat = DateTime.Now;

            Item i = await _menuRepository.EditItem(editedItem);

            foreach(var n in obj.ItemModifierGroupMapList){
                ItemModifierGroup exist = await _menuRepository.GetItemModifierGroupForEditItem(n.ItemId , n.ModifierGroupId);

                if(exist != null){
                    exist.Min = n.ModifierGroupMinVal;
                    exist.Max = n.ModifierGroupMaxVal;
                    await _menuRepository.EditItemUpdateDataOfItemModifierGroupMapping(exist);
                }
                else{
                    ItemModifierGroup newItemModifier = new ItemModifierGroup();
                    newItemModifier.ItemId = n.ItemId;
                    newItemModifier.ModifierGroupId = n.ModifierGroupId;
                    newItemModifier.Min = n.ModifierGroupMinVal;
                    newItemModifier.Max = n.ModifierGroupMaxVal;
                    newItemModifier.Isdeleted = false;
                    await _menuRepository.AddItemModifierGroupMapping(newItemModifier);
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
    #endregion

    #region DeleteMassItem
    public async Task DeleteMassItem(int itemid)
    {
        try
        {
            Item DeleteItem = await _menuRepository.GetItemAsync(itemid);
            DeleteItem.Isdeleted = true;
            DeleteItem.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            DeleteItem.Updatedat = DateTime.Now;
            await _menuRepository.DeleteItemById(DeleteItem);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
        }
    }
    #endregion

    #region EditAvailableItem
        public async Task<bool> EditAvailableItem(int ItemId , bool Status){
            return await _menuRepository.AvailableItemStatus(ItemId , Status , Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
        }
    #endregion

}
