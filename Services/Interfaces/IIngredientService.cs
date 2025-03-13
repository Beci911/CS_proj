using stuff;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IIngredientService
{
    Task<IngredientDto> AddIngredient(IngredientDto ingredientDto);
    Task<IngredientDto> GetIngredient(int id);
    Task<List<IngredientDto>> GetIngredients();
    Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto);
    Task<IngredientDto> DeleteIngredient(int id);
}

