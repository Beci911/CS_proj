using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stuff;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientService(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    public async Task<IngredientDto> AddIngredient(IngredientDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            throw new ArgumentNullException(nameof(ingredientDto));
        }

        var ingredient = Mapper.MapToIngredient(ingredientDto);
        var savedIngredient = await _ingredientRepository.AddIngredient(ingredient);

        if (savedIngredient == null)
        {
            throw new InvalidOperationException("Failed to add the ingredient.");
        }

        return Mapper.IngredientToDto(savedIngredient);
    }

    public async Task<IngredientDto> GetIngredient(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Ingredient ID must be greater than zero.", nameof(id));
        }

        var ingredient = await _ingredientRepository.GetIngredientById(id);
        return ingredient == null ? null : Mapper.IngredientToDto(ingredient);
    }

    public async Task<List<IngredientDto>> GetIngredients()
    {
        var ingredients = await _ingredientRepository.GetAllIngredients();
        return ingredients?.Select(Mapper.IngredientToDto).ToList() ?? new List<IngredientDto>();
    }

    public async Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            throw new ArgumentNullException(nameof(ingredientDto));
        }

        var ingredient = Mapper.MapToIngredient(ingredientDto);
        var updatedIngredient = await _ingredientRepository.UpdateIngredient(ingredient);

        if (updatedIngredient == null)
        {
            throw new InvalidOperationException("Failed to update the ingredient.");
        }

        return Mapper.IngredientToDto(updatedIngredient);
    }

    public async Task<IngredientDto> DeleteIngredient(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Ingredient ID must be greater than zero.", nameof(id));
        }

        var ingredient = await _ingredientRepository.RemoveIngredient(id);
        return ingredient == null ? null : Mapper.IngredientToDto(ingredient);
    }
}
