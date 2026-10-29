# Utveckling i Windows Molntjänster - startprojekt

Det här är startgrenen (`main`) för kursens övningar. Grenen `final-result` innehåller det färdiga, fungerande resultatet för alla övningar. Fastnar du i en övning kan du jämföra med motsvarande mapp där:

https://github.com/o0Gustav-Berg0o/Moln/tree/final-result

## Förutsättningar

- .NET 10 SDK (senaste LTS)
- Visual Studio 2022 eller senare (eller `dotnet` i terminalen)
- En Azure-prenumeration för övningarna som skapar resurser

## Så här är grenen uppbyggd

De flesta övningar skapar du själv från en tom mall (`dotnet new` eller Visual Studio), enligt guiden. Därför finns det bara mappar här för övningar där guiden ger dig startkod:

| Mapp | Övning | Läge |
|---|---|---|
| `Lektion4/ECommerceEventDemo` | Service Bus läxa (topics och prenumerationer) | Startkod med `TODO`-kommentarer. Projekten bygger först när du löst dem. |
| `Lektion6/SlutProv.Api` | Slutövningen (bildtjänst med Blob Storage, SQL och Service Bus) | Startkod med `TODO 1-7`. Appen bygger först när TODO-delarna är lösta. |

Övningar utan startkod (Lektion 2-5, till exempel Key Vault, Configuration manager, Function blob trigger, Service Bus, Application Insights, Container Apps, Static Web Apps och App Service deployment) skapar du själv. Det färdiga resultatet finns i `final-result` under mapparna `Lektion2` till `Lektion6`.

## Hemligheter

Lägg aldrig lösenord, nycklar eller connection strings i filer som checkas in. Platshållare i `appsettings.json` (till exempel `KLISTRA_IN_DIN_BLOB_CONNECTION_STRING_HÄR`) ersätter du med dina egna värden lokalt via `dotnet user-secrets` eller miljövariabler. Ta bort resursgruppen i Azure när du är klar med en övning.
