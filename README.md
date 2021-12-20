Use the dotnet secrets manager to apply "MysqlDbSettings:Password" to appsettings.json in both MVC and IDS projects.
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows

For Identity Server, use username: "alice" and password "Pass123$" for login progress. The complete dummy data can be imported with '/seed' arguments in project build. That will seed the data to the database from Config.cs in IDS project.