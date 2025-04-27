namespace Pizza_Shop_Repository.ViewModels;

public class PagginationViewModel
{
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalRecord { get; set; }
    public int TotalPage { get; set; }
    public int MinRow { get; set; }
    public int MaxRow { get; set; }
}
