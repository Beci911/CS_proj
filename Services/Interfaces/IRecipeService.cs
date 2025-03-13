using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using stuff;

public interface IRecipeService
{
    Task<RecipeDto> AddRecipe(RecipeDto recipeDto);
    Task<RecipeDto> AddIngredientToRecipe(int recipeId, int ingredientId);
    Task<bool> DeleteRecipe(int id);
    Task<RecipeDto> GetRecipe(int id);
    Task<RecipeDto> GetRecipeByName(string name);
    Task<List<RecipeDto>> GetAllRecipes();
    Task<RecipeDto> UpdateRecipe(RecipeDto recipeDto);
    Task<bool> RemoveIngredientFromRecipe(int recipeId, int ingredientId);
    Task<RecipeDto> GetRandomRecipeAsync(List<string> requiredIngredients = null);
}