# NancyFX tutorial

In deze tutorial beschrijf ik hoe je eenvoudig een NancyFX project kunt starten.
Daarnaast wil ik ingaan op wat geavanceerdere onderwerpen.

Inhoudsopgave:
1. Een NancyFX project aanmaken(#een-nancyfx-project-aanmaken)

## Een NancyFX project aanmaken

De eenvoudigste manier om met NancyFX te beginnen is door een leeg ASP.NET project
aan te maken en dan de volgende libraries via NuGet toe te voegen:
- Nancy
- Nancy.Hosting.Aspnet

Dit zorgt ervoor dat onze nieuwe NancyFX modules die we gaan maken, automatisch
gevonden worden en direct gaan werken.

## Een NancyFX module maken

Maak een map met de naam Modules aan in het project en voeg een nieuwe class
toe met de naam HalloModule.

Zorg ervoor dat deze class overerft van NancyModule en dat je in de constructor
een Get handler toevoegt. Zie hiervoor het HalloModule.cs bestand in het
project.

Zodra je nu het project uitvoert, zul je op de /hallo url een "Hallo wereld"
melding moeten zien.

## Een eenvoudige view maken voor HTML

Om van ons testproject een basis website te maken, is het handig om een Index.html
te delen waarmee we dingen kunnen testen.

Hiervoor kun je in NancyFX een view maken.

Maak hiervoor het IndexModule.cs bestand aan (zie voorbeeld). Voeg daarnaast een
map Views toe aan het project. Maak in de Views map een Index.html bestand aan.

Vanaf nu zou je automatisch het Index.html bestand moeten zien zodra je het 
project start.

## Statische content toevoegen

Maak hiervoor een Content map aan. NancyFX zal automatisch alle bestanden
in deze map delen via de /content/ url. In dit project heb ik bijvoorbeeld
een site.css aangemaakt die gebruikt wordt door Index.html.

## Het voorkomen van browser caching

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
