using Common.Pagination;

namespace Common.ViewModels.AdminPlaceVMs;
public class PagedAdminPlacesVM
{
    public IEnumerable<AdminPlaceVM> Places { get; set; }

    public PaginationData Pagination { get; set; }
}