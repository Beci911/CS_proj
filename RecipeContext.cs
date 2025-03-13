using Microsoft.EntityFrameworkCore;
using stuff;
public class RecipeContext : DbContext
{
    // Add your DbSet properties here, for example:
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    // Constructor that accepts DbContextOptions and passes it to the base constructor
    public RecipeContext(DbContextOptions<RecipeContext> options)
        : base(options)
    {
    }

    // Optionally override OnModelCreating if needed for more advanced configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional model configuration can go here
    }
}
