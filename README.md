### DB Migrations

dotnet ef migrations add FirstMigration -c CommonDBContext -p .\src\Common.Architecture.Persistance -s .\src\Common.Architecture.WebAPI -o Migrations
dotnet ef migrations remove -p .\src\Common.Architecture.Persistance -s .\src\Common.Architecture.WebAPI
dotnet ef database update -p .\src\Common.Architecture.Persistance -s .\src\Common.Architecture.WebAPI
dotnet ef database drop -p .\src\Common.Architecture.Persistance -s .\src\Common.Architecture.WebAPI

// revert applied migration
dotnet ef database update AddedXyzEntity dotnet ef migrations remove -p .\src\Common.Architecture.Persistance -s .\src\Common.Architecture.WebAPI

// Deployment
1. MVC Startup.cs commented // services.AddJsReport(...
2. MVC copy settings.Development.json to wwwroot
3. MVC copy C:\inetpub\AktifCloudMvc\wwwroot\SmartAdmin içine dist folder

