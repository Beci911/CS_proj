using Microsoft.EntityFrameworkCore;
using stuff;
public class IngredientRepository : IIngredientRepository
{
    private readonly RecipeContext _context;

    public IngredientRepository(RecipeContext context)
    {
        _context = context;
    }

    public async Task<Ingredient> AddIngredient(Ingredient ingredient)
    {
        var existingIngredient = await _context.Ingredients
            .FirstOrDefaultAsync(i => i.IngredientName == ingredient.IngredientName);

        if (existingIngredient == null)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        return existingIngredient;
    }

    public async Task<Ingredient> RemoveIngredient(int ingredientId)
    {
        var ingredient = await _context.Ingredients
            .Include(i => i.Recipes)
            .FirstOrDefaultAsync(i => i.Id == ingredientId);

        if (ingredient == null)
        {
            return null;
        }

        foreach (var recipe in ingredient.Recipes)
        {
            recipe.Ingredients.Remove(ingredient);
        }

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();

        return ingredient;
    }

    public async Task<List<Ingredient>> GetAllIngredients()
    {
        return await _context.Ingredients.ToListAsync();
    }

    public async Task<Ingredient> UpdateIngredient(Ingredient ingredient)
    {
        var existingIngredient = await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredient.Id);

        if (existingIngredient == null)
        {
            return null;
        }

        _context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
        await _context.SaveChangesAsync();

        return existingIngredient;
    }

    public async Task<Ingredient> GetIngredientById(int ingredientId)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredientId);
    }
}
