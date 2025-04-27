namespace Pizza_Shop_Repository.ViewModels;

public class KOTViewModel
{
    public List<KOTCategory> KOTCategorieList { get; set; }
    public List<KOTCardDetailsViewModel> KOTCardData { get; set; }
    // public String SelectedCategoryName { get; set; }

    public class KOTCategory{
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
