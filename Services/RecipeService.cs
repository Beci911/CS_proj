using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stuff;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngredientRepository _ingredientRepository;

    public RecipeService(IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository)
    {
        _recipeRepository = recipeRepository;
        _ingredientRepository = ingredientRepository;
    }

    public async Task<RecipeDto> AddRecipe(RecipeDto recipeDto)
    {
        if (recipeDto == null)
        {
            throw new ArgumentNullException(nameof(recipeDto));
        }

        var recipe = Mapper.MapToRecipe(recipeDto);
        var addedRecipe = await _recipeRepository.AddRecipe(recipe);
        return Mapper.RecipeToDto(addedRecipe);
    }

    public async Task<RecipeDto> AddIngredientToRecipe(int recipeId, int ingredientId)
    {
        var recipe = await _recipeRepository.AddIngredientToRecipe(recipeId, ingredientId);
        return Mapper.RecipeToDto(recipe);
    }

    public async Task<bool> DeleteRecipe(int id)
    {
        return await _recipeRepository.DeleteRecipe(id);
    }

    public async Task<RecipeDto> GetRecipe(int id)
    {
        var recipe = await _recipeRepository.GetRecipe(id);
        return recipe == null ? null : Mapper.RecipeToDto(recipe);
    }

    public async Task<RecipeDto> GetRecipeByName(string name)
    {
        var recipe = await _recipeRepository.GetRecipeByName(name);
        return recipe == null ? null : Mapper.RecipeToDto(recipe);
    }

    public async Task<List<RecipeDto>> GetAllRecipes()
    {
        var recipes = await _recipeRepository.GetAllRecipes();
        return recipes?.Select(Mapper.RecipeToDto).ToList() ?? new List<RecipeDto>();
    }

    public async Task<RecipeDto> UpdateRecipe(RecipeDto recipeDto)
    {
        if (recipeDto == null)
        {
            throw new ArgumentNullException(nameof(recipeDto));
        }

        var recipe = Mapper.MapToRecipe(recipeDto);
        var updatedRecipe = await _recipeRepository.UpdateRecipe(recipe);
        return Mapper.RecipeToDto(updatedRecipe);
    }

    public async Task<bool> RemoveIngredientFromRecipe(int recipeId, int ingredientId)
    {
        return await _recipeRepository.RemoveIngredientFromRecipe(recipeId, ingredientId);
    }

    public async Task<RecipeDto> GetRandomRecipeAsync(List<string> requiredIngredients = null)
    {
    var randomRecipe = await _recipeRepository.GetRandomRecipeAsync(requiredIngredients);

    if (randomRecipe == null)
    {
        throw new NotFoundException("No recipes found with the specified ingredients.");
    }

    return Mapper.RecipeToDto(randomRecipe);
}
}