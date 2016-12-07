# NancyFX tutorial

In deze tutorial beschrijf ik hoe je eenvoudig een NancyFX project kunt starten.
Daarnaast wil ik ingaan op wat geavanceerdere onderwerpen.

Inhoudsopgave:
  1. [Een NancyFX project aanmaken](#1-een-nancyfx-project-aanmaken)
  2. [Een NancyFX module maken](#2-een-nancyfx-module-maken)
  3. [Een eenvoudige view maken voor HTML](#3-een-eenvoudige-view-maken-voor-html)
  4. [Statische content toevoegen](#4-statische-content-toevoegen)
  5. [Het voorkomen van browser caching](#5-het-voorkomen-van-browser-caching)
  6. [Dependency injection met Autofac](#6-dependency-injection-met-autofac)
  7. [Token authentication inbouwen](#7-token-authentication-inbouwen)

## 1. Een NancyFX project aanmaken

De eenvoudigste manier om met NancyFX te beginnen is door een leeg ASP.NET project
aan te maken en dan de volgende libraries via NuGet toe te voegen:
- Nancy
- Nancy.Hosting.Aspnet

Dit zorgt ervoor dat onze nieuwe NancyFX modules die we gaan maken, automatisch
gevonden worden en direct gaan werken.

## 2. Een NancyFX module maken

Maak een map met de naam Modules aan in het project en voeg een nieuwe class
toe met de naam HalloModule.

Zorg ervoor dat deze class overerft van NancyModule en dat je in de constructor
een Get handler toevoegt:

```C#
using Nancy;

namespace NancyFxTutorial.Web.Modules
{
  public class HalloModule : NancyModule
  {
    public HalloModule()
    {
      Get["/hallo"] = parameters => "Hallo wereld";
    }
  }
}
```

Zodra je nu het project uitvoert, zul je op de /hallo url een "Hallo wereld"
melding moeten zien.

## 3. Een eenvoudige view maken voor HTML

Om van ons testproject een basis website te maken, is het handig om een Index.html
te delen waarmee we dingen kunnen testen.

Hiervoor kun je in NancyFX een view maken.

Maak hiervoor het IndexModule.cs bestand aan:

```C#
using Nancy;

namespace NancyFxTutorial.Web.Modules
{
  public class IndexModule : NancyModule
  {
    public IndexModule()
    {
      Get["/"] = _ => View["Index"];
    }
  }
}
```

Voeg daarnaast een map Views toe aan het project. Maak in de Views map een Index.html bestand 
aan. Vanaf nu zou je automatisch het Index.html bestand moeten zien zodra je het 
project start.

## 4. Statische content toevoegen

Maak hiervoor een Content map aan. NancyFX zal automatisch alle bestanden
in deze map delen via de /content/ url. In dit project heb ik bijvoorbeeld
een site.css aangemaakt die gebruikt wordt door Index.html:

```HTML
<html>
<head>
  <link href="/Content/site.css" rel="stylesheet" />
</head>
<body>
  <h1>NancyFX tutorial</h1>
</body>
</html>
```

## 5. Het voorkomen van browser caching

Een vervelend probleem dat zich voor kan doen wanneer je met een JavaScript
framework een API aanroept, is dat het antwoord door de browser in de cache
wordt bewaard.

Hierdoor krijg je vaak oude antwoorden te zien.

De mooiste manier om voor een bepaalde module caching te voorkomen, is
volgens de makers van NancyFX het gebruiken van extension methods. In dit
voorbeeld project heb ik een map Extensions aangemaakt, met daarin een
bestand NancyModuleExtensions.cs:

```C#
using Nancy;

namespace NancyFxTutorial.Web.Extensions
{
  public static partial class NancyModuleExtensions
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
```

Hierin heb ik een PreventCaching() functie gemaakt die een After hook van
NancyFX gebruikt om de cache control headers toe te voegen. Dit betekent dat
aan het einde van elke verwerking de headers worden toegevoegd.

Het voordeel hiervan is dat ik nu in elke willekeurige NancyModule simpelweg
this.PreventCaching() kan aanroepen.

Je kunt hiervan een voorbeeld zien in het TijdModule.cs bestand:

```C#
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
```

De werking hiervan kun je zien door de tijd te verversen wanneer je naar de
nieuwe testpagina (Index.html) van dit project gaat.

## 6. Dependency injection met Autofac

NancyFX heeft ingebouwde dependency injection. Dit betekent dat als er van een willekeurige
interface maar 1 implementatie is in het project, dat deze automatisch zal worden aangemaakt
en gebruikt in de modules die deze nodig hebben.

Als je hier meer controle over wil, kun je uiteraard ook een CustomBootstrapper maken waarin
je de TinyIoCContainer exact vertelt welke implementatie van een specifieke service
gebruikt moet worden.

Nog beter is echter het vervangen van TinyIOC door een uitgebreidere en betrouwbaardere
container. Zelf geef ik de voorkeur aan Autofac. Om Autofac te kunnen gebruiken heb je de
Nancy.Bootstrappers.Autofac NuGet package nodig.

Onze CustomBootstrapper.cs ziet er dan als volgt uit:

```C#
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

      // Voeg hier de registraties toe
    }
  }
}
```

1 van de voordelen die Autofac ons biedt, is het aanmaken van een object met een InstancePerRequest
scope. Dit betekent dat gedurende de request het object maar 1 keer bestaat, en wanneer dit object
IDisposable implementeert, ook automatisch wordt opgeruimd. Dit is erg handig voor het managen van
bijvoorbeeld database verbindingen.

Dezelfde SqlConnection kan dus automatisch worden gedeeld door meerdere Services gedurende de afhandeling
van een request, en wordt daarna automatisch weer opgeruimd. Hiervoor heb ik een SqlConnectionService.cs
gemaakt:

```C#
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NancyFxTutorial.Web.Services
{
  // De SqlConnectionService bestaat gedurende een gehele request.
  // Hierdoor kan dezelfde SqlConnection door meerdere services gedeeld
  // worden (uiteraard niet tegelijkertijd)
  public class SqlConnectionService : IDbConnectionService 
  {
    private IDbConnection DbConnection;

    public void Dispose()
    {
      // Sluit de openstaande SQL verbinding
      DbConnection?.Dispose();
    }

    public IDbConnection GetDbConnection()
    {
      // Geeft de bestaande SQL verbinding terug
      if (DbConnection != null) return DbConnection;

      // Maak een nieuwe SQL verbinding
      var mainConnectionString = ConfigurationManager.ConnectionStrings["NancyFxTutorial.Web.Properties.Settings.MainConnectionString"].ConnectionString;

      DbConnection = new SqlConnection(mainConnectionString);
      DbConnection.Open();

      return DbConnection;
    }
  }
}
```

De interface is eenvoudig:

```C#
using System;
using System.Data;

namespace NancyFxTutorial.Web.Services
{
  public interface IDbConnectionService : IDisposable 
  {
    IDbConnection GetDbConnection();
  }
}
```

Wel moeten we deze Service netjes registreren als InstancePerRequest in de CustomBootstrapper:

```C#
// De SqlConnectionService zal gedurende een gehele request bestaan
container.Update(builder => builder
  .RegisterType<SqlConnectionService>()
  .As<IDbConnectionService>()
  .InstancePerRequest());
```

Wanneer we nu een Service maken die IDbConnectionService nodig heeft, dan kan deze gewoon
GetDbConnection() aanroepen om een SqlConnection te krijgen. Andere services die voor
dezelfde request nodig zijn, kunnen deze SqlConnection dus hergebruiken en zodra de
request is afgehandeld, wordt de verbinding weer automatisch gesloten.

Als we nu een LoggingService willen maken die een IDbConnection nodig heeft, gaat dat
eenvoudig:

ILoggingService interface:
```C#
namespace NancyFxTutorial.Web.Services
{
  public interface ILoggingService
  {
    void LogMessage(string message);
  }
}
```

LoggingService implementatie:
```C#
namespace NancyFxTutorial.Web.Services
{
  public class LoggingService : ILoggingService
  {
    IDbConnectionService DbConnectionService;

    public LoggingService(IDbConnectionService dbConnectionService)
    {
      this.DbConnectionService = dbConnectionService;
    }

    public void LogMessage(string message)
    {
      var dbConnection = DbConnectionService.GetDbConnection();

      // Voeg hier code toe die de IDbConnection nodig heeft
    }
  }
}
```

Registratie met Autofac in onze CustomBootstrapper:
```C#
// De LogginService zal worden gemaakt zodra hij nodig is
container.Update(builder => builder
  .RegisterType<LoggingService>()
  .As<ILoggingService>());
```

We kunnen nu in elke willekeurige NancyModule de service injecten:

```C#
using Nancy;
using NancyFxTutorial.Web.Services;

namespace NancyFxTutorial.Web.Modules
{
  public class TestModule : NancyModule
  {
    public AuthModule(ILoggingService loggingService)
    {
      // ILoggingService zal automatisch voor je worden aangemaakt en kun je dus hier gebruiken
    }
  }
}
```

Het grote voordeel van dependency injection is dat je een duidelijke scheiding van 
verantwoordelijkheden krijgt en dat je bovendien meer flexibiliteit wint. Als je bijvoorbeeld
graag naar een bestand wil loggen in plaats van de database, dan zou je eenvoudig een andere
implementatie van ILoggingService kunnen maken die naar een bestand schrijft en dan in de
CustomBootstrapper de registratie aanpassen.

Vanaf dat moment logt het hele project naar een bestand in plaats van de database, zonder
dat je veel hoeft aan te passen in de code.

## 7. Token authentication inbouwen

Hiervoor heb je de volgende libraries nodig:

Microsoft.IdentityModel.Tokens (NuGet package)
System.IdentityModel.Tokens.Jwt (NuGet package)

**Let op:**
Sinds versie 5.0 heeft System.IdentityModel.Tokens.Jwt als dependency
Microsoft.IdentityModel.Tokens en dus niet meer de System.IdentityModel.Tokens 
library.

In de WebTokenFunctions class heb ik de volgende functies gemaakt:

public static string CreateToken(ClaimsIdentity claimsIdentity, string issuerName, string secret)
public static ClaimsPrincipal ValidateToken(string token, string issuerName, string secret)

Het idee is dat we een JSON Web Token maken op basis van bepaalde claims en een secret
signing key. De secret key zorgt ervoor dat er maar 1 instantie is die de tokens kan maken
en valideren.

De claims zijn eigenschappen van onze identiteit. We zeggen in feite "wij zijn gebruiker X".

