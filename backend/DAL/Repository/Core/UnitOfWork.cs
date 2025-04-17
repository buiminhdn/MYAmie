using DAL.Repository.IRepository;

namespace DAL.Repository.Core;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IAccountRepo Accounts { get; private set; }

    public IBusinessRepo Businesses { get; private set; }

    public ICategoryRepo Categories { get; private set; }

    public IFriendshipRepo Friendships { get; private set; }

    public IPlaceRepo Places { get; private set; }

    public IFeedbackRepo Feedbacks { get; private set; }

    public ICityRepo Cities { get; private set; }

    public IEmailRepo Emails { get; private set; }

    public IMessageRepo Messages { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Accounts = new AccountRepo(context);
        Businesses = new BusinessRepo(context);
        Categories = new CategoryRepo(context);
        Friendships = new FriendshipRepo(context);
        Places = new PlaceRepo(context);
        Feedbacks = new FeedbackRepo(context);
        Cities = new CityRepo(context);
        Emails = new EmailRepo(context);
        Messages = new MessageRepo(context);
    }

    public int Save()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
