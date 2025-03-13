using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using stuff;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly IIngredientService _ingredientService;

    public IngredientController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    [HttpGet]
    public async Task<ActionResult<List<IngredientDto>>> GetIngredients([FromQuery] string? name = null)
    {
    var ingredients = await _ingredientService.GetIngredients();

    if (!string.IsNullOrEmpty(name))
    {
        ingredients = ingredients
            .Where(i => i.IngredientName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (ingredients == null || ingredients.Count == 0)
    {
        return NotFound("No ingredients found.");
    }

    return Ok(ingredients);
}

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Ingredient ID must be greater than zero.");
        }

        var ingredient = await _ingredientService.GetIngredient(id);
        if (ingredient == null)
        {
            return NotFound($"Ingredient with id {id} not found.");
        }

        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody] IngredientDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            return BadRequest("Ingredient data is required.");
        }

        var addedIngredient = await _ingredientService.AddIngredient(ingredientDto);
        return CreatedAtAction(nameof(GetIngredient), new { id = addedIngredient.Id }, addedIngredient);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IngredientDto>> UpdateIngredient(int id, [FromBody] IngredientDto ingredientDto)
    {
        if (id <= 0)
        {
            return BadRequest("Ingredient ID must be greater than zero.");
        }

        if (ingredientDto == null)
        {
            return BadRequest("Ingredient data is required.");
        }

        if (id != ingredientDto.Id)
        {
            return BadRequest("The ingredient ID in the URL does not match the ID in the body.");
        }

        var updatedIngredient = await _ingredientService.UpdateIngredient(ingredientDto);
        if (updatedIngredient == null)
        {
            return NotFound($"Ingredient with id {id} not found.");
        }

        return Ok(updatedIngredient);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<IngredientDto>> DeleteIngredient(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Ingredient ID must be greater than zero.");
        }

        var deletedIngredient = await _ingredientService.DeleteIngredient(id);
        if (deletedIngredient == null)
        {
            return NotFound($"Ingredient with id {id} not found.");
        }

        return Ok(deletedIngredient);
    }
}
