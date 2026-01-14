namespace Downey.ArenaAllocator
{
    public unsafe struct CraftNode
    {
        public ItemType outputType;
        public int amountNeeded;
        public int amountAvailable;
        public CraftNode** subIngredients;
        public int subCount;
    }
    public unsafe class CraftSimulator
    {
        private readonly RecipeBook book;
        readonly Inventory inventory;
        
        public CraftSimulator(RecipeBook book, Inventory inventory)
        {
            this.book = book;
            this.inventory = inventory;
        }

        public CraftNode* SimulateCraft(ArenaAllocator arena, ItemType item, int amountNeeded)
        {
            CraftNode* node = arena.Alloc<CraftNode>();
            node->outputType = item;
            node->amountNeeded = amountNeeded;

            if (book.TryGetRecipe(item, out var recipe))
            {
                node->subCount = recipe.ingredients.Length;
                node->subIngredients = (CraftNode**)arena.Alloc<byte>(sizeof(CraftNode*) * node->subCount);
                
                int maxCraftable = int.MaxValue;

                for (int i = 0; i < recipe.ingredients.Length; i++)
                {
                    var ingredient = recipe.ingredients[i];
                    int requiredAmount = ingredient.count * amountNeeded;
                    
                    CraftNode* sub = SimulateCraft(arena,ingredient.type, requiredAmount);
                    node->subIngredients[i] = sub;
                    
                    int possible = sub->amountAvailable/ingredient.count;
                    if (possible < maxCraftable) maxCraftable = possible;
                }
                node->amountAvailable = maxCraftable;
            }
            else
            {
                node->subCount = 0;
                node->subIngredients = null;
                node->amountAvailable = inventory.GetCount(item);
            }

            return node;
        }
    }
}