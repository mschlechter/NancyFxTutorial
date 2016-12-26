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
  7. [WebToken authentication inbouwen](#7-webtoken-authentication-inbouwen)

## 1. Een NancyFX project aanmaken

De eenvoudigste manier om met NancyFX te beginnen is door een leeg ASP.NET project
aan te maken en dan de volgende libraries via NuGet toe te voegen:
- Nancy
- Nancy.Hosting.Aspnet

De Nancy.Hosting.Aspnet library zorgt ervoor dat de NancyFX handler wordt geregistreerd
in ons nieuwe web project. Hierdoor zullen de NancyFX modules die we gaan maken, 
automatisch gevonden worden en direct gaan werken.

## 2. Een NancyFX module maken

Een NancyFX module kun je zien als een soort handler of controller. Code voor het
afhandelen van HTTP requests horen dus in een module.

Voor het aanmaken van onze eerste module, maak je een map met de naam Modules aan in
het project en voeg je een nieuwe class toe met de naam HalloModule.

Zorg ervoor dat deze class overerft van NancyModule en dat je in de constructor
een Get handler toevoegt die als tekst "Hallo wereld" teruggeeft:

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

Zodra je nu het project uitvoert, en met een browser naar de /hallo url gaat, zou je
een "Hallo wereld" melding moeten zien.

## 3. Een eenvoudige view maken voor HTML

Om van ons testproject een echte web applicatie te maken, is het handig om een Index.html
te delen waarmee we dingen kunnen testen. Hiervoor kun je in NancyFX een view maken.

Voordat we de view zelf aanmaken, is het handig om eerst een module te maken die de view
teruggeeft. Maak hiervoor het IndexModule.cs bestand aan:

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

Voeg daarna een map Views toe aan het project. Maak in de Views map een Index.html bestand 
aan. Als het goed is, zou je nu wanneer je het project opstart, automatisch het Index.html
bestand moeten zien in de browser.

Zoals je ziet, kan een NancyModule als API werken, maar dus ook views teruggeven. Er is dus
niet een scheiding, zoals die bestaat tussen ASP.NET MVC en ASP.NET Web API. Overigens heeft
Microsoft in ASP.NET Core dit ook opgelost en gebruiken ze nu dezelfde controllers voor 
alles.

## 4. Statische content toevoegen

Het is altijd handig om in een web applicaties over statische content te kunnen beschikken.
Te denken valt hierbij aan CSS stylesheets, plaatjes enzovoorts.

Maak hiervoor een Content map aan. NancyFX zal automatisch alle bestanden in deze map delen
via de /content/ url. In dit project heb ik bijvoorbeeld een site.css aangemaakt die 
gebruikt wordt door Index.html:

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

Omdat het site.css bestand dus in de Content map staat, en NancyFX deze map automatisch deelt,
kunnen we het bestand direct gebruiken in de view.

## 5. Het voorkomen van browser caching

Een vervelend probleem dat zich voor kan doen wanneer je met een JavaScript
framework een API aanroept, is dat het antwoord door de browser in de cache
wordt bewaard. Vooral Internet Explorer doet dat heel graag.

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
testpagina (Index.html) van dit project gaat.

## 6. Dependency injection met Autofac

NancyFX heeft ingebouwde dependency injection. Dit betekent dat als er van een willekeurige
interface maar 1 implementatie is in het project, dat deze automatisch zal worden aangemaakt
en gebruikt in de modules die deze nodig hebben.

Hiervoor wordt by default TinyIOC gebruikt.

Beter is echter het vervangen van TinyIOC door een uitgebreidere en betrouwbaardere
container. Zelf geef ik de voorkeur aan Autofac. 1 van de voordelen is bijvoorbeeld dat
je meer controle krijgt over de levensduur van de objecten (lifetime management).

Om Autofac te kunnen gebruiken heb je de Nancy.Bootstrappers.Autofac NuGet package nodig.

Het configureren van Autofac kun je met een CustomBootstrapper. Een bootstrapper voor Autofac
ziet er als volgt uit:

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

Met Autofac kunnen we eenvoudig objecten aanmaken met een InstancePerRequest scope. Dit betekent dat
gedurende de request het object maar 1 keer bestaat, en wanneer dit object IDisposable implementeert,
het ook automatisch wordt opgeruimd. Dit is erg handig voor het managen van bijvoorbeeld database
verbindingen.

Dezelfde SqlConnection kan dus automatisch worden gedeeld door meerdere Services gedurende de afhandeling
van een request, en wordt daarna automatisch weer opgeruimd. Hiervoor heb ik een SqlConnectionService.cs
gemaakt:

```C#
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
    private string ConnectionString;

    public SqlConnectionService(string connectionString)
    {
      this.ConnectionString = connectionString;
    }

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
      DbConnection = new SqlConnection(ConnectionString);
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

      // Configuratie ophalen uit web.config
      var mainConnectionString = Properties.Settings.Default.MainConnectionString;

      // Services registreren bij Autofac
      container.Update(builder =>
      {
        // SqlConnectionService registreren die gedurende een gehele request zal bestaan
        builder.Register(ctx => new SqlConnectionService(mainConnectionString))
          .As<IDbConnectionService>()
          .InstancePerRequest();
      });
    }
  }
}
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
// Registreer de LoggingService
builder.RegisterType<LoggingService>().As<ILoggingService>();
```

Ook kunnen we nu in elke willekeurige NancyModule de service injecten:

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

## 7. WebToken authentication inbouwen

Hiervoor heb je de volgende libraries nodig:

  - Microsoft.IdentityModel.Tokens (NuGet package)
  - System.IdentityModel.Tokens.Jwt (NuGet package)

**Let op:**
Sinds versie 5.0 heeft System.IdentityModel.Tokens.Jwt als dependency
Microsoft.IdentityModel.Tokens en dus niet meer de System.IdentityModel.Tokens library.

Voor het werken met JSON Web Tokens zijn 2 functies van belang:
  1. Het aanmaken van een token (CreateToken)
  2. Het controleren van een token (ValidateToken)

Deze functies brengen we onder in een IWebTokenService interface:

```C#
using System.Security.Claims;

namespace NancyFxTutorial.Web.Services
{
  public interface IWebTokenService
  {
    string CreateToken(ClaimsIdentity claimsIdentity);
    ClaimsPrincipal ValidateToken(string token);
  }
}
```

Het idee is dat we een JSON Web Token maken op basis van bepaalde claims en een secret
signing key. De secret key zorgt ervoor dat er maar 1 instantie is die de tokens kan maken
en valideren.

De claims zijn eigenschappen van onze identiteit. We zeggen in feite "wij zijn gebruiker X".

Omdat we een issuerName en een secret key nodig hebben voor onze service, maken we hier
parameters van in de constructor. Tevens heb ik deze eigenschappen als settings in het
project opgenomen, zodat ze automatisch in de web.config terecht komen.

Onze WebTokenService ziet er dan als volgt uit:

```C#
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NancyFxTutorial.Web.Services
{
  public class WebTokenService : IWebTokenService
  {
    private string Issuer;
    private byte[] Secret;

    public WebTokenService(string issuer, string secret)
    {
      this.Issuer = issuer;
      this.Secret = Convert.FromBase64String(secret);
    }

    public string CreateToken(ClaimsIdentity claimsIdentity)
    {
      var signingKey = new SymmetricSecurityKey(Secret);
      var signingCredentials = new SigningCredentials(signingKey,
          SecurityAlgorithms.HmacSha256Signature);

      var securityTokenDescriptor = new SecurityTokenDescriptor()
      {
        Issuer = this.Issuer,
        Subject = claimsIdentity,
        SigningCredentials = signingCredentials,
        Expires = DateTime.Now.AddMinutes(15)
      }; // Token is 15 minuten geldig

      var tokenHandler = new JwtSecurityTokenHandler();

      var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
      var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

      return signedAndEncodedToken;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
      var validationParameters = new TokenValidationParameters()
      {
        IssuerSigningKey = new SymmetricSecurityKey(Secret),
        ValidIssuer = this.Issuer,
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken validatedToken = null;

      var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
      return principal;
    }
  }
}
```

De registratie van de WebTokenService in de CustomBootstrapper gaat als volgt:

```C#
var secretApiKey = Properties.Settings.Default.SecretApiKey;
var issuerName = Properties.Settings.Default.IssuerName;

// Registreer de WebTokenService
builder.Register(ctx => new WebTokenService(issuerName, secretApiKey)).As<IWebTokenService>();
```



**Aan dit document wordt nog gewerkt! De code in het project werkt overigens al wel.**
