using Nancy;
using NancyFxTutorial.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NancyFxTutorial.Web.Extensions
{
  public static partial class NancyModuleExtensions
  {
    public static void PreventCaching(this NancyModule module)
    {
      module.After += (NancyContext ctx) =>
      {
        ctx.Response.Headers.Add("Cache-control", "no-cache");
        ctx.Response.Headers.Add("Pragma", "no-cache");
      };
    }

    public static void ValidateToken(this NancyModule module)
    {
      // Lees dit eens goed...
      // http://bytefish.de/blog/token_authentication_owin_nancy/#implementing-token-authentication-with-nancy-and-owin


      //module.Before += (NancyContext ctx) =>
      //{

      //};

      module.Before += BeforeResponse;
    }

    static Response BeforeResponse(NancyContext ctx)
    {
      string bearerToken = "";

      // Haal token uit de request headers
      var authHeader = ctx.Request.Headers.Authorization;
      if (authHeader?.Length > 7) bearerToken = authHeader.Substring(7);

      if (!string.IsNullOrEmpty(bearerToken))
      {
        var principal = WebTokenFunctions.ValidateToken(bearerToken, AppUtils.Issuer, AppUtils.SecretApiKey);

        var logon = AuthLogon.CreateFromClaimsPrincipal(principal);
        if (logon != null)
        {
          ctx.CurrentUser = logon;

          // Laat de response door
          return null;
        }
      }

      return new Response
      {
        StatusCode = HttpStatusCode.Unauthorized
      };
    }
  }
}