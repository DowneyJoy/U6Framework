using System.Collections.Generic;

namespace Downey.ArenaAllocator
{
    public enum ItemType
    {
        IronOre,IronIngot,IronSword,Wood,Stick
    }

    public struct Ingredient
    {
        public ItemType type;
        public int count;
    }

    public class RecipeBook
    {
        readonly Dictionary<ItemType, Recipe> recipes = new();
        public void Add(Recipe recipe) => recipes[recipe.output] = recipe;
        public bool TryGetRecipe(ItemType type, out Recipe recipe) => recipes.TryGetValue(type, out recipe);
    }

    public class Recipe
    {
        public ItemType output;
        public Ingredient[] ingredients;

        public Recipe(ItemType output, Ingredient[] ingredients)
        {
            this.output = output;
            this.ingredients = ingredients;
        }
    }
    public class Inventory
    {
        private readonly Dictionary<ItemType, int> stock = new();

        public void Add(ItemType type, int count)
        {
            stock.TryAdd(type, 0);
            stock[type] += count;
        }
        
        public int GetCount(ItemType type) => stock.GetValueOrDefault(type,0);
    }
}