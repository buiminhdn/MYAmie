using Models.Accounts;
using Models.Core;

namespace Models.Businesses;
public class Business : BaseModel
{
    /// <summary>
    /// Total number of views for the business
    /// </summary>
    public int ViewCount { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string OperatingHours { get; set; }
    public int OwnerId { get; set; }

    // Navigation Property
    public Account Owner { get; set; } // 1-to-1 with Account
}
