using DAL.Repository.Core;
using Models.Friendships;

namespace DAL.Repository.IRepository;
public interface IFriendshipRepo : IBaseRepo<Friendship>
{
    Task<bool> IsFriend(int currentUserId, int otherUserId);
}