using System;
using Unity.Properties;
using UnityEngine;

namespace Downey.Property
{
    //[assembly: Unity.Properties.GeneratePropertyBagsForAssembly]
    [GeneratePropertyBag]
    public partial class MyData
    {
        [CreateProperty,Bindable]public int Score{get;set;}
        [CreateProperty]public string Name{get;set;}
    }
    
    public class BindableAttribute : Attribute {}

    public class Example : MonoBehaviour
    {
        void Start()
        {
            // MyData data = new MyData{Score = 42, Name = "James"};
            //
            // IPropertyBag<MyData> propertyBag = PropertyBag.GetPropertyBag<MyData>();
            //
            // foreach (IProperty<MyData> property in propertyBag.GetProperties(ref data))
            // {
            //     object value = property.GetValue(ref data);
            //     Debug.Log($"{property.Name} = {value}");
            // }
            //DebugUtilities.PrintBindablePaths(data);

            var path = PropertyPath.FromName("Inventory");
            path = PropertyPath.AppendName(path, "Items");
            path = PropertyPath.AppendIndex(path, 0);
            path = PropertyPath.AppendName(path, "Rarity");
            
            var character = new Character();
            //var visitor = new PrintPropertyVisitor();
            var visitor = new BindableStringSetterVisitor { NewValue = "Legendary" };
            PropertyContainer.Accept(visitor,ref character,path);
        }
    }
}