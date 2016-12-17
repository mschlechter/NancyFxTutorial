using Autofac;
using Nancy;
using Nancy.Bootstrappers.Autofac;
using NancyFxTutorial.Web.Core;
using NancyFxTutorial.Web.Services;
using System.Configuration;

namespace NancyFxTutorial.Web
{
  public class CustomBootstrapper : AutofacNancyBootstrapper
  {
    protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
    {
      base.ConfigureRequestContainer(container, context);

      var mainConnectionString = ConfigurationManager.ConnectionStrings["NancyFxTutorial.Web.Properties.Settings.MainConnectionString"].ConnectionString;

      // De SqlConnectionService zal gedurende een gehele request bestaan
      container.Update(builder => builder
        .Register(ctx => {
          return new SqlConnectionService(mainConnectionString);
        })
        .As<IDbConnectionService>()
        .InstancePerRequest());

      // Registreer de WebTokenService
      container.Update(builder => builder
        .Register(ctx => {
          return new WebTokenService(AppUtils.Issuer, AppUtils.SecretApiKey);
        })
        .As<IWebTokenService>());

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