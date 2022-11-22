string connString = "Server=localhost;Database=tempdb;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GitContext>(options => options.UseSqlServer(connString));
builder.Services.AddScoped<IGitRepoRepository, GitRepoRepository>();
builder.Services.AddScoped<ICommitRepository, CommitsRepository>();
builder.Services.AddScoped<IGitAnalyzer, GitInsights>();
builder.Services.AddScoped<IGitHelper, GitHelperInitializer>();

var corsSpec = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsSpec, policy => policy.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(corsSpec);

app.MapControllers();


app.Run();
