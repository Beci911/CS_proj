namespace stuff {
    public class Ingredient {
        public int Id { get; set; }
        public string IngredientName { get; set; }
        
        public List<Recipe> Recipes { get; set; }
    }
}