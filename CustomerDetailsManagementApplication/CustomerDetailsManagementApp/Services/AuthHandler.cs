using Microsoft.AspNetCore.Authorization;

namespace CustomerDetailsManagementApp.Services;

public class UserOrAdminRequirement : IAuthorizationRequirement { }

public class UserOrAdminHandler : AuthorizationHandler<UserOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserOrAdminRequirement requirement
    )
    {
        if (context.User.IsInRole("admin") || context.User.IsInRole("user"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
