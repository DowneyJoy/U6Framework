using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Downey.Property
{
    public class Character
    {
        [CreateProperty] public Inventory Inventory { get; set; } = new();
    }

    public class Inventory
    {
        [CreateProperty]
        public List<Item> Items { get; set; } = new()
        {
            new Item {Name = "Sword",Rarity = "Common"},
            new Item {Name = "Bow",Rarity = "Rare"},
        };
    }

    public class Item
    {
        [CreateProperty] public string Name { get; set; }
        [CreateProperty,Bindable] public string Rarity { get; set; }
    }
    public class PrintPropertyVisitor : IPropertyVisitor,
        IPropertyBagVisitor
    {
        public void Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
        {
            var value = property.GetValue(ref container);
            Debug.Log($"{property.Name} = {value}");
        }

        public void Visit<TContainer>(IPropertyBag<TContainer> bag, ref TContainer container)
        {
            foreach (var property in bag.GetProperties(ref container))
            {
                property.Accept(this,ref container);
            }
        }
    }
}