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
    
}
