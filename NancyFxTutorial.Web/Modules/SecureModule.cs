using Nancy;
using NancyFxTutorial.Web.Core;
using NancyFxTutorial.Web.Extensions;
using NancyFxTutorial.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class SecureModule : NancyModule
  {
    public AuthLogon CurrentLogon
    {
      get
      {
        return (AuthLogon)Context.CurrentUser;
      }
    }

    public SecureModule(IDbConnectionService dbConnectionService)
    {
      Get["/secure/hallo"] = _ =>
      {
        var x = dbConnectionService.OpenDbConnection();
        x.Dispose();

        return $"Hallo {CurrentLogon.UserName}, deze url wordt beveiligd met een JSON web token";
      };

      this.ValidateToken();
    }
  }
}