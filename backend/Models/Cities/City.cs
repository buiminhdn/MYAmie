using Models.Accounts;
using Models.Core;
using Models.Places;

namespace Models.Cities;
public class City : BaseModel
{
    public string Name { get; set; }

    // Relationship
    public ICollection<Account> Accounts { get; set; } // 1-to-many with Accounts
    public ICollection<Place> Places { get; set; } // 1-to-many with Places
}
