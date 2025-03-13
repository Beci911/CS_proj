using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using stuff;

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RecipeDto>>> GetAllRecipes()
    {
        var recipes = await _recipeService.GetAllRecipes();
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Recipe ID must be greater than zero.");
        }

        var recipe = await _recipeService.GetRecipe(id);
        if (recipe == null)
        {
            return NotFound($"Recipe with id {id} not found.");
        }

        return Ok(recipe);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<RecipeDto>> GetRecipeByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Recipe name is required.");
        }

        var recipe = await _recipeService.GetRecipeByName(name);
        if (recipe == null)
        {
            return NotFound($"Recipe with name {name} not found.");
        }

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDto>> AddRecipe([FromBody] RecipeDto recipeDto)
    {
        if (recipeDto == null)
        {
            return BadRequest("Recipe data is required.");
        }

        var addedRecipe = await _recipeService.AddRecipe(recipeDto);
        return CreatedAtAction(nameof(GetRecipe), new { id = addedRecipe.Id }, addedRecipe);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id, [FromBody] RecipeDto recipeDto)
    {
        if (id <= 0)
        {
            return BadRequest("Recipe ID must be greater than zero.");
        }

        if (recipeDto == null)
        {
            return BadRequest("Recipe data is required.");
        }

        if (id != recipeDto.Id)
        {
            return BadRequest("The recipe ID in the URL does not match the ID in the body.");
        }

        var updatedRecipe = await _recipeService.UpdateRecipe(recipeDto);
        if (updatedRecipe == null)
        {
            return NotFound($"Recipe with id {id} not found.");
        }

        return Ok(updatedRecipe);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipe(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Recipe ID must be greater than zero.");
        }

        var result = await _recipeService.DeleteRecipe(id);
        if (!result)
        {
            return NotFound($"Recipe with id {id} not found.");
        }

        return NoContent();
    }

    [HttpPost("{recipeId}/ingredients/{ingredientId}")]
    public async Task<ActionResult<RecipeDto>> AddIngredientToRecipe(int recipeId, int ingredientId)
    {
        if (recipeId <= 0 || ingredientId <= 0)
        {
            return BadRequest("Recipe ID and Ingredient ID must be greater than zero.");
        }

        var recipe = await _recipeService.AddIngredientToRecipe(recipeId, ingredientId);
        if (recipe == null)
        {
            return NotFound("Recipe or ingredient not found.");
        }

        return Ok(recipe);
    }

    [HttpDelete("{recipeId}/ingredients/{ingredientId}")]
    public async Task<ActionResult> RemoveIngredientFromRecipe(int recipeId, int ingredientId)
    {
        if (recipeId <= 0 || ingredientId <= 0)
        {
            return BadRequest("Recipe ID and Ingredient ID must be greater than zero.");
        }

        var result = await _recipeService.RemoveIngredientFromRecipe(recipeId, ingredientId);
        if (!result)
        {
            return NotFound("Recipe or ingredient not found.");
        }

        return NoContent();
    }

[HttpGet("suggestion")]
public async Task<ActionResult<RecipeDto>> GetRandomRecipe([FromQuery] string ingredients = null)
{
    try
    {
    
        var requiredIngredients = ingredients?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .ToList();

        var randomRecipe = await _recipeService.GetRandomRecipeAsync(requiredIngredients);
        return Ok(randomRecipe);
    }
    catch (NotFoundException ex)
    {
        return NotFound("No recipes found with the specified ingredients.");
    }
}
}