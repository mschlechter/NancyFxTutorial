using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class HalloModule : NancyModule
  {
    public HalloModule()
    {
      Get["/hallo"] = parameters => "Hallo wereld";
    }
  }
}