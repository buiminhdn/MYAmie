using Common.Pagination;

namespace Common.ViewModels.AdminUserVMs;
public class PagedAdminUsersVM
{
    public IEnumerable<AdminUserVM> Users { get; set; }
    public PaginationData Pagination { get; set; }
}
