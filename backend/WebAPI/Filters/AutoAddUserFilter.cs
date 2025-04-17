using Microsoft.AspNetCore.Mvc.Filters;
using Models.Core;
using System.Security.Claims;
using System.Security.Principal;

namespace WebAPI.Filters;

public class AutoAddUserFilter(params Type[] supportedModelTypes) : IActionFilter
{
    private readonly Type[] _supportedModelTypes = supportedModelTypes;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after execution in this case
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
            return;

        int userId = GetUserIdFromContext(context.HttpContext.User.Identity);
        Role userRole = GetUserRoleFromContext(context.HttpContext.User);

        foreach (var argument in context.ActionArguments.Values.Where(arg => IsSupportedModelType(arg.GetType())))
        {
            SetCurrentUserId(argument, userId);
            SetCurrentUserRole(argument, userRole);
        }
    }

    #region Utility Methods

    // Helper method to get the user ID from the authenticated user's identity
    private static int GetUserIdFromContext(IIdentity identity)
    {
        if (!int.TryParse(identity.Name, out int userId))
        {
            userId = 0;
        }
        return userId;
    }

    // Helper method to get the user role from the authenticated user's claims
    private static Role GetUserRoleFromContext(ClaimsPrincipal user)
    {
        if (!Enum.TryParse(user.FindFirstValue(ClaimTypes.Role), out Role role))
        {
            role = Role.USER;
        }
        return role;
    }

    // Helper method to check if the argument type is supported
    private bool IsSupportedModelType(Type argumentType)
    {
        return _supportedModelTypes.Any(rootType => rootType.IsAssignableFrom(argumentType));
    }

    // Helper method to set the CurrentUserId property of the argument
    private static void SetCurrentUserId(object argument, int userId)
    {
        var userIdProperty = argument.GetType().GetProperty("CurrentUserId");
        if (userIdProperty != null && userIdProperty.PropertyType == typeof(int))
        {
            userIdProperty.SetValue(argument, userId);
        }
    }

    // Helper method to set the CurrentUserRole property of the argument
    private static void SetCurrentUserRole(object argument, Role userRole)
    {
        var userRoleProp = argument.GetType().GetProperty("CurrentUserRole");
        if (userRoleProp != null && userRoleProp.PropertyType == typeof(Role))
        {
            userRoleProp.SetValue(argument, userRole);
        }
    }

    #endregion
}
