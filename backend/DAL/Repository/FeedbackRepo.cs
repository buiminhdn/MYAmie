using Microsoft.EntityFrameworkCore;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Models.Feedbacks;

namespace DAL.Repository;
public class FeedbackRepo : BaseRepo<Feedback>, IFeedbackRepo
{
    public FeedbackRepo(DbContext context) : base(context)
    {

    }
}
