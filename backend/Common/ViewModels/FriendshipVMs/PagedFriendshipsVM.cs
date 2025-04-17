using Common.Pagination;

namespace Common.ViewModels.FriendshipVMs;
public class PagedFriendshipsVM
{
    public IEnumerable<FriendshipVM> Friends { get; set; }

    public PaginationData Pagination { get; set; }
}
