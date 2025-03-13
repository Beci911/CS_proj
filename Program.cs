using Microsoft.EntityFrameworkCore;
using stuff;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string was not found in configuration or environment variables.");
}

builder.Services.AddDbContext<RecipeContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RecipeContext>();

    try
    {
     
        if (!dbContext.Database.CanConnect())
        {
            Console.WriteLine("Database does not exist or is not accessible.");
            throw new Exception("Database connection failed.");
        }

        Console.WriteLine("Applying migrations...");

        
        var migrationsPending = dbContext.Database.GetPendingMigrations().Any();
        
        if (migrationsPending)
        {
            Console.WriteLine("There are pending migrations. Applying migrations...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }

        if (!migrationsPending)
        {
            CreateMigration("AutoMigration");
        }
        if (migrationsPending)
        {
            Console.WriteLine("There are pending migrations. Applying migrations...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
    }
    catch (Exception ex)
    {
 
        Console.Error.WriteLine($"An error occurred while setting up the database: {ex.Message}");
        throw; 
    }
}

app.Run();


void CreateMigration(string migrationName)
{
    try
    {
        Console.WriteLine($"Creating migration: {migrationName}");

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"ef migrations add {migrationName} --startup-project ./ --project ./",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        var process = Process.Start(processStartInfo);
        process?.WaitForExit();

        if (process?.ExitCode == 0)
        {
            Console.WriteLine($"Migration {migrationName} created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create migration {migrationName}.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while creating migration: {ex.Message}");
    }
}
