using System.Security.Claims;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? user?.FindFirst(ClaimTypes.Name)?.Value;

            return long.TryParse(userIdStr, out var id) ? id : 0;
        }
    }

    public long InstitutionId
    {
        get
        {
            var instStr = _httpContextAccessor.HttpContext?.User?.FindFirst("InstitutionId")?.Value;
            return long.TryParse(instStr, out var id) ? id : 0;
        }
    }

    public string Role
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }
}