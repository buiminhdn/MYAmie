using Common.Pagination;

namespace Common.ViewModels.UserVMs;
public class PagedBusinessesVM
{
    public IEnumerable<BusinessVM> Businesses { get; set; }
    public PaginationData Pagination { get; set; }
}
