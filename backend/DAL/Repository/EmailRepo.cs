using Microsoft.EntityFrameworkCore;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Models.Emails;

namespace DAL.Repository;
public class EmailRepo : BaseRepo<Email>, IEmailRepo
{
    public EmailRepo(DbContext context) : base(context)
    {
    }
}
