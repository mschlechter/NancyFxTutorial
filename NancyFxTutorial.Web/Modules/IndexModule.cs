using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class IndexModule : NancyModule
  {
    public IndexModule()
    {
      Get["/"] = _ => View["Index"];
    }
  }
}