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

      // Services registreren bij Autofac
      container.Update(builder =>
      {
        // SqlConnectionService registreren die gedurende een gehele request zal bestaan
        builder.Register(ctx => new SqlConnectionService(mainConnectionString))
          .As<IDbConnectionService>()
          .InstancePerRequest();

        // Registreer de WebTokenService
        builder.Register(ctx => new WebTokenService(issuerName, secretApiKey)).As<IWebTokenService>();

        // Registreer de AuthenticationService
        builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();

        // Registreer de LoggingService
        builder.RegisterType<LoggingService>().As<ILoggingService>();
      });
    }
  }
}