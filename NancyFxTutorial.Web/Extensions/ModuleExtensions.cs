using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Extensions
{
  public static class ModuleExtensions
  {
    public static void PreventCaching(this NancyModule module)
    {
      module.After += (NancyContext ctx) =>
      {
        ctx.Response.Headers.Add("Cache-control", "no-cache");
        ctx.Response.Headers.Add("Pragma", "no-cache");
      };
    }
  }
}