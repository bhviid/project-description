using ProjectBravo.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace ProjectBravo;
internal class GitContextFactory : IDesignTimeDbContextFactory<GitContext>
{
    public GitContext CreateGitContext(string[] args)
    {       
        // START DB with docker
        // docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

        string CONNECTION_STRING="Server=localhost;Database=tempdb;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False";

        var optionsBuilder = new DbContextOptionsBuilder<GitContext>();
        optionsBuilder.UseSqlServer(CONNECTION_STRING)

        return new GitContext(optionsBuilder.Options);
    }
}