using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NancyFxTutorial.Web.Services;

namespace NancyFxTutorial.Web
{
  public class CustomBootstrapper : DefaultNancyBootstrapper
  {
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
      base.ConfigureApplicationContainer(container);

      // Omdat er maar 1 implementatie is, is deze regel niet nodig. Maar dit is dus
      // handig wanneer je meer dan 1 implementatie hebt en wilt kunnen kiezen.
      container.Register<IDbConnectionService, SqlConnectionService>();
    }
  }
}