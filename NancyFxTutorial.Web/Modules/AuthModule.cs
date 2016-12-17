using Nancy;
using Nancy.ModelBinding;
using NancyFxTutorial.Web.Core;
using NancyFxTutorial.Web.Models;
using NancyFxTutorial.Web.Services;

namespace NancyFxTutorial.Web.Modules
{
  public class AuthModule : NancyModule
  {
    public AuthModule(
      IAuthenticationService authenticationService, 
      IWebTokenService webTokenService,
      ILoggingService loggingService)
    {
      Post["/auth/token"] = _ =>
      {
        var authRequest = this.Bind<AuthenticationRequest>();

        var logon = authenticationService.GetLogonByCredentials(authRequest.Naam, authRequest.Wachtwoord);
        if (logon == null)
        {
          loggingService.LogMessage("Inloggen mislukt");

          return new Response
          {
            StatusCode = HttpStatusCode.BadRequest,
            ReasonPhrase = "Onbekende combinatie van gebruikersnaam en wachtwoord"
          };
        }

        loggingService.LogMessage("Inloggen gelukt");

        var identity = logon.ToClaimsIdentity();
        var token = webTokenService.CreateToken(identity);

        return token;
      };
    }
  }
}