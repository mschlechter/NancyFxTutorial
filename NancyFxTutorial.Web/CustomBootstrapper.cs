using Autofac;
using Nancy;
using Nancy.Bootstrappers.Autofac;
using NancyFxTutorial.Web.Services;

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

      // De LogginService zal worden gemaakt zodra hij nodig is
      container.Update(builder => builder
        .RegisterType<LoggingService>()
        .As<ILoggingService>());
    }
  }
}