using Microsoft.EntityFrameworkCore;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Models.Places;

namespace DAL.Repository;
public class PlaceRepo : BaseRepo<Place>, IPlaceRepo
{
    public PlaceRepo(DbContext context) : base(context)
    {
    }
}
