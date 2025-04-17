using Utility;

namespace Models.Core;
public abstract class BaseModel
{
    public int Id { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public long CreatedDate { get; set; } = DateTimeUtils.TimeInEpoch();
    public long UpdatedDate { get; set; } = DateTimeUtils.TimeInEpoch();

    public bool IsOwner(int currentUserId)
    {
        return CreatedBy == currentUserId;
    }

}
