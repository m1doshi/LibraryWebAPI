using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI.API.Extensions
{
    public class RoleHierarchyRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; }

        public RoleHierarchyRequirement(params string[] roles)
        {
            Roles = roles;
        }
    }
    public class RoleHierarchyHandler : AuthorizationHandler<RoleHierarchyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleHierarchyRequirement requirement)
        {
            var userRoleClaim = context.User.FindFirst("roleId");
            if (userRoleClaim != null)
            {
                var userRole = userRoleClaim.Value;
                if (requirement.Roles.Contains(userRole))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
    
}
