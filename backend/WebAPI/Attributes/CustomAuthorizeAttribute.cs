using Microsoft.AspNetCore.Authorization;
using Models.Core;

namespace WebAPI.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public CustomAuthorizeAttribute(params Role[] roles)
    {
        if (roles != null && roles.Length > 0)
        {
            // Convert enum values to integers as a comma-separated string
            Roles = string.Join(",", roles.Select(r => ((int)r).ToString()));
        }
    }
}
