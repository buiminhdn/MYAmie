using Models.Businesses;
using Models.Categories;
using Models.Cities;
using Models.Core;
using Models.Feedbacks;
using Models.Friendships;
using Models.Messages;
using Models.Places;

namespace Models.Accounts;

public class Account : BaseModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ShortDescription { get; set; }
    public string NormalizedInfo { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
    public string Cover { get; set; }
    public long DateOfBirth { get; set; }
    public string Images { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Characteristics { get; set; }
    public Role Role { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.ACTIVATED;
    public bool IsEmailVerified { get; set; } = false;
    public string RefreshToken { get; set; }
    public long RefreshTokenExpiryTime { get; set; }
    public int? CityId { get; set; }


    // Navigation Properties
    public City City { get; set; } // 1-to-1 with City
    public Business Business { get; set; } // Optional 1-to-1 with Business
    public ICollection<Place> Places { get; set; } // 1-to-many with Places
    public ICollection<Feedback> SentFeedbacks { get; set; } // 1-to-many with Feedbacks (as sender)
    public ICollection<Friendship> RequestedFriendships { get; set; } // 1-to-many with Friendships (as requester)
    public ICollection<Friendship> ReceivedFriendships { get; set; } // 1-to-many with Friendships (as requestee)
    public ICollection<Message> SentMessages { get; set; }  // 1-to-many with Messages (as sender)
    public ICollection<Message> ReceivedMessages { get; set; } // 1-to-many with Messages (as receiver)
    public ICollection<Category> Categories { get; set; }   // Many-to-many with Categories

}
