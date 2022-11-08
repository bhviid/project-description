// time for user secrets yep,

using ProjectBravo;

string connString =
    "Server=localhost;Database=tempdb;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GitContext>(options => options.UseSqlServer(connString));
builder.Services.AddScoped<IGitRepoRepository, GitRepoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/frequency/{user}/{repo_name}", 
    async (string? user, string? repo_name, IGitRepoRepository dbRepo) =>
{
    var foundInDb = await dbRepo.FindAsync(repo_name);
    string toReturn;

    if (foundInDb is null)
    {
        
        var cloned = GitInsights.CloneGithubRepo(user, repo_name);
        var authors = cloned.Commits.Select(c => c.Author.Name).Distinct().ToList();
        var commits = cloned.Commits.Select(c =>
            new CommitCreateDTO(c.Author.When.DateTime, c.Message, c.Author.Name, repo_name))
            .ToList();
        
        var s = await dbRepo.CreateAsync(new GitRepositryCreateDTO(repo_name, 
            authors, commits));

        toReturn = GitInsights.GetFrequencyString(GitInsights.GenerateCommitsByDate(cloned));
        
        //clone repo locally
        //add to database...
    }
    else
    {
        //check if the repo has been updated compared to our database, if yes update the database.
        //if(foundInDb.LatestCommit > repo_name)

        toReturn = "brr";
        //return new
        //{
        //    user,
        //    repo_name
        //};
    }

    return new { toReturn };
});

app.Run();