using Common.DTOs.UserDtos;
using DAL.Repository.Core;
using Models.Accounts;

namespace DAL.Repository.IRepository;
public interface IAccountRepo : IBaseRepo<Account>
{
    Task<IEnumerable<Account>> GetUsersWithinDistanceAsync(FilterUserParams param);
}
