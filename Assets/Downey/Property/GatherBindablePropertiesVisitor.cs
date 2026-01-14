using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Downey.Property
{
    public class GatherBindablePropertiesVisitor : PropertyVisitor,
        IVisitPropertyAdapter<int>,
        IVisitPropertyAdapter<float>,
        IVisitPropertyAdapter<string>
    {
        public List<PropertyPath> BindableProperties { get; set; } = new();

        protected override void VisitProperty<TContainer, TValue>(
            Property<TContainer, TValue> property, 
            ref TContainer container, 
            ref TValue value)
        {
            if (property.HasAttribute<BindableAttribute>())
            {
                var path = PropertyPath.AppendProperty(default, property);
                BindableProperties.Add(path);
            }
            base.VisitProperty(property, ref container, ref value);
        }

        public void Visit<TContainer>(in VisitContext<TContainer, int> context, ref TContainer container, ref int value)
        {
            Debug.Log($"{context.Property.Name} = {value}");
        }

        public void Visit<TContainer>(in VisitContext<TContainer, float> context, ref TContainer container, ref float value)
        {
            Debug.Log($"{context.Property.Name} = {value:F2}");
        }

        public void Visit<TContainer>(in VisitContext<TContainer, string> context, ref TContainer container, ref string value)
        {
            Debug.Log($"{context.Property.Name} = \"{value}\"");
        }
    }
}