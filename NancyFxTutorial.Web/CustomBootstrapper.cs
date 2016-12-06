using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NancyFxTutorial.Web.Services;
using Nancy.Hosting.Aspnet;
using Nancy.Bootstrappers.Autofac;
using Autofac;

namespace NancyFxTutorial.Web
{
  public class CustomBootstrapper : AutofacNancyBootstrapper
  {
    protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
    {
      base.ConfigureRequestContainer(container, context);

      // De SqlConnectionService zal gedurende een gehele request bestaan
      container.Update(builder => builder
        .RegisterType<SqlConnectionService>()
        .As<IDbConnectionService>()
        .InstancePerRequest());

      // De AuthenticationService zal worden gemaakt zodra hij nodig is
      container.Update(builder => builder
        .RegisterType<AuthenticationService>()
        .As<IAuthenticationService>());

    }
  }
}