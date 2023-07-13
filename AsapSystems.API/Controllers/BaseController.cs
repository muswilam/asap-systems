using System.Security.Claims;
using AsapSystems.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AsapSystems.API.Controllers;

[Controller]
public abstract class BaseController : ControllerBase
{
    protected BaseController() { }

    public ClaimsPrincipal CurrentUser
    {
        get
        {
            if (HttpContext.User is not null && HttpContext.User.Identity.IsAuthenticated)
                return HttpContext.User;
            else
                return null;
        }
    }

    public int CurrentUserId { get => CurrentUser is not null ? Convert.ToInt32(CurrentUser.FindFirstValue(TokenClaimTypeEnum.Id.ToString())) : default; }

    public int CurrentUserName { get => CurrentUser is not null ? Convert.ToInt32(CurrentUser.FindFirstValue(TokenClaimTypeEnum.Name.ToString())) : default; }
}
