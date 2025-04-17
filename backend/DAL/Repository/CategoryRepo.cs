using Microsoft.EntityFrameworkCore;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Models.Categories;

namespace DAL.Repository;
public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
{
    public CategoryRepo(DbContext context) : base(context)
    {
    }
}
