using System.Collections.Generic;
using System.Linq;
using stuff;

public static class Mapper
{
    public static RecipeDto RecipeToDto(Recipe recipe)
    {
        if (recipe == null) return null;

        return new RecipeDto
        {
            Id = recipe.Id,
            RecipeName = recipe.RecipeName,
            Description = recipe.Description,
            Image = recipe.Image,
            Ingredients = recipe.Ingredients?.Select(IngredientToDto).ToList() ?? new List<IngredientDto>()
        };
    }

    public static Recipe MapToRecipe(RecipeDto recipeDto)
    {
        if (recipeDto == null) return null;

        return new Recipe
        {
            Id = recipeDto.Id,
            RecipeName = recipeDto.RecipeName,
            Description = recipeDto.Description,
            Image = recipeDto.Image,
            Ingredients = recipeDto.Ingredients?.Select(MapToIngredient).ToList() ?? new List<Ingredient>()
        };
    }

    public static IngredientDto IngredientToDto(Ingredient ingredient)
    {
        if (ingredient == null) return null;

        return new IngredientDto
        {
            Id = ingredient.Id,
            IngredientName = ingredient.IngredientName
        };
    }

    public static Ingredient MapToIngredient(IngredientDto ingredientDto)
    {
        if (ingredientDto == null) return null;

        return new Ingredient
        {
            Id = ingredientDto.Id,
            IngredientName = ingredientDto.IngredientName
        };
    }
}