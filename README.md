# NancyFX tutorial

In deze tutorial beschrijf ik hoe je eenvoudig een NancyFX project kunt starten.
Daarnaast wil ik ingaan op wat geavanceerdere onderwerpen.

Inhoudsopgave:
  1. [Een NancyFX project aanmaken](#1-een-nancyfx-project-aanmaken)
  2. [Een NancyFX module maken](#2-een-nancyfx-module-maken)
  3. [Een eenvoudige view maken voor HTML](#3-een-eenvoudige-view-maken-voor-html)
  4. [Statische content toevoegen](#4-statische-content-toevoegen)
  5. [Het voorkomen van browser caching](#5-het-voorkomen-van-browser-caching)
  6. [Token authentication inbouwen](#6-token-authentication-inbouwen)

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
een Get handler toevoegt. Zie hiervoor het HalloModule.cs bestand in het
project.

Zodra je nu het project uitvoert, zul je op de /hallo url een "Hallo wereld"
melding moeten zien.

## 3. Een eenvoudige view maken voor HTML

Om van ons testproject een basis website te maken, is het handig om een Index.html
te delen waarmee we dingen kunnen testen.

Hiervoor kun je in NancyFX een view maken.

Maak hiervoor het IndexModule.cs bestand aan (zie voorbeeld). Voeg daarnaast een
map Views toe aan het project. Maak in de Views map een Index.html bestand aan.

Vanaf nu zou je automatisch het Index.html bestand moeten zien zodra je het 
project start.

## 4. Statische content toevoegen

Maak hiervoor een Content map aan. NancyFX zal automatisch alle bestanden
in deze map delen via de /content/ url. In dit project heb ik bijvoorbeeld
een site.css aangemaakt die gebruikt wordt door Index.html.

## 5. Het voorkomen van browser caching

Een vervelend probleem dat zich voor kan doen wanneer je met een JavaScript
framework een API aanroept, is dat het antwoord door de browser in de cache
wordt bewaard.

Hierdoor krijg je vaak oude antwoorden te zien.

De mooiste manier om voor een bepaalde module caching te voorkomen, is
volgens de makers van NancyFX het gebruiken van extension methods. In dit
voorbeeld project heb ik een map Extensions aangemaakt, met daarin een
bestand ModuleExtensions.cs.

Hierin heb ik een PreventCaching() functie gemaakt die een After hook van
NancyFX gebruikt om de cache control headers toe te voegen.

Het voordeel hiervan is dat ik nu in elke willekeurige NancyModule simpelweg
this.PreventCaching() kan aanroepen.

Je kunt hiervan een voorbeeld zien in het TijdModule.cs bestand. De werking
hiervan kun je zien door de tijd te verversen wanneer je naar onze testpagina
(Index.html) gaat.

## 6. Token authentication inbouwen

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

## 7. Dependency injection

NancyFX heeft ingebouwde dependency injection. Dit betekent dat als er van een willekeurige
interface maar 1 implementatie is in het project, dat deze automatisch zal worden aangemaakt
en geinjecteerd.

Als je hier meer controle over wil, kun je uiteraard ook een CustomBootstrapper maken waarin
je de TinyIoCContainer exact vertelt welke implementatie van een specifieke service
gebruikt moet worden.

Nog beter is het vervangen van TinyIOC door een uitgebreidere en stabielere container. Zelf
geef ik de voorkeur aan Autofac. Om die te kunnen gebruiken heb je de Nancy.Bootstrappers.Autofac
NuGet package nodig.

Zie de CustomBootstrapper voor een implementatie hiervan.

