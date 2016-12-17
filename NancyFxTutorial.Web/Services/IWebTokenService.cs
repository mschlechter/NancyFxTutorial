using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NancyFxTutorial.Web.Services
{
  public interface IWebTokenService
  {
    string CreateToken(ClaimsIdentity claimsIdentity);
    ClaimsPrincipal ValidateToken(string token);
  }
}