using Common.DTOs.UserDtos;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Accounts;
using Models.Core;

namespace DAL.Repository;
public class AccountRepo : BaseRepo<Account>, IAccountRepo
{
    private const double EarthRadiusKm = 6371;

    public AccountRepo(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Account>> GetUsersWithinDistanceAsync(FilterUserParams param)
    {
        if (param.Latitude == 0 || param.Longitude == 0)
            throw new ArgumentException("Latitude and Longitude must be provided.");

        if (param.DistanceInKm <= 0 || param.DistanceInKm > 15)
            throw new ArgumentException("Distance must be between 0 and 15 kilometers.");

        int distanceKm = param.DistanceInKm ?? 5;
        if (distanceKm <= 0)
            throw new ArgumentException("Distance must be greater than 0.");

        var query = _dbSet.AsQueryable()
         .Include(user => user.City)
         .Include(user => user.Categories)
         .Where(user =>
             user.Id != param.CurrentUserId &&
             user.Role == Role.USER &&
             user.Status == AccountStatus.ACTIVATED &&
             user.IsEmailVerified &&
             user.Latitude != 0 &&
             user.Longitude != 0);

        if (param.CategoryId > 0)
        {
            query = query.Where(user => user.Categories.Any(h => h.Id == param.CategoryId));
        }

        // Execute the query and then filter in memory
        var users = await query.ToListAsync();

        // Apply distance filter in memory
        return users.Where(user =>
            CalculateHaversineDistance(
                (double)user.Latitude,
                (double)user.Longitude,
                (double)param.Latitude!.Value,
                (double)param.Longitude!.Value) <= distanceKm);
    }

    private static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double dLat = (lat2 - lat1) * Math.PI / 180;
        double dLon = (lon2 - lon1) * Math.PI / 180;

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                   Math.Pow(Math.Sin(dLon / 2), 2);

        return EarthRadiusKm * 2 * Math.Asin(Math.Sqrt(a));
    }
}
