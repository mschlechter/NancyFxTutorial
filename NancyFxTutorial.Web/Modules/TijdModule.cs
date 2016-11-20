using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Modules
{
  public class TijdModule : NancyModule
  {
    public TijdModule()
    {
      Get["/tijd"] = _ => DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    }
  }
}