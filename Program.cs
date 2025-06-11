using Microsoft.EntityFrameworkCore;
using System;
using stuff;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
    });
});
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


Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("❌ JWT Error: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("✅ JWT validated.");
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowReactFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


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
    var adminUser = dbContext.Users.FirstOrDefault(u => u.Username == "admin");

    if (adminUser == null)
    {
        var passwordHasher = new PasswordHasher<User>();

        adminUser = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            Role = "ADMIN",
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "admintesztjelszo");

        dbContext.Users.Add(adminUser);
    }
    else if (adminUser.Role != "ADMIN")
    {
        adminUser.Role = "ADMIN";
        
    }

    dbContext.SaveChanges();
}

app.Run();
