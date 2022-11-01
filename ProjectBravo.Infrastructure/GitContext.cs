namespace ProjectBravo.Infrastructure;

public class GitContext : DbContext
{
    public DbSet<GitRepository> repos => Set<GitRepository>();

    public GitContext(DbContextOptions<GitContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}