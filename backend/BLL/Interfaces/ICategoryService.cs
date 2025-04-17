using Common.Responses;
using Common.ViewModels.CategoryVMs;

namespace BLL.Interfaces;
public interface ICategoryService
{
    Task<ApiResponse<IEnumerable<CategoryVM>>> GetCategories();
}
