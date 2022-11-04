namespace ProjectBravo.Infrastructure;


public class GitContext : DbContext
{
    public DbSet<GitRepository> Repos => Set<GitRepository>();
    public DbSet<Commit> Commits => Set<Commit>();
    public DbSet<Author> Authors => Set<Author>();

    public GitContext(DbContextOptions<GitContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}