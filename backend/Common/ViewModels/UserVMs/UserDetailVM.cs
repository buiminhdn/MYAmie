using Common.ViewModels.CategoryVMs;

namespace Common.ViewModels.UserVMs;
public class UserDetailVM
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public IEnumerable<CategoryVM> Categories { get; set; }
    public IEnumerable<string> Characteristics { get; set; }
    public string City { get; set; }
    public string Cover { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Images { get; set; }
}
