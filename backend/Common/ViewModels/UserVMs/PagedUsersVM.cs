
using Common.Pagination;

namespace Common.ViewModels.UserVMs;
public class PagedUsersVM
{
    public IEnumerable<UserVM> Users { get; set; }
    public PaginationData Pagination { get; set; }
}
