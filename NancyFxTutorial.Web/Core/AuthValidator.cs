using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Core
{
  public class AuthValidator
  {
    public AuthLogon GetLogon(string naam, string wachtwoord)
    {
      // Hier zouden we echt de credentials kunnen valideren
      // Voor deze tutorial ga ik er even van uit dat je met test en 1234 kunt
      // aanmelden.

      if (naam == "test" && wachtwoord == "1234") return GetTestLogon();

      return null;
    }

    public AuthLogon GetLogonById(int userId)
    {
      if (userId == 1) return GetTestLogon();

      return null;
    }

    private AuthLogon GetTestLogon()
    {
      return new AuthLogon {
        UserID = 1,
        UserName = "Test",
        Role ="Administrator",
        VolledigeNaam = "Test gebruiker"
      };
    }
  }
}