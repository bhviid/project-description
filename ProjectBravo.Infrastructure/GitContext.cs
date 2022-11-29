namespace ProjectBravo.Infrastructure;


public class GitContext : DbContext
{
    public DbSet<GitRepositoryEntity> Repos => Set<GitRepositoryEntity>();
    public DbSet<CommitEntity> Commits => Set<CommitEntity>();
    public DbSet<AuthorEntity> Authors => Set<AuthorEntity>();

    public GitContext(DbContextOptions<GitContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}