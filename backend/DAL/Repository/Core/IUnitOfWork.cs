using DAL.Repository.IRepository;

namespace DAL.Repository.Core;
public interface IUnitOfWork
{
    IAccountRepo Accounts { get; }
    IBusinessRepo Businesses { get; }
    ICategoryRepo Categories { get; }
    IFriendshipRepo Friendships { get; }
    IPlaceRepo Places { get; }
    IFeedbackRepo Feedbacks { get; }
    ICityRepo Cities { get; }
    IEmailRepo Emails { get; }
    IMessageRepo Messages { get; }

    int Save();
    Task<int> SaveAsync();
}
