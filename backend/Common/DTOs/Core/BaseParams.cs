using Models.Core;
using System.Text.Json.Serialization;

namespace Common.DTOs.Core;
public class BaseParams
{
    [JsonIgnore]
    public int CurrentUserId { get; set; }
    [JsonIgnore]
    public Role CurrentUserRole { get; set; }
}
