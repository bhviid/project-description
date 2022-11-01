using ProjectBravo.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace ProjectBravo;
internal class GitContextFactory : IDesignTimeDbContextFactory<GitContext>
{
    public GitContext CreateDbContext(string[] args)
    {       
        // START DB with docker
        // docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
       
        // CREATE MIGRATION
        // dotnet ef migrations add InitialMigration --startup-project ProjectBravo --project ProjectBravo.Infrastructure

        // UPDATE DATABASE
        // dotnet ef database update --startup-project ProjectBravo --project ProjectBravo.Infrastructure

        string CONNECTION_STRING="Server=localhost;Database=tempdb;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False";

        var optionsBuilder = new DbContextOptionsBuilder<GitContext>();
        optionsBuilder.UseSqlServer(CONNECTION_STRING);

        return new GitContext(optionsBuilder.Options);
    }
}