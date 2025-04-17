using Microsoft.EntityFrameworkCore;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Models.Cities;

namespace DAL.Repository;
public class CityRepo : BaseRepo<City>, ICityRepo
{
    public CityRepo(DbContext context) : base(context)
    {
    }
}
