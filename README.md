#DnDBeyond Back End Challenge
This is my first ever project using C#, let alone .NET. Please excuse me if I make any convention faux pas as I learn the language and the framework.

This application was developed using hexagonal architecture and TDD.

##Usage
On startup, the application will read in the first character from the briv.json file in the root directory and add it to the in-memory database.

I have also left Swagger enabled on the project, so a basic description of available routes should appear on startup.

To manually add, update, or remove characters to the in-memory database, use the functions defined in the DomainCharacterController. A GET to `/api/characters` will return all characters currently in the database in the app domain model format as JSON, POST with a character in that format will add it, so on and so forth. A convenience route `POST /api/characters/addCharacter` has been added to add a character to the DB in the format that was defined in `briv.json` (it will be converted to the domain format before being stored).

The HealthTrackingController defines the routes for the core functionality, i.e. damaging, healing, and adding temporary hitpoints to characters. The "briv" character will start with ID 1 by default on startup. 

##Testing
The application was developed using TDD utilizing xunit and Moq. Tests are located in the BackEndChallengeTests directory and the `BackEndChallengeTests.csproj` project file. 

##Assumptions
Out of Scope Features: 
* Instant character death according to 5e rules
* Items that set character stats (Amulet of Health)
* Items that modify character defenses
* Proper input validation and scrubbing