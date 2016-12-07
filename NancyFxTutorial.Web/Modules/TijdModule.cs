using Nancy;
using NancyFxTutorial.Web.Extensions;
using System;

namespace NancyFxTutorial.Web.Modules
{
  public class TijdModule : NancyModule
  {
    public TijdModule()
    {
      Get["/tijd"] = _ => DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

      this.PreventCaching();
    }
  }
}