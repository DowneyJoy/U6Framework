using UnityEngine.UIElements;
using ZLinq;

namespace Downey.ZLinq
{
    public struct VisualElementTraverser : ITraverser<VisualElementTraverser, VisualElement>
    {
        readonly VisualElement current;
        private int nextChildIndex;
        private readonly int totalChildren;

        public VisualElementTraverser(VisualElement origin)
        {
            current = origin;
            nextChildIndex = 0;
            totalChildren = origin?.childCount ?? 0;
        }
        
        public VisualElement Origin => current;

        public VisualElementTraverser ConvertToTraverser(VisualElement next) => new (next);

        public bool TryGetParent(out VisualElement parent)
        {
            parent = current?.parent;
            return parent != null;
        }

        public bool TryGetHasChild(out bool hasChild)
        {
            hasChild = totalChildren > 0;
            return true;
        }

        public bool TryGetChildCount(out int count)
        {
            count = totalChildren;
            return true;
        }

        public bool TryGetNextChild(out VisualElement child)
        {
            if (nextChildIndex < totalChildren)
            {
                child = current[nextChildIndex++];
                return true;
            }
            
            child = null;
            return false;
        }

        public bool TryGetNextSibling(out VisualElement next)
        {
            if (current == null || current.parent == null)
            {
                next = null;
                return false;
            }
            
            var parent = current.parent;
            var index = parent.IndexOf(current);

            if (index >= 0 && index + 1 < parent.childCount)
            {
                next = parent[index + 1];
                return true;
            }
            next = null;
            return false;
        }

        public bool TryGetPreviousSibling(out VisualElement previous)
        {
            if (current == null || current.parent == null)
            {
                previous = null;
                return false;
            }
            
            var parent = current.parent;
            var index = parent.IndexOf(current);

            if (index > 0)
            {
                previous = parent[index - 1];
                return true;
            }
            previous = null;
            return false;
        }

        public void Dispose()
        {
            
        }
    }
}