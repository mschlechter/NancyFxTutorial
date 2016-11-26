using Nancy;
using Nancy.ModelBinding;
using NancyFxTutorial.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class AuthModule : NancyModule
  {
    // Deze kun je genereren met de WebTokenFunctions.CreateSecret functie
    private string SECRET = "+a8tTwAuErCTfvPYtGeaS/dMJmxx9IlbmlOt8QnHAqY=";

    public AuthModule()
    {
      Post["/auth/token"] = _ =>
      {
        var authRequest = this.Bind<AuthRequest>();

        var logon = new AuthValidator().GetLogon(authRequest.Naam, authRequest.Wachtwoord);
        if (logon == null)
        {
          return "";
        }

        var identity = WebTokenFunctions.CreateClaimIdentity(logon.ID.ToString(), logon.Naam, "Gebruiker");
        var token = WebTokenFunctions.CreateToken(identity, "NancyFxTutorial", SECRET);

        return token;
      };
    }
  }
}