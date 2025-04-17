using Common.Pagination;

namespace Common.ViewModels.PlaceVMs;
public class PagedPlacesVM
{
    public IEnumerable<PlaceVM> Places { get; set; }

    public PaginationData Pagination { get; set; }
}
