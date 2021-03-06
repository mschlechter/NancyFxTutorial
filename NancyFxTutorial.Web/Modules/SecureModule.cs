﻿using Nancy;
using NancyFxTutorial.Web.Core;
using NancyFxTutorial.Web.Extensions;
using NancyFxTutorial.Web.Models;
using NancyFxTutorial.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class SecureModule : NancyModule
  {
    private IWebTokenService WebTokenService;

    public AuthenticationLogon CurrentLogon
    {
      get
      {
        return (AuthenticationLogon)Context.CurrentUser;
      }
    }

    public SecureModule(IWebTokenService webTokenService)
    {
      this.WebTokenService = webTokenService;
      this.Before += BeforeResponse;
    }

    private Response BeforeResponse(NancyContext ctx)
    {
      string bearerToken = "";

      // Haal token uit de request headers
      var authHeader = ctx.Request.Headers.Authorization;
      if (authHeader?.Length > 7) bearerToken = authHeader.Substring(7);

      if (!string.IsNullOrEmpty(bearerToken))
      {
        var principal = WebTokenService.ValidateToken(bearerToken);

        var logon = AuthenticationLogon.CreateFromClaimsPrincipal(principal);
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