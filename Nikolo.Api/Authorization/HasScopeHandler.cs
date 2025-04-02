using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nikolo.Api.Authorization;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    private readonly ILogger<HasScopeHandler> _logger;

    public HasScopeHandler(ILogger<HasScopeHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        _logger.LogInformation("Checking authorization for scope: {Scope}", requirement.Scope);

        // Log all claims
        foreach (var claim in context.User.Claims)
        {
            _logger.LogInformation("Claim Type: {Type}, Value: {Value}, Issuer: {Issuer}", claim.Type, claim.Value, claim.Issuer);
        }

        // Check if the scope claim is present
        var scopeClaim = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer);
        if (scopeClaim == null)
        {
            _logger.LogWarning("No 'scope' claim found in the token.");
            return Task.CompletedTask;
        }

        _logger.LogInformation("Scope claim found: {ScopeClaim}", scopeClaim.Value);

        // Check if the required scope is in the token
        var scopes = scopeClaim.Value.Split(' ');
        if (scopes.Contains(requirement.Scope))
        {
            _logger.LogInformation("Required scope '{Scope}' is present. Authorization granted.", requirement.Scope);
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning("Required scope '{Scope}' is missing. Authorization denied.", requirement.Scope);
        }

        return Task.CompletedTask;
    }
}