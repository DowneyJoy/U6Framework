using Unity.Properties;
using UnityEngine;

namespace Downey.Property
{
    public class BindableStringSetterVisitor:IPropertyBagVisitor,
        IPropertyVisitor
    {
        public string NewValue { get; set; }
        public void Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
        {
            foreach (var property in properties.GetProperties(ref container))
            {
                property.Accept(this,ref container);
            }
        }

        public void Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
        {
            if(typeof(TValue) != typeof(string))return;
            if(!property.HasAttribute<BindableAttribute>())return;

            var currentValue = property.GetValue(ref container);
            Debug.Log($"[Bindable] {property.Name} = {currentValue}");

            if (!string.IsNullOrEmpty(NewValue))
            {
                property.SetValue(ref container, (TValue)(object)NewValue);
                Debug.Log($"-> Update {property.Name} to: {NewValue}");
            }
        }
    }
}