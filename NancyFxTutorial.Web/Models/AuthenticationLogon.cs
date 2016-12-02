using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NancyFxTutorial.Web.Models
{
  public class AuthenticationLogon : IUserIdentity
  {
    public IEnumerable<string> Claims
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int UserID { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }

    public string VolledigeNaam { get; set; }

    public ClaimsIdentity ToClaimsIdentity()
    {
      var claimsIdentity = new ClaimsIdentity(new List<Claim>()
      {
          new Claim(ClaimTypes.NameIdentifier, this.UserID.ToString()),
          new Claim(ClaimTypes.Name, this.UserName),
          new Claim(ClaimTypes.Role, this.Role)
      }, "Custom");

      return claimsIdentity;
    }

    public static AuthenticationLogon CreateFromClaimsPrincipal(ClaimsPrincipal principal)
    {
      if (principal == null) return null;

      var userId = principal.Claims
        .Where(c => c.Type == ClaimTypes.NameIdentifier)
        .Select(c => c.Value).SingleOrDefault();

      int userIdValue = -1;

      if (!Int32.TryParse(userId, out userIdValue))
      {
        return null;
      }

      var userName = principal.Claims
        .Where(c => c.Type == ClaimTypes.Name)
        .Select(c => c.Value).SingleOrDefault();

      var role = principal.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value).SingleOrDefault();

      // Maak hier een user object aan op basis van de claims en ken het toe
      var logon = new AuthenticationLogon
      {
        UserID = userIdValue,
        UserName = userName,
        Role = role
      };

      return logon;
    }
  }
}