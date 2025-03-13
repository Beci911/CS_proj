using stuff;
public interface IIngredientRepository
{
    Task<Ingredient> AddIngredient(Ingredient ingredient);
    Task<Ingredient> RemoveIngredient(int ingredientId);
    Task<List<Ingredient>> GetAllIngredients();
    Task<Ingredient> UpdateIngredient(Ingredient ingredient);
    Task<Ingredient> GetIngredientById(int ingredientId);
}
