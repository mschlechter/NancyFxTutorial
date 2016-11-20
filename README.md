# NancyFX tutorial

In deze tutorial beschrijf ik hoe je eenvoudig een NancyFX project kunt starten.
Daarnaast wil ik ingaan op wat geavanceerdere onderwerpen.

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

