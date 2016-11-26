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

      if (naam == "test" && wachtwoord == "1234")
        return new AuthLogon { ID = 1, Naam = "Test", VolledigeNaam = "Test gebruiker" };

      return null;
    }
  }
}