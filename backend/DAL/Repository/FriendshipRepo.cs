using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Friendships;

namespace DAL.Repository;
public class FriendshipRepo : BaseRepo<Friendship>, IFriendshipRepo
{
    public FriendshipRepo(DbContext context) : base(context)
    {
    }

    public async Task<bool> IsFriend(int currentUserId, int otherUserId)
    {
        return await _dbSet.AnyAsync(f =>
            (f.RequesterId == currentUserId && f.RequesteeId == otherUserId) ||
            (f.RequesterId == otherUserId && f.RequesteeId == currentUserId));
    }
}
