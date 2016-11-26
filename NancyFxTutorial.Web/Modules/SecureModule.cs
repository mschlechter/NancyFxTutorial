using Nancy;
using NancyFxTutorial.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class SecureModule : NancyModule
  {
    public SecureModule()
    {
      Get["/secure/hallo"] = _ => "Hallo, deze url wordt beveiligd met een JSON web token";

      this.ValidateToken();
    }
  }
}