using Microsoft.EntityFrameworkCore;
using stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeContext _context;

    public RecipeRepository(RecipeContext context)
    {
        _context = context;
    }

    public async Task<Recipe> AddRecipe(Recipe recipe)
    {
        try
        {
            var existingRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeName.ToLower() == recipe.RecipeName.ToLower());
            if (existingRecipe != null) return existingRecipe;

            var ingredientNames = recipe.Ingredients.Select(i => i.IngredientName).ToList();
            var existingIngredients = await _context.Ingredients.Where(i => ingredientNames.Contains(i.IngredientName)).ToListAsync();
            var newIngredients = recipe.Ingredients.Where(i => !existingIngredients.Any(ei => ei.IngredientName == i.IngredientName)).ToList();

            recipe.Ingredients = existingIngredients.Concat(newIngredients).ToList();
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding recipe", ex);
        }
    }

    public async Task<Recipe> AddIngredientToRecipe(int recipeId, int ingredientId)
    {
        try
        {
            var recipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe == null) throw new Exception("Recipe not found");

            var ingredient = await _context.Ingredients.FindAsync(ingredientId);
            if (ingredient == null || recipe.Ingredients.Contains(ingredient)) throw new Exception("Ingredient not found or already added");

            recipe.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return recipe;
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding ingredient to recipe", ex);
        }
    }

    public async Task<bool> DeleteRecipe(int id)
    {
        try
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting recipe", ex);
        }
    }

    public async Task<Recipe> GetRecipe(int id)
    {
        return await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recipe> GetRecipeByName(string name)
    {
        return await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.RecipeName.ToLower() == name.ToLower());
    }

    public async Task<List<Recipe>> GetAllRecipes()
    {
        return await _context.Recipes.Include(r => r.Ingredients).ToListAsync();
    }

    public async Task<Recipe> UpdateRecipe(Recipe recipe)
    {
        try
        {
            var existingRecipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.Id == recipe.Id);
            if (existingRecipe == null) throw new Exception("Recipe not found");

            existingRecipe.RecipeName = recipe.RecipeName;
            existingRecipe.Description = recipe.Description;
            existingRecipe.Image = recipe.Image;

            var ingredientNames = recipe.Ingredients.Select(i => i.IngredientName).ToList();
            var existingIngredients = await _context.Ingredients.Where(i => ingredientNames.Contains(i.IngredientName)).ToListAsync();
            var newIngredients = recipe.Ingredients.Where(i => !existingIngredients.Any(ei => ei.IngredientName == i.IngredientName)).ToList();

            existingRecipe.Ingredients = existingIngredients.Concat(newIngredients).ToList();
            await _context.SaveChangesAsync();
            return existingRecipe;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating recipe", ex);
        }
    }

    public async Task<bool> RemoveIngredientFromRecipe(int recipeId, int ingredientId)
    {
        try
        {
            var recipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe == null) return false;

            var ingredient = recipe.Ingredients.FirstOrDefault(i => i.Id == ingredientId);
            if (ingredient == null) return false;

            recipe.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error removing ingredient from recipe", ex);
        }
    }

    public async Task<List<Ingredient>> GetExistingIngredients(List<string> ingredientNames)
    {
        return await _context.Ingredients.Where(i => ingredientNames.Contains(i.IngredientName)).ToListAsync();
    }

   public async Task<Recipe> GetRandomRecipeAsync(List<string> requiredIngredients = null)
{
    var query = _context.Recipes
        .Include(r => r.Ingredients)
        .AsQueryable();

    if (requiredIngredients != null && requiredIngredients.Any())
    {
        query = query.Where(r => r.Ingredients.Any(i => requiredIngredients.Contains(i.IngredientName)));
    }

    if (!await query.AnyAsync())
    {
        return null; 
    }

    var randomRecipe = await query
        .OrderBy(r => Guid.NewGuid()) 
        .FirstOrDefaultAsync();

    return randomRecipe;
}
}