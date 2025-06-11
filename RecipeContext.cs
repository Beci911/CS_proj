using Microsoft.EntityFrameworkCore;
using stuff;
public class RecipeContext : DbContext
{
    // Add your DbSet properties here, for example:
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<User> Users { get; set; }


    // Constructor that accepts DbContextOptions and passes it to the base constructor
    public RecipeContext(DbContextOptions<RecipeContext> options)
        : base(options)
    {
    }

    // Optionally override OnModelCreating if needed for more advanced configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.RecipesAdded)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
        .HasMany(u => u.FavoriteRecipes)
        .WithMany(r => r.FavoritedByUsers)
        .UsingEntity<Dictionary<string, object>>(
            "UserFavoriteRecipes",
            j => j
                .HasOne<Recipe>()
                .WithMany()
                .HasForeignKey("RecipeId")
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.HasKey("UserId", "RecipeId");
            }
        );
    }

}
