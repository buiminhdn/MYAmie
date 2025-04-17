using Models.Accounts;
using Models.Core;
using Models.Places;

namespace Models.Categories;
public class Category : BaseModel
{
    public string Name { get; set; }
    public string Icon { get; set; }

    // Relationship
    public ICollection<Account> Accounts { get; set; } // Many-to-many with Accounts
    public ICollection<Place> Places { get; set; } // Many-to-many with Places
}
