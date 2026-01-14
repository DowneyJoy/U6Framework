using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Downey.ArenaAllocator
{
    public unsafe class Example : MonoBehaviour
    {
        private RecipeBook book;
        private Inventory inventory;
        ArenaAllocator arena;
        CraftSimulator simulator;

        void Start()
        {
            book = new RecipeBook();
            inventory = new Inventory();
            arena = new ArenaAllocator(UnsafeUtility.SizeOf<CraftNode>() * 10);
            simulator = new CraftSimulator(book, inventory);
            
            book.Add(new Recipe(ItemType.IronIngot,new []
            {
                new Ingredient{type = ItemType.IronOre,count = 2}
            }));
            
            book.Add(new Recipe(ItemType.IronSword,new []
            {
                new Ingredient{type = ItemType.IronIngot,count = 3},
                new Ingredient{type = ItemType.Stick,count = 1}
            }));
            
            inventory.Add(ItemType.IronOre,10);
            inventory.Add(ItemType.Stick,3);

            CraftNode* root = simulator.SimulateCraft(arena, ItemType.IronSword, 1);
            Debug.Log($"Can craft {root->outputType}: {root->amountAvailable}/{root->amountNeeded}");

            for (int i = 0; i < root->subCount; i++)
            {
                CraftNode* sub = root->subIngredients[i];
                Debug.Log($" -> {sub->outputType}: {sub->amountAvailable}/{sub->amountNeeded}");
            }
            arena.Reset();
        }
        
        void OnDestroy() => arena.Dispose();
    }
}