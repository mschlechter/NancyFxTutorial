using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NancyFxTutorial.Web.Services;

namespace NancyFxTutorial.Web.Modules
{
  public class HalloSecureModule : SecureModule
  {
    public HalloSecureModule(IWebTokenService webTokenService) : base(webTokenService)
    {
      Get["/secure/hallo"] = _ =>
      {
        return $"Hallo {CurrentLogon.UserName}, deze url wordt beveiligd met een JSON web token";
      };
    }
  }
}