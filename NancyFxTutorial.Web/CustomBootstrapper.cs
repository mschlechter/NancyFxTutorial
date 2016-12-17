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

      // Configuratie ophalen uit web.config
      var mainConnectionString = Properties.Settings.Default.MainConnectionString;

      var secretApiKey = Properties.Settings.Default.SecretApiKey;
      var issuerName = Properties.Settings.Default.IssuerName;

      // SqlConnectionService registreren die gedurende een gehele request zal bestaan
      container.Update(builder => builder
        .Register(ctx => {
          return new SqlConnectionService(mainConnectionString);
        })
        .As<IDbConnectionService>()
        .InstancePerRequest());

      // Registreer de WebTokenService
      container.Update(builder => builder
        .Register(ctx => {
          return new WebTokenService(issuerName, secretApiKey);
        })
        .As<IWebTokenService>());

      // Registreer de AuthenticationService
      container.Update(builder => builder
        .RegisterType<AuthenticationService>()
        .As<IAuthenticationService>());

      // Registreer de LoggingService
      container.Update(builder => builder
        .RegisterType<LoggingService>()
        .As<ILoggingService>());
    }
  }
}