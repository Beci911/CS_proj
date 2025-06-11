using Microsoft.EntityFrameworkCore;

namespace stuff
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;

        public ICollection<Recipe> RecipesAdded { get; set; } = new List<Recipe>();

        public ICollection<Recipe> FavoriteRecipes { get; set; } = new List<Recipe>();

    }
}
