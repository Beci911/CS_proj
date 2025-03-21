using Microsoft.EntityFrameworkCore;
using System;
using stuff;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("CONNECTION_STRING") ??
    Environment.GetEnvironmentVariable("CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException($"The connection string was not found in configuration or environment variables. {connectionString}");
}
Console.WriteLine($"Using connection string: {connectionString}");

builder.Services.AddDbContext<RecipeContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RecipeContext>();

    try
    {
        Console.WriteLine("Checking database connection...");
        if (!dbContext.Database.CanConnect())
        {
            throw new Exception("Database connection failed.");
        }

        Console.WriteLine("Applying pending migrations...");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"An error occurred while setting up the database: {ex.Message}");
        throw;
    }
}

app.Run();
