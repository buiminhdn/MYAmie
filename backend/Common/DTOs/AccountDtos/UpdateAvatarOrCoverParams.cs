using Common.DTOs.Core;

namespace Common.DTOs.AccountDtos;
public class UpdateAvatarOrCoverParams : BaseParams
{
    public ImageTypeParam Type { get; set; }
}

public enum ImageTypeParam
{
    Avatar = 1,
    Cover = 2
}