using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NancyFxTutorial.Web.Services;
using Nancy.Hosting.Aspnet;

namespace NancyFxTutorial.Web
{
  public class CustomBootstrapper : DefaultNancyBootstrapper
  {
    protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
    {
      base.ConfigureRequestContainer(container, context);

      // Trying to get SqlConnectionService in a per request scope and let it be disposed automatically
      container
        .Register<IDbConnectionService, SqlConnectionService>()
        .AsPerRequestSingleton();
    }
  }
}