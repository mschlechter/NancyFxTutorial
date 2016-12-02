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
    public AuthModule()
    {
      Post["/auth/token"] = _ =>
      {
        var authRequest = this.Bind<AuthRequest>();

        var logon = new AuthValidator().GetLogon(authRequest.Naam, authRequest.Wachtwoord);
        if (logon == null)
        {
          return new Response {
            StatusCode = HttpStatusCode.BadRequest,
            ReasonPhrase = "Onbekende combinatie van gebruikersnaam en wachtwoord"
          };
        }

        var identity = logon.ToClaimsIdentity();
        var token = WebTokenFunctions.CreateToken(identity, AppUtils.Issuer, AppUtils.SecretApiKey);

        return token;
      };
    }
  }
}