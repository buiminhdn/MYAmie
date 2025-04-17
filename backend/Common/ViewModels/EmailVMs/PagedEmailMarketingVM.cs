using Common.Pagination;

namespace Common.ViewModels.EmailVMs;
public class PagedEmailMarketingVM
{
    public IEnumerable<EmailMarketingVM> Emails { get; set; }
    public PaginationData Pagination { get; set; }
}
