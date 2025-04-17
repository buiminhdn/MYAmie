using Microsoft.EntityFrameworkCore;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Models.Businesses;

namespace DAL.Repository;
public class BusinessRepo : BaseRepo<Business>, IBusinessRepo
{
    public BusinessRepo(DbContext context) : base(context)
    {
    }
}
