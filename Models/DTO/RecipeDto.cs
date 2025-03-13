namespace stuff {
    public class RecipeDto {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
    }
}