using System.Security.Claims;

namespace NancyFxTutorial.Web.Services
{
  public interface IWebTokenService
  {
    string CreateToken(ClaimsIdentity claimsIdentity);
    ClaimsPrincipal ValidateToken(string token);
  }
}