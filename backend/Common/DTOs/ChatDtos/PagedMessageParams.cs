using Common.DTOs.Core;

namespace Common.DTOs.ChatDtos;
public class PagedMessageParams : BaseParams
{
    public int OtherUserId { get; set; }
    public int PageNumber { get; set; }
}
