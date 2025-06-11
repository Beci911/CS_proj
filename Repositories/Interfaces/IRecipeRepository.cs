using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using stuff;

public interface IRecipeRepository
{
    Task<Recipe> AddRecipe(Recipe recipe);
    Task<Recipe> AddIngredientToRecipe(int recipeId, int ingredientId);
    Task<bool> DeleteRecipe(int id);
    Task<Recipe> GetRecipe(int id);
    Task<Recipe> GetRecipeByName(string name);
    Task<List<Recipe>> GetAllRecipes();
    Task<Recipe> UpdateRecipe(Recipe recipe);
    Task<bool> RemoveIngredientFromRecipe(int recipeId, int ingredientId);
    Task<List<Ingredient>> GetExistingIngredients(List<string> ingredientNames);
    Task<Recipe> GetRandomRecipeAsync(List<string> requiredIngredients = null);
    // Get all recipes created by a specific user
    // Get all recipes created by a specific user
    Task<List<Recipe>> GetRecipesByUserIdAsync(int userId);

    // Get all favorite recipes for a user
    Task<List<Recipe>> GetFavoriteRecipesByUserIdAsync(int userId);

    // Add a recipe to favorites
    Task AddFavoriteAsync(int userId, int recipeId);

    // Remove a recipe from favorites
    Task RemoveFavoriteAsync(int userId, int recipeId);


}
