using Common.ViewModels.CategoryVMs;

namespace Common.ViewModels.BusinessVMs;
public class BusinessDetailVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Cover { get; set; }
    public string Avatar { get; set; }
    public string ShortDescription { get; set; }
    public IEnumerable<CategoryVM> Categories { get; set; }
    public string City { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string OperatingHours { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Images { get; set; }


}
